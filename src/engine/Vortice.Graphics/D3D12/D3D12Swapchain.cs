// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX;
using SharpDX.Mathematics.Interop;
using SharpDX.Direct3D12;
using System;
using DXGI = SharpDX.DXGI;

namespace Vortice.Graphics.D3D12
{
    internal unsafe class D3D12Swapchain : DXGISwapchain
    {
        public readonly DirectX12Texture[] _backbufferTextures;

        public D3D12Swapchain(
            DXGI.Factory2 factory,
            D3D12GraphicsDevice device,
            PresentationParameters presentationParameters)
            : base(device,
                  presentationParameters,
                  factory,
                  ((D3D12CommandQueue)device.GraphicsQueue).NativeQueue,
                  2)
        {
            _backbufferTextures = new DirectX12Texture[_frameCount];
            for (int i = 0; i < _frameCount; i++)
            {
                var backBufferTexture = _swapChain.GetBackBuffer<Resource>(i);
                var d3dTextureDesc = backBufferTexture.Description;
                var textureDescription = TextureDescription.Texture2D(
                    (int)d3dTextureDesc.Width,
                    d3dTextureDesc.Height,
                    d3dTextureDesc.MipLevels,
                    d3dTextureDesc.DepthOrArraySize,
                    DirectX12Convert.Convert(d3dTextureDesc.Format),
                    DirectX12Convert.Convert(d3dTextureDesc.Flags),
                    (SampleCount)d3dTextureDesc.SampleDescription.Count);
                _backbufferTextures[i] = new DirectX12Texture(device, textureDescription, backBufferTexture);
            }
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            base.Destroy();

            for (int i = 0; i < _frameCount; i++)
            {
                _backbufferTextures[i].Dispose();
            }
        }
    }
}
