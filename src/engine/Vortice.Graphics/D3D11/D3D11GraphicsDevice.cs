// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

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
        private readonly D3D11GraphicsDeviceFactory _factory;
        public readonly Device1 NativeDevice;
        public readonly DeviceContext1 NativeImmediateContext;

        private readonly bool _supportsConcurrentResources;
        private readonly bool _supportsCommandLists;
        private readonly D3D11Swapchain _mainSwapchain;

        /// <inheritdoc/>
        public override Swapchain MainSwapchain => _mainSwapchain;

        /// <inheritdoc/>
        public override CommandQueue GraphicsQueue { get; }

        public bool SupportsConcurrentResources => _supportsConcurrentResources;
        public bool SupportsCommandLists => _supportsCommandLists;

        public D3D11GraphicsDevice(
            D3D11GraphicsDeviceFactory factory,
            DXGIAdapter adapter,
            PresentationParameters presentationParameters)
            : base(adapter, presentationParameters)
        {
            _factory = factory;
            var creationFlags = DeviceCreationFlags.BgraSupport/* | DeviceCreationFlags.VideoSupport*/;

            if (_factory.Validation)
            {
                creationFlags |= DeviceCreationFlags.Debug;
            }

            try
            {
                using (var device = new Device(adapter.Adapter, creationFlags, s_featureLevels))
                {
                    if (D3D11Utils.IsWindows10x)
                    {
                        NativeDevice = device.QueryInterface<Device5>();
                    }
                    else
                    {
                        NativeDevice = device.QueryInterface<Device1>();
                    }
                }
            }
            catch (SharpDXException)
            {
                // Remove debug flag not being supported.
                creationFlags &= ~DeviceCreationFlags.Debug;

                using (var device = new Device(adapter.Adapter, creationFlags, s_featureLevels))
                {
                    if (D3D11Utils.IsWindows10x)
                    {
                        NativeDevice = device.QueryInterface<Device5>();
                    }
                    else
                    {
                        NativeDevice = device.QueryInterface<Device1>();
                    }
                }
            }

            NativeDevice.CheckThreadingSupport(out _supportsConcurrentResources, out _supportsCommandLists);

            // Create queue's
            NativeImmediateContext = NativeDevice.ImmediateContext1;
            GraphicsQueue = new D3D11CommandQueue(this);

            //ImmediateContext = new D3D11CommandContext(this, Device.ImmediateContext1);
            _mainSwapchain = new D3D11Swapchain(_factory.DXGIFactory, this, presentationParameters);
        }

        protected override void Destroy()
        {
            _mainSwapchain.Dispose();
            NativeImmediateContext.Dispose();
            NativeDevice.Dispose();
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
            ((D3D11CommandQueue)GraphicsQueue).Tick();
        }

        protected override GraphicsBuffer CreateBufferCore(in BufferDescriptor descriptor, IntPtr initialData)
        {
            return new D3D11Buffer(this, descriptor, initialData);
        }

        protected override Texture CreateTextureCore(in TextureDescription description)
        {
            return new D3D11Texture(this, description, nativeTexture: null);
        }
    }
}
