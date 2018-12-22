// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX;
using SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;
using System.Diagnostics;
using static Vortice.Graphics.D3D11.Utils;

namespace Vortice.Graphics.D3D11
{
    internal unsafe class SwapchainD3D11 : SwapChainDXGI
    {
        public readonly TextureD3D11 BackbufferTexture;

        public SwapchainD3D11(GPUDeviceD3D11 device, in SwapChainDescriptor descriptor)
            : base(device.DXGIFactory, descriptor, device.D3DDevice, 2, 1)
        {
            var backBufferTexture = Resource.FromSwapChain<SharpDX.Direct3D11.Texture2D>(_swapChain, 0);
            BackbufferTexture = new TextureD3D11(device, backBufferTexture, BackBufferFormat);
        }

        /// <inheritdoc/>
        public override void Destroy()
        {
            base.Destroy();
            BackbufferTexture.Destroy();
        }

        /// <inheritdoc/>
        public override GPUTexture GetBackBufferTexture(int index)
        {
            Debug.Assert(index == 0);
            return BackbufferTexture;
        }
    }
}
