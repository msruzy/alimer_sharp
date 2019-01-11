// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;
using static Vortice.Graphics.D3D11.Utils;

namespace Vortice.Graphics.D3D11
{
    internal class TextureD3D11 : Texture
    {
        public readonly Resource Resource;
        public readonly DXGI.Format DXGIFormat;

        public TextureD3D11(DeviceD3D11 device, Texture2D nativeTexture, DXGI.Format dxgiFormat)
           : base(device, Convert(nativeTexture.Description))
        {
            Resource = nativeTexture;
            DXGIFormat = dxgiFormat;
        }

        public TextureD3D11(DeviceD3D11 device, in TextureDescription description)
            : base(device, description)
        {
            // Create new one.
            DXGIFormat = D3DConvert.Convert(description.Format);

            var cpuFlags = CpuAccessFlags.None;
            var resourceUsage = ResourceUsage.Default;
            var bindFlags = Convert(description.TextureUsage, description.Format);
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

                        Resource = new SharpDX.Direct3D11.Texture2D(device.D3DDevice, d3dTextureDesc);
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

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Resource.Dispose();
        }
    }
}
