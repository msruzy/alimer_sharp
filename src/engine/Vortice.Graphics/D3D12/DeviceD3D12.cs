// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D12;
using Vortice.Diagnostics;
using DXGI = SharpDX.DXGI;

namespace Vortice.Graphics.D3D12
{
    internal class DeviceD3D12 : GraphicsDevice
    {
        private static readonly FeatureLevel[] s_featureLevels = new FeatureLevel[]
        {
            FeatureLevel.Level_12_1,
            FeatureLevel.Level_12_0,
            FeatureLevel.Level_11_1,
            FeatureLevel.Level_11_0,
        };

        private static bool? s_isSupported;
        public const int RenderLatency = 2;
        public readonly DXGI.Factory4 DXGIFactory;
        public readonly DXGI.Adapter1 DXGIAdapter;
        public readonly Device D3DDevice;

        public readonly CommandQueue GraphicsQueue;

        public FeatureLevel FeatureLevel { get; private set; }

        private readonly List<IUnknown>[] _deferredReleases = new List<IUnknown>[RenderLatency];

        private readonly FenceD3D12 _frameFence;
        private long _currentFrameIndex;
        private long _currentCPUFrame;
        private long _currentGPUFrame;
        private bool _shuttingDown;

        private readonly DescriptorAllocator[] _descriptorAllocator = new DescriptorAllocator[(int)DescriptorHeapType.NumTypes];
        private readonly object _heapAllocationLock = new object();
        private readonly List<DescriptorHeap> _descriptorHeapPool = new List<DescriptorHeap>();

        /// <inheritdoc/>
        public override CommandBuffer ImmediateContext { get; }

        public long CurrentCPUFrame => _currentCPUFrame;
        public long CurrentGPUFrame => _currentGPUFrame;

        /// <summary>
        /// Check if given DirectX12 backend is supported.
        /// </summary>
        /// <returns>True if supported, false otherwise.</returns>
        public static bool IsSupported()
        {
            if (s_isSupported.HasValue)
                return s_isSupported.Value;

            if (Platform.PlatformType != PlatformType.Windows
                && Platform.PlatformType != PlatformType.UWP)
            {
                s_isSupported = false;
                return false;
            }

            try
            {
                using (var tempFactory = new DXGI.Factory1())
                {
                    var adapterCount = tempFactory.GetAdapterCount1();
                    for (var i = 0; i < adapterCount; i++)
                    {
                        var adapter = tempFactory.GetAdapter1(i);
                        var desc = adapter.Description1;

                        // Don't select the Basic Render Driver adapter.
                        if ((desc.Flags & SharpDX.DXGI.AdapterFlags.Software) != SharpDX.DXGI.AdapterFlags.None)
                        {
                            continue;
                        }

                        try
                        {
                            var tempDevice = new Device(adapter, FeatureLevel.Level_11_0);
                            tempDevice.Dispose();

                            s_isSupported = true;
                            return true;
                        }
                        catch (SharpDX.SharpDXException)
                        {
                        }
                    }
                }
            }
            catch (Exception)
            {
                s_isSupported = false;
                return false;
            }

            s_isSupported = true;
            return true;
        }

