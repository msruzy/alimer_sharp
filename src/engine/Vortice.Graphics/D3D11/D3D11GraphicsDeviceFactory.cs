// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DirectX.DXGI;
using static Vortice.DirectX.DXGI.DXGI;

namespace Vortice.Graphics.D3D11
{
    internal class D3D11GraphicsDeviceFactory : GraphicsDeviceFactory
    {
        public readonly IDXGIFactory1 DXGIFactory;

        public D3D11GraphicsDeviceFactory(bool validation)
            : base(GraphicsBackend.Direct3D11, validation)
        {
            if (CreateDXGIFactory1(out DXGIFactory).Failure)
            {
                throw new GraphicsException("Cannot create IDXGIFactory1");
            }
        }

        protected override GraphicsDevice CreateDeviceImpl(PowerPreference powerPreference)
        {
            return new DeviceD3D11(DXGIFactory, Validation);
        }

        public static bool IsSupported()
        {
            return Platform.PlatformType == PlatformType.Windows
                || Platform.PlatformType == PlatformType.UWP;
        }
    }
}
