// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Diagnostics;
using Vortice.DirectX.Direct3D11;

namespace Vortice.Graphics.D3D11
{
    internal unsafe class SwapchainD3D11 : SwapChainDXGI
    {
        public readonly TextureD3D11 BackbufferTexture;

        public SwapchainD3D11(DeviceD3D11 device, in SwapChainDescriptor descriptor)
            : base(device, descriptor, device.DXGIFactory, device.D3D11Device, 2, 1)
        {
            var backBufferTexture = _swapChain.GetBuffer<ID3D11Texture2D>(0);
            var textureDescriptor = Utils.Convert(backBufferTexture.Description);
            BackbufferTexture = new TextureD3D11(device, ref textureDescriptor, backBufferTexture, BackBufferFormat);

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
