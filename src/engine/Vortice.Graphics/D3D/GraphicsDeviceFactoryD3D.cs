// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpD3D12.Debug;
using static SharpDXGI.DXGI;
using static SharpD3D12.D3D12;
using System.Diagnostics;
using SharpDXGI;

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
                    Debug.Assert(CreateDXGIFactory1(out DXGIFactory).Success);
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

                    Debug.Assert(CreateDXGIFactory2(Validation, out IDXGIFactory4 dxgiFactory4).Success);
                    DXGIFactory = dxgiFactory4;
                    break;
            }
        }

        protected override GraphicsDevice CreateDeviceImpl(PowerPreference powerPreference)
        {
            if (Backend == GraphicsBackend.Direct3D11)
            {
                return new D3D11.DeviceD3D11(DXGIFactory, Validation);
            }
            else
            {
                return new D3D12.DeviceD3D12((IDXGIFactory4)DXGIFactory);
            }
        }
    }
}
