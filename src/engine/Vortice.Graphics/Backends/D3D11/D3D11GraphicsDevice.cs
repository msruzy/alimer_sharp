// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;

namespace Vortice.Graphics.D3D11
{
    internal unsafe class D3D11GraphicsDevice : IGraphicsDevice
    {
        public readonly Device1 Device;
        private readonly D3D11SwapChain _swapChain;

        public D3D11GraphicsDevice(PresentationParameters presentationParameters, bool validation)
        {
            var creationFlags = DeviceCreationFlags.BgraSupport | DeviceCreationFlags.VideoSupport;

            if (validation)
            {
                creationFlags |= DeviceCreationFlags.Debug;
            }

            try
            {
                using (var device = new Device(SharpDX.Direct3D.DriverType.Hardware, creationFlags))
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
                using (var device = new Device(SharpDX.Direct3D.DriverType.Hardware, creationFlags))
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

            _swapChain = new D3D11SwapChain(this, presentationParameters);
        }

        public void Destroy()
        {
            _swapChain.Destroy();
            Device.Dispose();
        }

        public void Present()
        {
            _swapChain.Present();
        }
    }
}
