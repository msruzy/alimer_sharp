// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using SharpD3D11;
using SharpD3D11.Debug;
using SharpDXGI;
using SharpDXGI.Direct3D;
using Vortice.Diagnostics;
using static SharpD3D11.D3D11;

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

        private static bool? _isSupported;

        public readonly IDXGIFactory1 DXGIFactory;
        public readonly IDXGIAdapter1 DXGIAdapter;

        public readonly ID3D11Device Device;
        public readonly FeatureLevel FeatureLevel;
        public readonly ID3D11DeviceContext DeviceContext;

        // Device1
        public readonly ID3D11Device1 Device1;

        private readonly bool _supportsConcurrentResources;
        private readonly bool _supportsCommandLists;

        /// <inheritdoc/>
        public override CommandBuffer ImmediateCommandBuffer { get; }

        public bool SupportsConcurrentResources => _supportsConcurrentResources;
        public bool SupportsCommandLists => _supportsCommandLists;

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
                    out Device,
                    out FeatureLevel,
                    out DeviceContext).Failure)
                {
                    // Remove debug flag not being supported.
                    creationFlags &= ~DeviceCreationFlags.Debug;

                    if (D3D11CreateDevice(null, DriverType.Hardware,
                        creationFlags, s_featureLevels,
                        out Device, out FeatureLevel, out DeviceContext).Failure)
                    {
                        throw new GraphicsException("Cannot create D3D11 Device");
                    }
                }

                DXGIAdapter = adapter;
                break;
            }

            Device1 = Device.QueryInterfaceOrNull<ID3D11Device1>();

            // Detect multithreading
            var featureDataThreading = default(FeatureDataThreading);
            if (Device.CheckFeatureSupport(SharpD3D11.Feature.Threading, ref featureDataThreading))
            {
                _supportsConcurrentResources = featureDataThreading.DriverConcurrentCreates;
                _supportsCommandLists = featureDataThreading.DriverCommandLists;
            }

            // Init device features.
            InitializeFeatures();

            // Create immediate context.
            ImmediateCommandBuffer = new CommandBufferD3D11(this, DeviceContext);
        }

        protected override void Destroy()
        {
            ImmediateCommandBuffer.Dispose();
            var deviceDebug = Device.QueryInterfaceOrNull<ID3D11Debug>();
            if (deviceDebug != null)
            {
                deviceDebug.ReportLiveDeviceObjects(ReportLiveDeviceObjectFlags.Summary);
                deviceDebug.Dispose();
            }

            Device1?.Dispose();
            Device.Dispose();
        }

        private void InitializeFeatures()
        {
            var adapterDesc = DXGIAdapter.Description1;
            Features.VendorId = adapterDesc.VendorId;
            Features.DeviceId = adapterDesc.DeviceId;
            Features.DeviceName = adapterDesc.Description;
            Log.Debug($"Direct3D Adapter: VID:{adapterDesc.VendorId}, PID:{adapterDesc.DeviceId} - {adapterDesc.Description}");

            if (_supportsConcurrentResources
                && _supportsCommandLists)
            {
                Features.Multithreading = true;
            }
        }

        protected override void FrameCore()
        {
            DeviceContext.Flush();
        }

        protected override void WaitIdleCore()
        {
            DeviceContext.Flush();
        }

        protected override GraphicsBuffer CreateBufferImpl(in BufferDescriptor descriptor, IntPtr initialData)
        {
            return null;
            //return new BufferD3D11(this, descriptor, initialData);
        }

        protected override Texture CreateTextureImpl(in TextureDescription description)
        {
            return new TextureD3D11(this, description);
        }

        protected override Shader CreateShaderImpl(byte[] vertex, byte[] pixel)
        {
            return null;
            //return new ShaderD3D11(this, vertex, pixel);
        }

        protected override Framebuffer CreateFramebufferImpl(FramebufferAttachment[] colorAttachments, FramebufferAttachment? depthStencilAttachment)
        {
            return new FramebufferD3D11(this, colorAttachments, depthStencilAttachment);
        }

        protected override SwapChain CreateSwapChainImpl(in SwapChainDescriptor descriptor)
        {
            return new SwapchainD3D11(this, descriptor);
        }
    }
}
