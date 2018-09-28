// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX;
using SharpDX.Mathematics.Interop;
using SharpDX.Direct3D11;
using System;
using DXGI = SharpDX.DXGI;

namespace Vortice.Graphics.D3D11
{
    internal unsafe class D3D11Swapchain : DXGISwapchain
    {
        public readonly D3D11Texture BackbufferTexture;

        public D3D11Swapchain(DXGI.Factory2 factory, D3D11GraphicsDevice device, PresentationParameters presentationParameters)
            : base(device, presentationParameters, factory, device.NativeDevice, 2, 1)
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

            BackbufferTexture = new D3D11Texture(device, textureDescription, backBufferTexture);
            Initialize(new[] { BackbufferTexture });
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            base.Destroy();
            BackbufferTexture.Dispose();
        }
    }
}
