// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDirect3D12;

namespace Vortice.Graphics.D3D12
{
    internal unsafe class SwapchainD3D12 : SwapChainDXGI
    {
        private readonly TextureD3D12[] _backbufferTextures;

        public SwapchainD3D12(
            DeviceD3D12 device,
            SwapChainDescriptor descriptor,
            int backbufferCount)
            : base(device, descriptor, device.DXGIFactory, device.GraphicsQueue, backbufferCount, backbufferCount)
        {
            _backbufferTextures = new TextureD3D12[backbufferCount];
            for (int i = 0; i < backbufferCount; i++)
            {
                var backBufferTexture = _swapChain.GetBuffer<ID3D12Resource>(i);
                var d3dTextureDesc = backBufferTexture.Description;
                var textureDescription = TextureDescription.Texture2D(
                    (int)d3dTextureDesc.Width,
                    d3dTextureDesc.Height,
                    d3dTextureDesc.MipLevels,
                    d3dTextureDesc.DepthOrArraySize,
                    D3DConvert.Convert(d3dTextureDesc.Format),
                    D3D12Convert.Convert(d3dTextureDesc.Flags),
                    (SampleCount)d3dTextureDesc.SampleDescription.Count);

                _backbufferTextures[i] = new TextureD3D12(device, textureDescription, backBufferTexture);
            }

            // Configure base.
            Configure(descriptor);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            base.Destroy();
            for (int i = 0; i < BackBufferCount; i++)
            {
                _backbufferTextures[i].Dispose();
            }
        }

        /// <inheritdoc/>
        protected override Texture GetBackBufferTexture(int index)
        {
            return _backbufferTextures[index];
        }
    }
}
