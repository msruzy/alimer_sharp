// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX.Direct3D;
using SharpDX.Direct3D12;
using Vortice.Graphics.DirectX12;

namespace Vortice.Graphics
{
    /// <summary>
    /// DirectX12 <see cref="GraphicsDeviceFactory"/> implementation.
    /// </summary>
    public sealed class DirectX12GraphicsDeviceFactory : GraphicsDeviceFactory
    {
        private static bool? _isSupported;
        public readonly SharpDX.DXGI.Factory4 DXGIFactory;

        /// <summary>
        /// Check if given DirectX12 backend is supported.
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

            try
            {
                using (var tempFactory = new SharpDX.DXGI.Factory1())
                {
                    var adapterCount = tempFactory.GetAdapterCount1();
                    for (var i = 0; i < adapterCount; i++)
                    {
                        var adapter = tempFactory.GetAdapter1(i);
                        var desc = adapter.Description1;

                        // Don't select the Basic Render Driver adapter.
                        if ((desc.Flags & SharpDX.DXGI.AdapterFlags.Software) != SharpDX.DXGI.AdapterFlags.None)
                        {
                            continue;
                        }

                        try
                        {
                            var tempDevice = new Device(adapter, FeatureLevel.Level_11_0);
                            tempDevice.Dispose();

                            _isSupported = true;
                            return true;
                        }
                        catch (SharpDX.SharpDXException)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {
                _isSupported = false;
                return false;
            }

            _isSupported = true;
            return true;
        }

        public DirectX12GraphicsDeviceFactory(bool validation)
            : base(GraphicsBackend.DirectX12, validation)
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
            using (var tempFactory = new SharpDX.DXGI.Factory2(Validation))
            {
                DXGIFactory = tempFactory.QueryInterface<SharpDX.DXGI.Factory4>();
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

                _adapters.Add(new DirectX12GraphicsAdapter(adapter));
            }
        }

        protected override void Destroy()
        {
            DXGIFactory.Dispose();
        }

        protected override GraphicsDevice CreateGraphicsDeviceImpl(GraphicsAdapter adapter, PresentationParameters presentationParameters)
        {
            return new DirectX12GraphicsDevice(this, (DirectX12GraphicsAdapter)adapter, presentationParameters);
        }
    }
}
