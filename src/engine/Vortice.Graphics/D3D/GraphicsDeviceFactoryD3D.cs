// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DirectX.Direct3D12.Debug;
using static Vortice.DirectX.DXGI.DXGI;
using static Vortice.DirectX.Direct3D12.D3D12;
using Vortice.DirectX.DXGI;

namespace Vortice.Graphics
{
    internal class GraphicsDeviceFactoryD3D : GraphicsDeviceFactory
    {
        public readonly IDXGIFactory1 DXGIFactory;

        public GraphicsDeviceFactoryD3D(GraphicsBackend backend, bool validation)
            : base(backend, validation)
        {
            switch (backend)
            {
                case GraphicsBackend.Direct3D11:
                    if (CreateDXGIFactory1(out DXGIFactory).Failure)
                    {
                        throw new GraphicsException("Cannot create IDXGIFactory1");
                    }
                    break;

                case GraphicsBackend.Direct3D12:
                    // Just try to enable debug layer.
                    if (validation
                        && D3D12GetDebugInterface<ID3D12Debug>(out var debug).Success)
                    {
                        // Enable the D3D12 debug layer.
                        debug.EnableDebugLayer();
                    }
                    else
                    {
                        Validation = false;
                    }

                    if (CreateDXGIFactory2(Validation, out IDXGIFactory4 dxgiFactory4).Failure)
                    {
                        throw new GraphicsException("Cannot create IDXGIFactory4");
                    }

                    DXGIFactory = dxgiFactory4;
                    break;
            }
        }

        protected override GraphicsDevice CreateDeviceImpl(PowerPreference powerPreference)
        {
            return new D3D12.DeviceD3D12((IDXGIFactory4)DXGIFactory);
        }
    }
}
