// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX;
using SharpDX.Mathematics.Interop;
using SharpDX.Direct3D11;
using System;
using DXGI = SharpDX.DXGI;
using System.Diagnostics;

namespace Vortice.Graphics.D3D11
{
    internal unsafe class SwapchainD3D11 : DXGISwapchain
    {
        public readonly TextureD3D11 BackbufferTexture;

        public SwapchainD3D11(D3D11GraphicsDevice device, PresentationParameters presentationParameters)
            : base(device, presentationParameters, device.D3DDevice, 2, 1)
        {
            var backBufferTexture = Resource.FromSwapChain<Texture2D>(_swapChain, 0);
            var d3dTextureDesc = backBufferTexture.Description;
            var textureDescription = TextureDescription.Texture2D(
                d3dTextureDesc.Width,
                d3dTextureDesc.Height,
                d3dTextureDesc.MipLevels,
                d3dTextureDesc.ArraySize,
                D3DConvert.Convert(d3dTextureDesc.Format),
                D3D11Utils.Convert(d3dTextureDesc.BindFlags),
                (SampleCount)d3dTextureDesc.SampleDescription.Count);

            BackbufferTexture = new TextureD3D11(device, textureDescription, backBufferTexture);
            Initialize(1);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            base.Destroy();
            BackbufferTexture.Dispose();
        }

        protected override Texture GetBackbufferTexture(int index)
        {
            Debug.Assert(index == 0);
            return BackbufferTexture;
        }
    }
}
