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
    internal unsafe class D3D11GraphicsDevice : GraphicsDevice
    {
        private static readonly FeatureLevel[] s_featureLevels = new FeatureLevel[]
        {
            FeatureLevel.Level_11_1,
            FeatureLevel.Level_11_0,
            FeatureLevel.Level_10_1,
            FeatureLevel.Level_10_0
        };

        private static bool? _isSupported;
        public readonly Device D3DDevice;
        public readonly Device1 D3DDevice1;
        public readonly FeatureLevel FeatureLevel;
        public readonly DeviceContext D3DContext;
        public readonly DeviceContext1 D3DContext1;
        public readonly UserDefinedAnnotation D3DAnnotation;

        private readonly bool _supportsConcurrentResources;
        private readonly bool _supportsCommandLists;
        private readonly D3D11Swapchain _mainSwapchain;

        /// <inheritdoc/>
        public override Swapchain MainSwapchain => _mainSwapchain;

        public bool SupportsConcurrentResources => _supportsConcurrentResources;
        public bool SupportsCommandLists => _supportsCommandLists;

        public D3D11GraphicsDevice(bool validation, PresentationParameters presentationParameters)
            : base(GraphicsBackend.Direct3D11, presentationParameters)
        {
#if DEBUG
            SharpDX.Configuration.EnableObjectTracking = true;
            SharpDX.Configuration.ThrowOnShaderCompileError = false;
#endif
            // Create factory first.
            using (var dxgifactory = new DXGI.Factory1())
            {
                var adapterCount = dxgifactory.GetAdapterCount1();
                for (var i = 0; i < adapterCount; i++)
                {
                    var adapter = dxgifactory.GetAdapter1(i);
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

                    Features.VendorId = desc.VendorId;
                    Features.DeviceId = desc.DeviceId;
                    Features.DeviceName = desc.Description;
                    Log.Debug($"Direct3D Adapter ({i}): VID:{desc.VendorId}, PID:{desc.DeviceId} - {desc.Description}");
                    break;
                }
            }

            FeatureLevel = D3DDevice.FeatureLevel;
            D3DContext = D3DDevice.ImmediateContext;
            D3DDevice1 = D3DDevice.QueryInterfaceOrNull<Device1>();
            if (D3DDevice1 != null)
            {
                D3DContext1 = D3DContext.QueryInterface<DeviceContext1>();
                D3DAnnotation = D3DContext.QueryInterface<UserDefinedAnnotation>();
            }

            // Detect multithreading
            D3DDevice1.CheckThreadingSupport(out _supportsConcurrentResources, out _supportsCommandLists);
            if (_supportsConcurrentResources
                && _supportsCommandLists)
            {
                Features.Multithreading = true;
            }

            // Create queue's
            //GraphicsQueue = new D3D11CommandQueue(this);

            //ImmediateContext = new D3D11CommandContext(this, Device.ImmediateContext1);
            _mainSwapchain = new D3D11Swapchain(this, presentationParameters);
        }

        protected override void Destroy()
        {
            _mainSwapchain.Dispose();
            D3DContext.Dispose();
            D3DContext1?.Dispose();
            D3DAnnotation?.Dispose();
            D3DDevice1?.Dispose();

            var deviceDebug = D3DDevice.QueryInterfaceOrNull<DeviceDebug>();
            if (deviceDebug != null)
            {
                deviceDebug.ReportLiveDeviceObjects(ReportingLevel.Summary);
                deviceDebug.Dispose();
            }

            D3DDevice.Dispose();
        }

        /// <summary>
        /// Check if given DirectX11 backend is supported.
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

            FeatureLevel supportedFeatureLevel = 0;
            try
            {
                supportedFeatureLevel = SharpDX.Direct3D11.Device.GetSupportedFeatureLevel();
            }
            catch (SharpDX.SharpDXException)
            {
                // if GetSupportedFeatureLevel() fails, D3D11 is not supported.
                _isSupported = false;
                return false;
            }

            _isSupported = true;
            return true;
        }

        protected override void FrameCore()
        {
        }

        protected override void WaitIdleCore()
        {
        }

        protected override GraphicsBuffer CreateBufferCore(in BufferDescriptor descriptor, IntPtr initialData)
        {
            return new D3D11Buffer(this, descriptor, initialData);
        }

        protected override Texture CreateTextureCore(in TextureDescription description)
        {
            return new D3D11Texture(this, description, nativeTexture: null);
        }

        internal override IFramebuffer CreateFramebuffer(FramebufferAttachment[] colorAttachments)
        {
            throw new NotImplementedException();
        }
    }
}
