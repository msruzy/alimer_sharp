// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace Vortice.Graphics.D3D11
{
    internal class D3D11Texture : Texture
    {
        public readonly DXGI.Format DXGIFormat;
        public readonly Resource Resource;

        public D3D11Texture(D3D11GraphicsDevice device, in TextureDescription description, Resource nativeTexture)
            : base(device, description)
        {
            DXGIFormat = D3DConvert.Convert(description.Format);
            if (nativeTexture == null)
            {
                // Create new one.
                var cpuFlags = CpuAccessFlags.None;
                var resourceUsage = ResourceUsage.Default;
                var bindFlags = D3D11Utils.Convert(description.TextureUsage, description.Format);
                var optionFlags = ResourceOptionFlags.None;

                var arraySize = description.ArrayLayers;
                if (description.TextureType == TextureType.TextureCube)
                {
                    arraySize *= 6;
                    optionFlags = ResourceOptionFlags.TextureCube;
                }

                switch (description.TextureType)
                {
                    case TextureType.Texture1D:
                        {
                            var d3dTextureDesc = new Texture1DDescription()
                            {
                                Width = description.Width,
                                MipLevels = description.MipLevels,
                                ArraySize = description.ArrayLayers,
                                Format = DXGIFormat,
                                BindFlags = bindFlags,
                                CpuAccessFlags = cpuFlags,
                                Usage = resourceUsage,
                                OptionFlags = optionFlags,
                            };

                            Resource = new Texture1D(device.NativeDevice, d3dTextureDesc);
                        }
                        break;

                    case TextureType.Texture2D:
                    case TextureType.TextureCube:
                        {
                            var d3dTextureDesc = new Texture2DDescription()
                            {
                                Width = description.Width,
                                Height = description.Height,
                                MipLevels = description.MipLevels,
                                ArraySize = description.ArrayLayers,
                                Format = DXGIFormat,
                                BindFlags = bindFlags,
                                CpuAccessFlags = cpuFlags,
                                Usage = resourceUsage,
                                SampleDescription = new DXGI.SampleDescription((int)description.Samples, 0),
                                OptionFlags = optionFlags,
                            };

                            Resource = new Texture2D(device.NativeDevice, d3dTextureDesc);
                        }
                        break;

                    case TextureType.Texture3D:
                        {
                            var d3dTextureDesc = new Texture3DDescription()
                            {
                                Width = description.Width,
                                Height = description.Height,
                                Depth = description.Depth,
                                MipLevels = description.MipLevels,
                                Format = DXGIFormat,
                                BindFlags = bindFlags,
                                CpuAccessFlags = cpuFlags,
                                Usage = resourceUsage,
                                OptionFlags = optionFlags,
                            };

                            Resource = new Texture3D(device.NativeDevice, d3dTextureDesc);
                        }
                        break;
                }
            }
            else
            {
                Resource = nativeTexture;
            }
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Resource.Dispose();
        }

        protected override TextureView CreateTextureViewCore(in TextureViewDescriptor descriptor)
        {
            return new D3D11TextureView(this, descriptor);
        }
    }

    internal class D3D11TextureView : TextureView
    {
        public readonly RenderTargetView RenderTargetView;

        public D3D11TextureView(D3D11Texture texture, in TextureViewDescriptor descriptor)
            : base(texture, descriptor)
        {
            var dxgiFormat = D3DConvert.Convert(descriptor.Format);
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
                    ((D3D11GraphicsDevice)texture.Device).NativeDevice,
                    texture.Resource,
                    viewDesc);
            }
        }

        protected internal override void Destroy()
        {
            RenderTargetView?.Dispose();
        }
    }
}
