// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;

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

        private readonly D3D11GraphicsDeviceFactory _factory;
        public readonly Device1 Device;
        private readonly D3D11SwapChain _swapChain;

        public D3D11GraphicsDevice(
            D3D11GraphicsDeviceFactory factory,
            D3D11GpuAdapter adapter,
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
                    if (PlatformDetection.IsWindows10x)
                    {
                        Device = device.QueryInterface<Device5>();
                    }
                    else
                    {
                        Device = device.QueryInterface<Device1>();
                    }
                }
            }
            catch (SharpDXException)
            {
                // Remove debug flag not being supported.
                creationFlags &= ~DeviceCreationFlags.Debug;

                using (var device = new Device(adapter.Adapter, creationFlags, s_featureLevels))
                {
                    if (PlatformDetection.IsWindows10x)
                    {
                        Device = device.QueryInterface<Device5>();
                    }
                    else
                    {
                        Device = device.QueryInterface<Device1>();
                    }
                }

                _factory.Validation = false;
            }

            _swapChain = new D3D11SwapChain(this, presentationParameters);
        }

        protected override void Destroy()
        {
            _swapChain.Destroy();
            Device.Dispose();
        }

        protected override void PresentCore()
        {
            _swapChain.Present();
        }
    }
}
