// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX.Direct3D;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12GraphicsDeviceFactory : GraphicsDeviceFactory
    {
        public readonly SharpDX.DXGI.Factory4 DXGIFactory;

        
        public D3D12GraphicsDeviceFactory(bool validation)
            : base(GraphicsBackend.Direct3D12, validation)
        {
#if DEBUG
            SharpDX.Configuration.EnableObjectTracking = true;
            SharpDX.Configuration.ThrowOnShaderCompileError = false;
#endif

            // Just try to enable debug layer.
            try
            {
                if (validation)
                {
                    // Enable the D3D12 debug layer.
                    DebugInterface.Get().EnableDebugLayer();

                    Validation = true;
                }
            }
            catch (SharpDX.SharpDXException)
            {
                Validation = false;
            }

            // Create factory first.
            DXGIFactory = new SharpDX.DXGI.Factory4(Validation);

            var adapterCount = DXGIFactory.GetAdapterCount1();
            for (var i = 0; i < adapterCount; i++)
            {
                var adapter = DXGIFactory.GetAdapter1(i);
                var desc = adapter.Description1;

                // Don't select the Basic Render Driver adapter.
                if ((desc.Flags & SharpDX.DXGI.AdapterFlags.Software) != SharpDX.DXGI.AdapterFlags.None)
                {
                    continue;
                }

                _adapters.Add(new DXGIAdapter(adapter));
            }
        }

        protected override void Destroy()
        {
            DXGIFactory.Dispose();
        }

        protected override GraphicsDevice CreateGraphicsDeviceImpl(GraphicsAdapter adapter, PresentationParameters presentationParameters)
        {
            return new D3D12GraphicsDevice(this, (DXGIAdapter)adapter, presentationParameters);
        }
    }
}
