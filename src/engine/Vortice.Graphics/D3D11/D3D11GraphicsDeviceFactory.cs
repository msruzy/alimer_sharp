// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D;

namespace Vortice.Graphics.D3D11
{
    internal class D3D11GraphicsDeviceFactory : GraphicsDeviceFactory
    {
        public readonly SharpDX.DXGI.Factory2 DXGIFactory;

        public D3D11GraphicsDeviceFactory(bool validation)
            : base(GraphicsBackend.Direct3D11, validation)
        {
#if DEBUG
            SharpDX.Configuration.EnableObjectTracking = true;
            SharpDX.Configuration.ThrowOnShaderCompileError = false;
#endif
            // Create factory first.
            using (var tempFactory = new SharpDX.DXGI.Factory1())
            {
                DXGIFactory = tempFactory.QueryInterface<SharpDX.DXGI.Factory2>();
            }

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
            return new D3D11GraphicsDevice(this, (DXGIAdapter)adapter, presentationParameters);
        }
    }
}
