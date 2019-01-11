// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D11;
using System.Diagnostics;

namespace Vortice.Graphics.D3D11
{
    internal unsafe class SwapchainD3D11 : SwapChainDXGI
    {
        public readonly TextureD3D11 BackbufferTexture;

        public SwapchainD3D11(DeviceD3D11 device, in SwapChainDescriptor descriptor)
            : base(device, descriptor, device.DXGIFactory, device.D3DDevice, 2, 1)
        {
            var backBufferTexture = Resource.FromSwapChain<Texture2D>(_swapChain, 0);
            BackbufferTexture = new TextureD3D11(device, backBufferTexture, BackBufferFormat);

            // Configure base.
            Configure(descriptor);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            base.Destroy();
            BackbufferTexture.Dispose();
        }

        /// <inheritdoc/>
        protected override Texture GetBackBufferTexture(int index)
        {
            Debug.Assert(index == 0);
            return BackbufferTexture;
        }
    }
}
