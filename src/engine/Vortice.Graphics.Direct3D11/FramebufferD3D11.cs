// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.Direct3D11;
using Vortice.Diagnostics;

#if TODO
namespace Vortice.Graphics.Direct3D11
{
    internal class FramebufferD3D11 : Framebuffer
    {
        public readonly ID3D11RenderTargetView[] RenderTargetViews;
        public ID3D11DepthStencilView DepthStencilView;

        public FramebufferD3D11(DeviceD3D11 device, FramebufferAttachment[] colorAttachments, FramebufferAttachment? depthStencilAttachment)
            : base(device, colorAttachments, depthStencilAttachment)
        {
            if (colorAttachments.Length > 0)
            {
                RenderTargetViews = new ID3D11RenderTargetView[colorAttachments.Length];
                for (var i = 0; i < colorAttachments.Length; i++)
                {
                    var attachment = colorAttachments[i];
                    var texture = attachment.Texture;
                    var arraySize = texture.ArrayLayers - attachment.Slice;
                    bool isTextureMs = (int)texture.Samples > 1;

                    var d3dTexture = (TextureD3D11)texture;
                    var viewDesc = new RenderTargetViewDescription
                    {
                        Format = d3dTexture.DXGIFormat
                    };

                    switch (texture.TextureType)
                    {
                        case TextureType.Texture1D:
                            if (arraySize > 1)
                            {
                                viewDesc.ViewDimension = RenderTargetViewDimension.Texture1DArray;
                                viewDesc.Texture1DArray.MipSlice = attachment.MipLevel;
                                viewDesc.Texture1DArray.FirstArraySlice = attachment.Slice;
                                viewDesc.Texture1DArray.ArraySize = arraySize;
                            }
                            else
                            {
                                viewDesc.ViewDimension = RenderTargetViewDimension.Texture1D;
                                viewDesc.Texture1D.MipSlice = attachment.MipLevel;
                            }
                            break;

                        case TextureType.Texture2D:
                            if (arraySize > 1)
                            {
                                if (isTextureMs)
                                {
                                    viewDesc.ViewDimension = RenderTargetViewDimension.Texture2DMultisampledArray;
                                    viewDesc.Texture2DMSArray.FirstArraySlice = attachment.Slice;
                                    viewDesc.Texture2DMSArray.ArraySize = arraySize;
                                }
                                else
                                {
                                    viewDesc.ViewDimension = RenderTargetViewDimension.Texture2DArray;
                                    viewDesc.Texture2DArray.MipSlice = attachment.MipLevel;
                                    viewDesc.Texture2DArray.FirstArraySlice = attachment.Slice;
                                    viewDesc.Texture2DArray.ArraySize = arraySize;
                                }
                            }
                            else
                            {
                                if (isTextureMs)
                                {
                                    viewDesc.ViewDimension = RenderTargetViewDimension.Texture2DMultisampled;
                                }
                                else
                                {
                                    viewDesc.ViewDimension = RenderTargetViewDimension.Texture2D;
                                    viewDesc.Texture2D.MipSlice = attachment.MipLevel;
                                }
                            }
                            break;

                        case TextureType.Texture3D:
                            // TODO: Add RenderPassAttachment.DepthPlane
                            viewDesc.ViewDimension = RenderTargetViewDimension.Texture3D;
                            viewDesc.Texture3D.MipSlice = attachment.MipLevel;
                            viewDesc.Texture3D.FirstWSlice = attachment.Slice;
                            viewDesc.Texture3D.WSize = texture.Depth;
                            break;

                        case TextureType.TextureCube:
                            if (isTextureMs)
                            {
                                viewDesc.ViewDimension = RenderTargetViewDimension.Texture2DMultisampledArray;
                                viewDesc.Texture2DMSArray.FirstArraySlice = attachment.Slice * 6;
                                viewDesc.Texture2DMSArray.ArraySize = arraySize * 6;
                            }
                            else
                            {
                                viewDesc.ViewDimension = RenderTargetViewDimension.Texture2DArray;
                                viewDesc.Texture2DArray.MipSlice = attachment.MipLevel;
                                viewDesc.Texture2DArray.FirstArraySlice = attachment.Slice * 6;
                                viewDesc.Texture2DArray.ArraySize = arraySize * 6;
                            }
                            break;
                    }

                    RenderTargetViews[i] = device.Device.CreateRenderTargetView(d3dTexture.Resource, viewDesc);
                }
            }
            else
            {
                RenderTargetViews = new ID3D11RenderTargetView[0];
            }

            if (depthStencilAttachment != null)
            {
                var attachment = depthStencilAttachment.Value;
                var texture = attachment.Texture;
                var d3dTexture = (TextureD3D11)texture;
                var arraySize = texture.ArrayLayers - attachment.Slice;
                bool isTextureMs = (int)texture.Samples > 1;

                var viewDesc = new DepthStencilViewDescription
                {
                    Format = d3dTexture.DXGIFormat,
                    Flags = DepthStencilViewFlags.None
                };

                switch (texture.TextureType)
                {
                    case TextureType.Texture1D:
                        if (arraySize > 1)
                        {
                            viewDesc.ViewDimension = DepthStencilViewDimension.Texture1DArray;
                            viewDesc.Texture1DArray.MipSlice = attachment.MipLevel;
                            viewDesc.Texture1DArray.FirstArraySlice = attachment.Slice;
                            viewDesc.Texture1DArray.ArraySize = arraySize;
                        }
                        else
                        {
                            viewDesc.ViewDimension = DepthStencilViewDimension.Texture1D;
                            viewDesc.Texture1D.MipSlice = attachment.MipLevel;
                        }
                        break;
                    case TextureType.Texture2D:
                        if (arraySize > 1)
                        {
                            if (isTextureMs)
                            {
                                viewDesc.ViewDimension = DepthStencilViewDimension.Texture2DMultisampledArray;
                                viewDesc.Texture2DMSArray.FirstArraySlice = attachment.Slice;
                                viewDesc.Texture2DMSArray.ArraySize = arraySize;
                            }
                            else
                            {
                                viewDesc.ViewDimension = DepthStencilViewDimension.Texture2DArray;
                                viewDesc.Texture2DArray.MipSlice = attachment.MipLevel;
                                viewDesc.Texture2DArray.FirstArraySlice = attachment.Slice;
                                viewDesc.Texture2DArray.ArraySize = arraySize;
                            }
                        }
                        else
                        {
                            if (isTextureMs)
                            {
                                viewDesc.ViewDimension = DepthStencilViewDimension.Texture2DMultisampled;
                            }
                            else
                            {
                                viewDesc.ViewDimension = DepthStencilViewDimension.Texture2D;
                                viewDesc.Texture2D.MipSlice = attachment.MipLevel;
                            }
                        }

                        break;

                    case TextureType.TextureCube:
                        if (isTextureMs)
                        {
                            viewDesc.ViewDimension = DepthStencilViewDimension.Texture2DMultisampledArray;
                            viewDesc.Texture2DMSArray.FirstArraySlice = attachment.Slice * 6;
                            viewDesc.Texture2DMSArray.ArraySize = arraySize * 6;
                        }
                        else
                        {
                            viewDesc.ViewDimension = DepthStencilViewDimension.Texture2DArray;
                            viewDesc.Texture2DArray.MipSlice = attachment.MipLevel * 6;
                            viewDesc.Texture2DArray.FirstArraySlice = attachment.Slice * 6;
                            viewDesc.Texture2DArray.ArraySize = arraySize * 6;
                        }
                        break;

                    case TextureType.Texture3D:
                        viewDesc.ViewDimension = DepthStencilViewDimension.Texture2DArray;
                        viewDesc.Texture2DArray.MipSlice = attachment.MipLevel;
                        viewDesc.Texture2DArray.FirstArraySlice = attachment.Slice;
                        viewDesc.Texture2DArray.ArraySize = texture.Depth;
                        break;

                    default:
                        viewDesc.ViewDimension = DepthStencilViewDimension.Unknown;
                        Log.Error("Invalid texture type");
                        break;
                }

                DepthStencilView = device.Device.CreateDepthStencilView(d3dTexture.Resource, viewDesc);
            }
        }

        protected override void Destroy()
        {
            DepthStencilView?.Dispose();
            for (var i = 0; i < RenderTargetViews.Length; i++)
            {
                RenderTargetViews[i].Dispose();
            }
        }
    }
}

#endif