        public DeviceD3D12(bool validation)
            : base(GraphicsBackend.Direct3D12)
        {
#if DEBUG
            Configuration.EnableObjectTracking = true;
            Configuration.ThrowOnShaderCompileError = false;
#endif

            // Just try to enable debug layer.
            if (validation)
            {
                try
                {
                    // Enable the D3D12 debug layer.
                    DebugInterface.Get().EnableDebugLayer();

                    Validation = true;
                }
                catch (SharpDXException)
                {
                    Log.Warn("Direct3D Debug Device required but not available.");
                    Validation = false;
                }
            }

            // Create factory first.
            using (var factory = new DXGI.Factory2(Validation))
            {
                DXGIFactory = factory.QueryInterface<DXGI.Factory4>();
            }

            var adapterCount = DXGIFactory.GetAdapterCount1();
            for (var i = 0; i < adapterCount; i++)
            {
                var adapter = DXGIFactory.GetAdapter1(i);
                var desc = adapter.Description1;

                // Don't select the Basic Render Driver adapter.
                if ((desc.Flags & DXGI.AdapterFlags.Software) != DXGI.AdapterFlags.None)
                {
                    continue;
                }

                var tempDevice = new Device(adapter, FeatureLevel.Level_11_0);
                tempDevice.Dispose();

                DXGIAdapter = adapter;
            }

            try
            {
                D3DDevice = new Device(DXGIAdapter, FeatureLevel.Level_11_0);
            }
            catch (SharpDXException)
            {
                // Create the Direct3D 12 with WARP adapter.
                var warpAdapter = DXGIFactory.GetWarpAdapter();
                D3DDevice = new Device(warpAdapter, FeatureLevel.Level_11_0);
            }

            if (Validation)
            {
                var infoQueue = D3DDevice.QueryInterfaceOrNull<InfoQueue>();
                if (infoQueue != null)
                {
#if DEBUG
                    infoQueue.SetBreakOnSeverity(MessageSeverity.Corruption, true);
                    infoQueue.SetBreakOnSeverity(MessageSeverity.Error, true);
#endif

                    infoQueue.AddStorageFilterEntries(new InfoQueueFilter
                    {
                        DenyList = new InfoQueueFilterDescription
                        {
                            Ids = new[]
                            {
                                MessageId.ClearrendertargetviewMismatchingclearvalue,

                                // These happen when capturing with VS diagnostics
                                MessageId.MapInvalidNullRange,
                                MessageId.UnmapInvalidNullRange
                            }
                        }
                    });
                    infoQueue.Dispose();
                }
            }

            // Init device features.
            InitializeFeatures();

            // Create main graphics command queue.
            GraphicsQueue = D3DDevice.CreateCommandQueue(new CommandQueueDescription(CommandListType.Direct, CommandQueueFlags.None));
            GraphicsQueue.Name = "Main GraphicsQueue";

            // Create ImmediateContext.
            for (int i = 0; i < RenderLatency; i++)
            {
                _deferredReleases[i] = new List<IUnknown>();
            }
            ImmediateContext = new CommandBufferD3D12(this, RenderLatency, CommandListType.Direct);
            ((CommandBufferD3D12)ImmediateContext).CommandList.Name = "Primary Graphics Command List";

            // Create the frame fence
            _frameFence = new FenceD3D12(this, 0);

            // Create descriptor allocators
            for (var i = 0; i < (int)DescriptorHeapType.NumTypes; i++)
            {
                _descriptorAllocator[i] = new DescriptorAllocator(this, (DescriptorHeapType)i);
            }
        }

        protected override void Destroy()
        {
            WaitIdle();

            _shuttingDown = true;
            _frameFence.Dispose();
            GraphicsQueue.Dispose();

            // Clear DescriptorHeap Pools.
            lock (_heapAllocationLock)
            {
                foreach (var heap in _descriptorHeapPool)
                {
                    heap.Dispose();
                }

                _descriptorHeapPool.Clear();
            }

            if (Validation)
            {
                var debugDevice = D3DDevice.QueryInterfaceOrNull<DebugDevice>();
                if (debugDevice != null)
                {
                    debugDevice.ReportLiveDeviceObjects(RldoFlags.Detail);
                    debugDevice.Dispose();
                }
            }

            D3DDevice.Dispose();
        }

        private void InitializeFeatures()
        {
            var adapterDesc = DXGIAdapter.Description1;
            Features.VendorId = adapterDesc.VendorId;
            Features.DeviceId = adapterDesc.DeviceId;
            Features.DeviceName = adapterDesc.Description;
            Log.Debug($"Direct3D Adapter: VID:{adapterDesc.VendorId}, PID:{adapterDesc.DeviceId} - {adapterDesc.Description}");

            FeatureLevel = D3DDevice.CheckMaxSupportedFeatureLevel(s_featureLevels);
            var D3D12Options = D3DDevice.D3D12Options;

            var dataShaderModel = D3DDevice.CheckShaderModel(ShaderModel.Model60);
            //var dataShaderModel1 = D3DDevice.CheckShaderModel(ShaderModel.Model61);
            //var dataShaderModel2 = D3DDevice.CheckShaderModel(ShaderModel.Model62);
            var waveIntrinsicsSupport = D3DDevice.GetD3D12Options1();

            //Device.CheckFeatureSupport(Feature.D3D12Options1, ref waveIntrinsicsSupport);
            var featureDataRootSignature = new FeatureDataRootSignature
            {
                HighestVersion = RootSignatureVersion.Version11
            };

            if (!D3DDevice.CheckFeatureSupport(Feature.RootSignature, ref featureDataRootSignature))
            {
                featureDataRootSignature.HighestVersion = RootSignatureVersion.Version10;
            }

            var gpuVaSupport = D3DDevice.GetGpuVirtualAddressSupport();
        }

