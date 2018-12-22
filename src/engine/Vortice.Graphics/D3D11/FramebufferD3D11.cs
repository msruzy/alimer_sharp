﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D11;
using Vortice.Diagnostics;

namespace Vortice.Graphics.D3D11
{
    internal class FramebufferD3D11 : GPUFramebuffer
    {
        public readonly GPUDeviceD3D11 Device;
        public readonly RenderTargetView[] RenderTargetViews;
        public DepthStencilView DepthStencilView;

        public FramebufferD3D11(GPUDeviceD3D11 device, FramebufferAttachment[] colorAttachments, FramebufferAttachment? depthStencilAttachment)
        {
            Device = device;

            if (colorAttachments.Length > 0)
            {
                RenderTargetViews = new RenderTargetView[colorAttachments.Length];
                for (var i = 0; i < colorAttachments.Length; i++)
                {
                    var attachment = colorAttachments[i];
                    var texture = attachment.Texture;
                    var arraySize = texture.ArrayLayers - attachment.Slice;
                    bool isTextureMs = (int)texture.Samples > 1;

                    var d3dTexture = (TextureD3D11)texture._backend;
                    var viewDesc = new RenderTargetViewDescription
                    {
                        Format = d3dTexture.DXGIFormat
                    };

                    switch (texture.TextureType)
                    {
                        case TextureType.Texture1D:
                            if (arraySize > 1)
                            {
                                viewDesc.Dimension = RenderTargetViewDimension.Texture1DArray;
                                viewDesc.Texture1DArray.MipSlice = attachment.MipLevel;
                                viewDesc.Texture1DArray.FirstArraySlice = attachment.Slice;
                                viewDesc.Texture1DArray.ArraySize = arraySize;
                            }
                            else
                            {
                                viewDesc.Dimension = RenderTargetViewDimension.Texture1D;
                                viewDesc.Texture1D.MipSlice = attachment.MipLevel;
                            }
                            break;

                        case TextureType.Texture2D:
                            if (arraySize > 1)
                            {
                                if (isTextureMs)
                                {
                                    viewDesc.Dimension = RenderTargetViewDimension.Texture2DMultisampledArray;
                                    viewDesc.Texture2DMSArray.FirstArraySlice = attachment.Slice;
                                    viewDesc.Texture2DMSArray.ArraySize = arraySize;
                                }
                                else
                                {
                                    viewDesc.Dimension = RenderTargetViewDimension.Texture2DArray;
                                    viewDesc.Texture2DArray.MipSlice = attachment.MipLevel;
                                    viewDesc.Texture2DArray.FirstArraySlice = attachment.Slice;
                                    viewDesc.Texture2DArray.ArraySize = arraySize;
                                }
                            }
                            else
                            {
                                if (isTextureMs)
                                {
                                    viewDesc.Dimension = RenderTargetViewDimension.Texture2DMultisampled;
                                }
                                else
                                {
                                    viewDesc.Dimension = RenderTargetViewDimension.Texture2D;
                                    viewDesc.Texture2D.MipSlice = attachment.MipLevel;
                                }
                            }
                            break;

                        case TextureType.Texture3D:
                            // TODO: Add RenderPassAttachment.DepthPlane
                            viewDesc.Dimension = RenderTargetViewDimension.Texture3D;
                            viewDesc.Texture3D.MipSlice = attachment.MipLevel;
                            viewDesc.Texture3D.FirstDepthSlice = attachment.Slice;
                            viewDesc.Texture3D.DepthSliceCount = texture.Depth;
                            break;

                        case TextureType.TextureCube:
                            if (isTextureMs)
                            {
                                viewDesc.Dimension = RenderTargetViewDimension.Texture2DMultisampledArray;
                                viewDesc.Texture2DMSArray.FirstArraySlice = attachment.Slice * 6;
                                viewDesc.Texture2DMSArray.ArraySize = arraySize * 6;
                            }
                            else
                            {
                                viewDesc.Dimension = RenderTargetViewDimension.Texture2DArray;
                                viewDesc.Texture2DArray.MipSlice = attachment.MipLevel;
                                viewDesc.Texture2DArray.FirstArraySlice = attachment.Slice * 6;
                                viewDesc.Texture2DArray.ArraySize = arraySize * 6;
                            }
                            break;
                    }

                    RenderTargetViews[i] = new RenderTargetView(device.D3DDevice, d3dTexture, viewDesc);
                }
            }

            if (depthStencilAttachment != null)
            {
                var attachment = depthStencilAttachment.Value;
                var texture = attachment.Texture;
                var d3dTexture = (TextureD3D11)texture._backend;
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
                            viewDesc.Dimension = DepthStencilViewDimension.Texture1DArray;
                            viewDesc.Texture1DArray.MipSlice = attachment.MipLevel;
                            viewDesc.Texture1DArray.FirstArraySlice = attachment.Slice;
                            viewDesc.Texture1DArray.ArraySize = arraySize;
                        }
                        else
                        {
                            viewDesc.Dimension = DepthStencilViewDimension.Texture1D;
                            viewDesc.Texture1D.MipSlice = attachment.MipLevel;
                        }
                        break;
                    case TextureType.Texture2D:
                        if (arraySize > 1)
                        {
                            if (isTextureMs)
                            {
                                viewDesc.Dimension = DepthStencilViewDimension.Texture2DMultisampledArray;
                                viewDesc.Texture2DMSArray.FirstArraySlice = attachment.Slice;
                                viewDesc.Texture2DMSArray.ArraySize = arraySize;
                            }
                            else
                            {
                                viewDesc.Dimension = DepthStencilViewDimension.Texture2DArray;
                                viewDesc.Texture2DArray.MipSlice = attachment.MipLevel;
                                viewDesc.Texture2DArray.FirstArraySlice = attachment.Slice;
                                viewDesc.Texture2DArray.ArraySize = arraySize;
                            }
                        }
                        else
                        {
                            if (isTextureMs)
                            {
                                viewDesc.Dimension = DepthStencilViewDimension.Texture2DMultisampled;
                            }
                            else
                            {
                                viewDesc.Dimension = DepthStencilViewDimension.Texture2D;
                                viewDesc.Texture2D.MipSlice = attachment.MipLevel;
                            }
                        }

                        break;

                    case TextureType.TextureCube:
                        if (isTextureMs)
                        {
                            viewDesc.Dimension = DepthStencilViewDimension.Texture2DMultisampledArray;
                            viewDesc.Texture2DMSArray.FirstArraySlice = attachment.Slice * 6;
                            viewDesc.Texture2DMSArray.ArraySize = arraySize * 6;
                        }
                        else
                        {
                            viewDesc.Dimension = DepthStencilViewDimension.Texture2DArray;
                            viewDesc.Texture2DArray.MipSlice = attachment.MipLevel * 6;
                            viewDesc.Texture2DArray.FirstArraySlice = attachment.Slice * 6;
                            viewDesc.Texture2DArray.ArraySize = arraySize * 6;
                        }
                        break;

                    case TextureType.Texture3D:
                        viewDesc.Dimension = DepthStencilViewDimension.Texture2DArray;
                        viewDesc.Texture2DArray.MipSlice = attachment.MipLevel;
                        viewDesc.Texture2DArray.FirstArraySlice = attachment.Slice;
                        viewDesc.Texture2DArray.ArraySize = texture.Depth;
                        break;

                    default:
                        viewDesc.Dimension = DepthStencilViewDimension.Unknown;
                        Log.Error("Invalid texture type");
                        break;
                }

                DepthStencilView = new DepthStencilView(device.D3DDevice, d3dTexture, viewDesc);
            }
        }

        public override void Destroy()
        {
        }
    }
}