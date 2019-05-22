// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using Vortice.DirectX.Direct3D11;
using Vortice.DirectX.Direct3D11.Debug;
using Vortice.DirectX.DXGI;
using Vortice.DirectX.Direct3D;
using Vortice.Diagnostics;
using static Vortice.DirectX.Direct3D11.D3D11;

namespace Vortice.Graphics.D3D11
{
    internal unsafe class DeviceD3D11 : GraphicsDevice
    {
        private static readonly FeatureLevel[] s_featureLevels = new FeatureLevel[]
        {
            FeatureLevel.Level_11_1,
            FeatureLevel.Level_11_0,
            FeatureLevel.Level_10_1,
            FeatureLevel.Level_10_0
        };

        public readonly IDXGIFactory1 DXGIFactory;
        public readonly IDXGIAdapter1 DXGIAdapter;

        public readonly ID3D11Device D3D11Device;
        public readonly FeatureLevel D3D11FeatureLevel;
        public readonly ID3D11DeviceContext D3D11DeviceContext;

        // Device1
        public readonly ID3D11Device1 D3D11Device1;

        public bool SupportsConcurrentResources { get; }
        public bool SupportsCommandLists { get; }

        public DeviceD3D11(IDXGIFactory1 factory, bool validation)
            : base(GraphicsBackend.Direct3D11)
        {
            DXGIFactory = factory;
            var adapters = DXGIFactory.EnumAdapters1();
            for (var i = 0; i < adapters.Length; i++)
            {
                var adapter = adapters[0];
                var desc = adapter.Description1;

                // Don't select the Basic Render Driver adapter.
                if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                {
                    continue;
                }

                var creationFlags = DeviceCreationFlags.BgraSupport/* | DeviceCreationFlags.VideoSupport*/;

                if (validation)
                {
                    creationFlags |= DeviceCreationFlags.Debug;
                }

                if (D3D11CreateDevice(
                    null,
                    DriverType.Hardware,
                    creationFlags,
                    s_featureLevels,
                    out D3D11Device,
                    out D3D11FeatureLevel,
                    out D3D11DeviceContext).Failure)
                {
                    // Remove debug flag not being supported.
                    creationFlags &= ~DeviceCreationFlags.Debug;

                    if (D3D11CreateDevice(null, DriverType.Hardware,
                        creationFlags, s_featureLevels,
                        out D3D11Device, out D3D11FeatureLevel, out D3D11DeviceContext).Failure)
                    {
                        throw new GraphicsException("Cannot create D3D11 Device");
                    }
                }

                DXGIAdapter = adapter;
                break;
            }

            D3D11Device1 = D3D11Device.QueryInterfaceOrNull<ID3D11Device1>();

            // Detect multithreading
            FeatureDataThreading featureDataThreading = default;
            if (D3D11Device.CheckFeatureSupport(Vortice.DirectX.Direct3D11.Feature.Threading, ref featureDataThreading))
            {
                SupportsConcurrentResources = featureDataThreading.DriverConcurrentCreates;
                SupportsCommandLists = featureDataThreading.DriverCommandLists;
            }

            // Init device features.
            InitializeFeatures();

            // Create command queue's.
            _graphicsCommandQueue = new CommandQueueD3D11(this, D3D11DeviceContext);
            _computeCommandQueue = new CommandQueueD3D11(this, CommandQueueType.Compute);
            _copyCommandQueue = new CommandQueueD3D11(this, CommandQueueType.Copy);
        }

        protected override void Destroy()
        {
            ((CommandQueueD3D11)_copyCommandQueue).Destroy();
            ((CommandQueueD3D11)_computeCommandQueue).Destroy();
            ((CommandQueueD3D11)_graphicsCommandQueue).Destroy();

            var deviceDebug = D3D11Device.QueryInterfaceOrNull<ID3D11Debug>();
            if (deviceDebug != null)
            {
                deviceDebug.ReportLiveDeviceObjects(ReportLiveDeviceObjectFlags.Summary);
                deviceDebug.Dispose();
            }

            D3D11Device1?.Dispose();
            D3D11Device.Dispose();
        }

        private void InitializeFeatures()
        {
            var adapterDesc = DXGIAdapter.Description1;
            Features.VendorId = adapterDesc.VendorId;
            Features.DeviceId = adapterDesc.DeviceId;
            Features.DeviceName = adapterDesc.Description;
            Log.Debug($"Direct3D Adapter: VID:{adapterDesc.VendorId}, PID:{adapterDesc.DeviceId} - {adapterDesc.Description}");

            if (SupportsConcurrentResources
                && SupportsCommandLists)
            {
                Features.Multithreading = true;
            }
        }

        protected override void FrameCore()
        {
            D3D11DeviceContext.Flush();
        }

        protected override void WaitIdleCore()
        {
            D3D11DeviceContext.Flush();
        }

        protected override SwapChain CreateSwapChainImpl(in SwapChainDescriptor descriptor)
        {
            return new SwapchainD3D11(this, descriptor);
        }

        protected override GraphicsBuffer CreateBufferImpl(in BufferDescriptor descriptor, IntPtr initialData)
        {
            return new BufferD3D11(this, descriptor, initialData);
        }

        protected override Texture CreateTextureImpl(in TextureDescription description)
        {
            return new TextureD3D11(this, description);
        }

        protected override Shader CreateShaderImpl(ShaderBytecode bytecode)
        {
            return new ShaderD3D11(this, bytecode);
        }

        protected override PipelineState CreateRenderPipelineStateImpl(in RenderPipelineDescriptor descriptor)
        {
            return new PipelineStateD3D11(this, descriptor);
        }
    }
}
