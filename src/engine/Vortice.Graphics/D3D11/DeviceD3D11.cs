// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Vortice.Diagnostics;
using DXGI = SharpDX.DXGI;

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

        public readonly DXGI.Factory1 DXGIFactory;
        public readonly DXGI.Adapter1 DXGIAdapter;
        public readonly Device D3DDevice;
        public readonly Device1 D3DDevice1;
        public readonly FeatureLevel FeatureLevel;
        public readonly DeviceContext D3DImmediateContext;

        private readonly bool _supportsConcurrentResources;
        private readonly bool _supportsCommandLists;

        /// <inheritdoc/>
        public override CommandBuffer ImmediateCommandBuffer { get; }

        public bool SupportsConcurrentResources => _supportsConcurrentResources;
        public bool SupportsCommandLists => _supportsCommandLists;

        public DeviceD3D11(bool validation)
            : base(GraphicsBackend.Direct3D11)
        {
            // Create factory first.
            DXGIFactory = new DXGI.Factory1();
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

                var creationFlags = DeviceCreationFlags.BgraSupport/* | DeviceCreationFlags.VideoSupport*/;

                if (validation)
                {
                    creationFlags |= DeviceCreationFlags.Debug;
                }

                try
                {
                    D3DDevice = new Device(adapter, creationFlags, s_featureLevels);
                }
                catch (SharpDXException)
                {
                    // Remove debug flag not being supported.
                    creationFlags &= ~DeviceCreationFlags.Debug;

                    D3DDevice = new Device(adapter, creationFlags, s_featureLevels);
                }

                DXGIAdapter = adapter;
                break;
            }
            
            FeatureLevel = D3DDevice.FeatureLevel;
            D3DImmediateContext = D3DDevice.ImmediateContext;
            D3DDevice1 = D3DDevice.QueryInterfaceOrNull<Device1>();
            if (D3DDevice1 != null)
            {
                D3DImmediateContext = D3DImmediateContext.QueryInterface<DeviceContext1>();
            }

            // Detect multithreading
            D3DDevice1.CheckThreadingSupport(out _supportsConcurrentResources, out _supportsCommandLists);

            // Init device features.
            InitializeFeatures();

            // Create immediate context.
            ImmediateCommandBuffer = new CommandBufferD3D11(this, D3DImmediateContext);
        }

        protected override void Destroy()
        {
            ImmediateCommandBuffer.Dispose();
            D3DDevice1?.Dispose();

            var deviceDebug = D3DDevice.QueryInterfaceOrNull<DeviceDebug>();
            if (deviceDebug != null)
            {
                deviceDebug.ReportLiveDeviceObjects(ReportingLevel.Summary);
                deviceDebug.Dispose();
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

            if (_supportsConcurrentResources
                && _supportsCommandLists)
            {
                Features.Multithreading = true;
            }
        }

        protected override void FrameCore()
        {
            D3DImmediateContext.Flush();
        }

        protected override void WaitIdleCore()
        {
            D3DImmediateContext.Flush();
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
