// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Runtime.InteropServices;
using SharpDX.Direct3D;

namespace Vortice.Graphics.D3D11
{
    internal class D3D11GraphicsDeviceFactory : IGraphicsDeviceFactory
    {
        private static bool? _isSupported;
        public readonly SharpDX.DXGI.Factory2 DXGIFactory;

        public bool Validation { get; set; }
        public List<GraphicsAdapter> Adapters { get; }

        public static bool IsSupported()
        {
            if (_isSupported.HasValue)
                return _isSupported.Value;

            if (Platform.PlatformType != PlatformType.Windows
                && Platform.PlatformType != PlatformType.WindowsUniversal)
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

        public D3D11GraphicsDeviceFactory(bool validation)
        {
            Validation = validation;

            // Create factory first.
            using (var tempFactory = new SharpDX.DXGI.Factory1())
            {
                DXGIFactory = tempFactory.QueryInterface<SharpDX.DXGI.Factory2>();
            }

            var adapterCount = DXGIFactory.GetAdapterCount1();
            Adapters = new List<GraphicsAdapter>(adapterCount);
            for (var i = 0; i < adapterCount; i++)
            {
                var adapter = DXGIFactory.GetAdapter1(i);
                var desc = adapter.Description1;

                // Don't select the Basic Render Driver adapter.
                if ((desc.Flags & SharpDX.DXGI.AdapterFlags.Software) != SharpDX.DXGI.AdapterFlags.None)
                {
                    continue;
                }

                Adapters.Add(new D3D11GpuAdapter(adapter));
            }
        }

        public void Destroy()
        {
            DXGIFactory.Dispose();
        }

        public GraphicsDevice CreateGraphicsDevice(GraphicsAdapter adapter, PresentationParameters presentationParameters)
        {
            return new D3D11GraphicsDevice(this, (D3D11GpuAdapter)adapter, presentationParameters);
        }

    }
}
