// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DirectX.Direct3D12.Debug;
using static Vortice.DirectX.DXGI.DXGI;
using static Vortice.DirectX.Direct3D12.D3D12;
using Vortice.DirectX.DXGI;

namespace Vortice.Graphics.D3D12
{
    internal class GraphicsDeviceFactoryD3D12 : GraphicsDeviceFactory
    {
        public readonly IDXGIFactory4 DXGIFactory;

        public GraphicsDeviceFactoryD3D12(bool validation)
            : base(GraphicsBackend.Direct3D12, validation)
        {
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

            if (CreateDXGIFactory2(Validation, out DXGIFactory).Failure)
            {
                throw new GraphicsException("Cannot create IDXGIFactory4");
            }
        }

        protected override GraphicsDevice CreateDeviceImpl(PowerPreference powerPreference) => new DeviceD3D12(DXGIFactory);
    }
}