        protected override void FrameCore()
        {
            // Present the frame.
            //_mainSwapchain.Present();

            ++_currentCPUFrame;

            // Signal the fence with the current frame number, so that we can check back on it
            _frameFence.Signal(GraphicsQueue, _currentCPUFrame);

            // Wait for the GPU to catch up before we stomp an executing command buffer
            long gpuLag = _currentCPUFrame - _currentGPUFrame;
            Debug.Assert(gpuLag <= RenderLatency);
            if (gpuLag >= RenderLatency)
            {
                // Make sure that the previous frame is finished
                _frameFence.Wait(_currentGPUFrame + 1);
                ++_currentGPUFrame;
            }

            // See if we have any deferred releases to process
            ProcessDeferredReleases(_currentFrameIndex);
            //ProcessDeferredSRVCreates(_currentFrameIndex);
        }

        public DescriptorHandle AllocateDescriptor(DescriptorHeapType type, int count = 1)
        {
            return _descriptorAllocator[(int)type].Allocate(count);
        }

        public DescriptorHeap RequestNewHeap(DescriptorHeapType type, int descriptorCount)
        {
            lock (_heapAllocationLock)
            {
                var flags = DescriptorHeapFlags.None;

                if (type == DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView
                    || type == DescriptorHeapType.Sampler)
                {
                    flags = DescriptorHeapFlags.ShaderVisible;
                }

                var heapDescription = new DescriptorHeapDescription
                {
                    Type = type,
                    DescriptorCount = descriptorCount,
                    Flags = flags,
                    NodeMask = 0,
                };

                var heap = D3DDevice.CreateDescriptorHeap(heapDescription);
                _descriptorHeapPool.Add(heap);
                return heap;
            }
        }

        protected override GraphicsBuffer CreateBufferImpl(in BufferDescriptor descriptor, IntPtr initialData)
        {
            return new BufferD3D12(this, descriptor, initialData);
        }

        protected override Texture CreateTextureImpl(in TextureDescription description)
        {
            return new TextureD3D12(this, description, nativeTexture: null);
        }

        protected override Shader CreateShaderImpl(byte[] vertex, byte[] pixel)
        {
            throw new NotImplementedException();
        }

        protected override Framebuffer CreateFramebufferImpl(FramebufferAttachment[] colorAttachments, FramebufferAttachment? depthStencilAttachment)
        {
            return new FramebufferD3D12(this, colorAttachments, depthStencilAttachment);
        }

        protected override SwapChain CreateSwapChainImpl(in SwapChainDescriptor descriptor)
        {
            return new SwapchainD3D12(this, descriptor, 2);
        }

        public void DeferredRelease<T>(ref T resource, bool forceDeferred = false) where T : IUnknown
        {
            IUnknown unknown = resource;
            DeferredRelease_(unknown, forceDeferred);
            resource = default;
        }

        private void DeferredRelease_(IUnknown resource, bool forceDeferred = false)
        {
            if (resource == null)
                return;

            if ((_currentCPUFrame == _currentGPUFrame && !forceDeferred)
                || _shuttingDown
                || D3DDevice == null)
            {
                // Free-for-all!
                var count = resource.Release();
                Debug.Assert(count == 0);
                return;
            }

            _deferredReleases[_currentFrameIndex].Add(resource);
        }

        protected override void WaitIdleCore()
        {
            // Wait for the GPU to fully catch up with the CPU
            Debug.Assert(_currentCPUFrame >= _currentGPUFrame);
            if (_currentCPUFrame > _currentGPUFrame)
            {
                _frameFence.Wait(_currentCPUFrame);
                _currentGPUFrame = _currentCPUFrame;
            }

            // Clean up what we can now
            for (var i = 1; i < RenderLatency; ++i)
            {
                long frameIdx = (i + _currentFrameIndex) % RenderLatency;
                ProcessDeferredReleases(frameIdx);
                //ProcessDeferredSRVCreates(frameIdx);
            }
        }

        private void ProcessDeferredReleases(long frameIndex)
        {
            for (var i = 0; i < _deferredReleases[frameIndex].Count; ++i)
            {
                var count = _deferredReleases[frameIndex][i].Release();
                Debug.Assert(count == 0);
            }

            _deferredReleases[frameIndex].Clear();
        }
    }
}
