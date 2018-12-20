// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace Vortice.Graphics.D3D11
{
    internal class TextureD3D11 : Texture
    {
        public readonly DXGI.Format DXGIFormat;
        public readonly Resource Resource;

        public TextureD3D11(D3D11GraphicsDevice device, in TextureDescription description, Resource nativeTexture)
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

                            Resource = new Texture1D(device.D3DDevice, d3dTextureDesc);
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

                            Resource = new Texture2D(device.D3DDevice, d3dTextureDesc);
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

                            Resource = new Texture3D(device.D3DDevice, d3dTextureDesc);
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
    }
}
