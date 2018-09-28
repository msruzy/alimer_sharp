﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12GraphicsDevice : GraphicsDevice
    {
        private static readonly FeatureLevel[] s_featureLevels = new FeatureLevel[]
        {
            FeatureLevel.Level_12_1,
            FeatureLevel.Level_12_0,
            FeatureLevel.Level_11_1,
            FeatureLevel.Level_11_0,
        };

        private static bool? _isSupported;
        private readonly D3D12GraphicsDeviceFactory _factory;
        public readonly Device NativeDevice;
        public readonly FeatureLevel FeatureLevel;
        public readonly int RTVDescriptorSize;
        public readonly int DSVDescriptorSize;

        private readonly D3D12Swapchain _mainSwapchain;

        /// <inheritdoc/>
        public override Swapchain MainSwapchain => _mainSwapchain;

        /// <inheritdoc/>
        public override CommandQueue GraphicsQueue { get; }

        public D3D12GraphicsDevice(
            D3D12GraphicsDeviceFactory factory,
            DXGIAdapter adapter,
            PresentationParameters presentationParameters)
            : base(adapter, presentationParameters)
        {
            _factory = factory;
            try
            {
                NativeDevice = new Device(adapter.Adapter, FeatureLevel.Level_11_0);
            }
            catch (SharpDXException)
            {
                // Create the Direct3D 12 with WARP adapter.
                var warpAdapter = factory.DXGIFactory.GetWarpAdapter();
                NativeDevice = new Device(warpAdapter, FeatureLevel.Level_11_0);
            }

            if (_factory.Validation)
            {
                var infoQueue = NativeDevice.QueryInterfaceOrNull<InfoQueue>();
                if (infoQueue != null)
                {
                    infoQueue.SetBreakOnSeverity(MessageSeverity.Corruption, true);
                    infoQueue.SetBreakOnSeverity(MessageSeverity.Error, true);
                    infoQueue.AddStorageFilterEntries(new InfoQueueFilter
                    {
                        DenyList = new InfoQueueFilterDescription
                        {
                            Ids = new[]
                            {
                                MessageId.MapInvalidNullRange,
                                MessageId.UnmapInvalidNullRange
                            }
                        }
                    });
                    infoQueue.Dispose();
                }
            }

            // Init capabilities.
            unsafe
            {
                fixed (FeatureLevel* levelsPtr = &s_featureLevels[0])
                {
                    var featureLevels = new FeatureDataFeatureLevels
                    {
                        FeatureLevelCount = 4,
                        FeatureLevelsRequestedPointer = new IntPtr(levelsPtr)
                    };

                    Debug.Assert(NativeDevice.CheckFeatureSupport(Feature.FeatureLevels, ref featureLevels));
                    FeatureLevel = featureLevels.MaxSupportedFeatureLevel;
                }

                var options = NativeDevice.D3D12Options;
            }

            var featureDataRootSignature = new FeatureDataRootSignature
            {
                HighestVersion = RootSignatureVersion.Version11
            };

            if (!NativeDevice.CheckFeatureSupport(Feature.RootSignature, ref featureDataRootSignature))
            {
                featureDataRootSignature.HighestVersion = RootSignatureVersion.Version10;
            }

            var gpuVaSupport = default(FeatureDataGpuVirtualAddressSupport);
            NativeDevice.CheckFeatureSupport(Feature.GpuVirtualAddressSupport, ref gpuVaSupport);

            // Create direct command queue.
            GraphicsQueue = new D3D12CommandQueue(this, CommandListType.Direct);

            // Get descriptor heaps size.
            RTVDescriptorSize = NativeDevice.GetDescriptorHandleIncrementSize(DescriptorHeapType.RenderTargetView);
            DSVDescriptorSize = NativeDevice.GetDescriptorHandleIncrementSize(DescriptorHeapType.DepthStencilView);

            // Create main swap chain.
            _mainSwapchain = new D3D12Swapchain(_factory.DXGIFactory, this, presentationParameters);
        }

        protected override void Destroy()
        {
            //WaitIdle();
            ((D3D12CommandQueue)GraphicsQueue).Destroy();
            _mainSwapchain.Dispose();

            if (_factory.Validation)
            {
                var debugDevice = NativeDevice.QueryInterfaceOrNull<DebugDevice>();
                if (debugDevice != null)
                {
                    debugDevice.ReportLiveDeviceObjects(RldoFlags.Detail);
                    debugDevice.Dispose();
                }
            }

            NativeDevice.Dispose();
        }

        /// <summary>
        /// Check if given DirectX12 backend is supported.
        /// </summary>
        /// <returns>True if supported, false otherwise.</returns>
        public static bool IsSupported()
        {
            if (_isSupported.HasValue)
                return _isSupported.Value;

            if (Platform.PlatformType != PlatformType.Windows
                && Platform.PlatformType != PlatformType.UWP)
            {
                _isSupported = false;
                return false;
            }

            try
            {
                using (var tempFactory = new SharpDX.DXGI.Factory1())
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

                            _isSupported = true;
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
                _isSupported = false;
                return false;
            }

            _isSupported = true;
            return true;
        }


        //public override void WaitIdle()
        //{
        //CommandListManager.WaitIdle();
        //}

        protected override void PresentCore(Swapchain swapchain)
        {
            ((D3D12Swapchain)swapchain).Present();
        }

        protected override GraphicsBuffer CreateBufferCore(in BufferDescriptor descriptor, IntPtr initialData)
        {
            return new DirectX12Buffer(this, descriptor, initialData);
        }

        protected override Texture CreateTextureCore(in TextureDescription description)
        {
            return new DirectX12Texture(this, description, nativeTexture: null);
        }
    }
}
