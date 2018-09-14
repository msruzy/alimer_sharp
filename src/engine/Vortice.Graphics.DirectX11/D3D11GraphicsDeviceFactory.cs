// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D;
using Vortice.Graphics.DirectX11;

namespace Vortice.Graphics
{
    /// <summary>
    /// DirectX11 <see cref="GraphicsDeviceFactory"/> implementation.
    /// </summary>
    public sealed class DirectX11GraphicsDeviceFactory : GraphicsDeviceFactory
    {
        private static bool? _isSupported;
        public readonly SharpDX.DXGI.Factory2 DXGIFactory;

        /// <summary>
        /// Check if given DirectX11 backend is supported.
        /// </summary>
        /// <returns>True if supported, false otherwise.</returns>
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

        public DirectX11GraphicsDeviceFactory(bool validation)
            : base(GraphicsBackend.DirectX11, validation)
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

                _adapters.Add(new DirectX11GpuAdapter(adapter));
            }
        }

        protected override void Destroy()
        {
            DXGIFactory.Dispose();
        }

        protected override GraphicsDevice CreateGraphicsDeviceImpl(GraphicsAdapter adapter, PresentationParameters presentationParameters)
        {
            return new D3D11GraphicsDevice(this, (DirectX11GpuAdapter)adapter, presentationParameters);
        }
    }
}
