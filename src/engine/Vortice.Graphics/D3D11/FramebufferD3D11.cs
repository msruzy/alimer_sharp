// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D11;

namespace Vortice.Graphics.D3D11
{
    internal class FramebufferD3D11 : IFramebuffer
    {
        public readonly D3D11GraphicsDevice Device;
        public readonly RenderTargetView[] RenderTargetViews;
        public DepthStencilView DepthStencilView;

        public FramebufferD3D11(D3D11GraphicsDevice device, FramebufferAttachment[] colorAttachments)
        {
            Device = device;

            if (colorAttachments.Length > 0)
            {
                RenderTargetViews = new RenderTargetView[colorAttachments.Length];
                for (var i = 0; i < colorAttachments.Length; i++)
                {
                    var texture = ((TextureD3D11)colorAttachments[i].Texture);

                    var renderTargetViewDesc = new RenderTargetViewDescription
                    {
                        Format = texture.DXGIFormat
                    };

                    switch (texture.TextureType)
                    {
                        case TextureType.Texture1D:
                            renderTargetViewDesc.Dimension = RenderTargetViewDimension.Texture1D;
                            break;

                        case TextureType.Texture2D:
                            renderTargetViewDesc.Dimension = RenderTargetViewDimension.Texture2D;
                            break;

                        case TextureType.Texture3D:
                            renderTargetViewDesc.Dimension = RenderTargetViewDimension.Texture3D;
                            break;

                        case TextureType.TextureCube:
                            renderTargetViewDesc.Dimension = RenderTargetViewDimension.Texture2DArray;
                            break;
                    }

                            /*
        if ((texture.TextureUsage & TextureUsage.RenderTarget) != 0)
                    {
                        var viewDesc = new RenderTargetViewDescription
                        {
                            Format = dxgiFormat
                        };

                        var mipLevel = 0;
                        var slice = 0;
                        var arrayLayers = texture.ArrayLayers;
                        var samples = texture.Samples;

                        switch (texture.TextureType)
                        {
                            case TextureType.Texture1D:
                                if (arrayLayers <= 1)
                                {
                                    viewDesc.Dimension = RenderTargetViewDimension.Texture1D;
                                    viewDesc.Texture1D.MipSlice = mipLevel;
                                }
                                else
                                {
                                    viewDesc.Dimension = RenderTargetViewDimension.Texture1DArray;
                                    viewDesc.Texture1DArray.MipSlice = mipLevel;
                                    viewDesc.Texture1DArray.FirstArraySlice = slice;
                                    viewDesc.Texture1DArray.ArraySize = arrayLayers;
                                }
                                break;

                            case TextureType.Texture2D:
                                if (arrayLayers <= 1)
                                {
                                    if (samples <= SampleCount.Count1)
                                    {
                                        viewDesc.Dimension = RenderTargetViewDimension.Texture2D;
                                        viewDesc.Texture2D.MipSlice = mipLevel;
                                    }
                                    else
                                    {
                                        viewDesc.Dimension = RenderTargetViewDimension.Texture2DMultisampled;
                                    }
                                }
                                else
                                {
                                    if (samples <= SampleCount.Count1)
                                    {
                                        viewDesc.Dimension = RenderTargetViewDimension.Texture2DArray;
                                        viewDesc.Texture2DArray.MipSlice = mipLevel;
                                        viewDesc.Texture2DArray.FirstArraySlice = slice;
                                        viewDesc.Texture2DArray.ArraySize = arrayLayers;
                                    }
                                    else
                                    {
                                        viewDesc.Dimension = RenderTargetViewDimension.Texture2DMultisampledArray;
                                        viewDesc.Texture2DMSArray.FirstArraySlice = slice;
                                        viewDesc.Texture2DMSArray.ArraySize = arrayLayers;
                                    }
                                }

                                break;

                            case TextureType.Texture3D:
                                // TODO: Add RenderPassAttachment.DepthPlane
                                viewDesc.Dimension = RenderTargetViewDimension.Texture3D;
                                viewDesc.Texture3D.MipSlice = mipLevel;
                                viewDesc.Texture3D.FirstDepthSlice = slice;
                                viewDesc.Texture3D.DepthSliceCount = arrayLayers;
                                break;

                            case TextureType.TextureCube:
                                // TODO.
                                viewDesc.Dimension = RenderTargetViewDimension.Texture2DArray;
                                break;
                        }

                        RenderTargetView = new RenderTargetView(
                            ((D3D11GraphicsDevice)texture.Device).D3DDevice,
                            texture.Resource,
                            viewDesc);
                    }*/

                    RenderTargetViews[i] = new RenderTargetView(device.D3DDevice, texture.Resource, renderTargetViewDesc);
                }
            }
        }

        public void Destroy()
        {
        }
    }
}
