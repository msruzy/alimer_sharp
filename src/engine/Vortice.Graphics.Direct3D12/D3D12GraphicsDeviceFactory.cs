// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.Direct3D12.Debug;
using Vortice.DXGI;
using static Vortice.DXGI.DXGI;
using static Vortice.Direct3D12.D3D12;
using Vortice.Direct3D12;
using Vortice.DirectX.Direct3D;
using System.Runtime.InteropServices;

namespace Vortice.Graphics.Direct3D12
{
    public class D3D12GraphicsDeviceFactory : GraphicsDeviceFactory
    {
        private static bool? s_isSupported;
        public readonly IDXGIFactory4 DXGIFactory;

        /// <summary>
        /// Checks if given DirectX12 backend is supported.
        /// </summary>
        /// <returns>True if supported, false otherwise.</returns>
        public static bool IsSupported()
        {
            if (s_isSupported.HasValue)
            {
                return s_isSupported.Value;
            }

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                s_isSupported = false;
                return false;
            }

            if (CreateDXGIFactory1(out IDXGIFactory1 tempDXGIFactory1).Failure)
            {
                s_isSupported = false;
                return false;
            }

            var adapters = tempDXGIFactory1.EnumAdapters1();
            for (var i = 0; i < adapters.Length; i++)
            {
                var adapter = adapters[i];
                var desc = adapter.Description1;

                // Don't select the Basic Render Driver adapter.
                if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                {
                    continue;
                }

                if (ID3D12Device.IsSupported(adapter, FeatureLevel.Level_11_0))
                {
                    s_isSupported = true;
                    return true;
                }
            }

            s_isSupported = true;
            return true;
        }

        public D3D12GraphicsDeviceFactory(bool validation)
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

        protected override GraphicsDevice CreateDeviceImpl(PowerPreference powerPreference) => new D3D12GraphicsDevice(DXGIFactory);
    }
}
