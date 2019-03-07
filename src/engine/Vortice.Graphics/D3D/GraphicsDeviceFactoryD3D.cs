// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX;
using SharpDX.Direct3D12;
using SharpDX.DXGI;
using Vortice.Diagnostics;

namespace Vortice.Graphics
{
    internal class GraphicsDeviceFactoryD3D : GraphicsDeviceFactory
    {
        public readonly Factory1 DXGIFactory;

        public GraphicsDeviceFactoryD3D(GraphicsBackend backend, bool validation)
            : base(backend, validation)
        {
#if DEBUG
            Configuration.EnableObjectTracking = true;
            Configuration.ThrowOnShaderCompileError = false;
#endif

            switch (backend)
            {
                case GraphicsBackend.Direct3D11:
                    DXGIFactory = new Factory1();
                    break;

                case GraphicsBackend.Direct3D12:
                    // Just try to enable debug layer.
                    if (validation)
                    {
                        try
                        {
                            // Enable the D3D12 debug layer.
                            DebugInterface.Get().EnableDebugLayer();

                            Validation = true;
                        }
                        catch (SharpDXException)
                        {
                            Log.Warn("Direct3D Debug Device required but not available.");
                            Validation = false;
                        }
                    }

                    using (var factory = new Factory2(Validation))
                    {
                        DXGIFactory = factory.QueryInterface<Factory4>();
                    }
                    break;
            }
        }
    }
}
