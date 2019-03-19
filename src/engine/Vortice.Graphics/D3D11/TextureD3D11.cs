// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using static Vortice.Graphics.D3D11.Utils;
using System.Collections.Generic;
using SharpD3D11;
using SharpDXGI;

namespace Vortice.Graphics.D3D11
{
    internal class TextureD3D11 : Texture
    {
        public readonly ID3D11Resource Resource;
        public readonly Format DXGIFormat;
        private readonly Dictionary<TextureViewDescriptor, TextureViewD3D11> _views = new Dictionary<TextureViewDescriptor, TextureViewD3D11>();

        public TextureD3D11(DeviceD3D11 device, ID3D11Texture2D nativeTexture, Format dxgiFormat)
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
            var resourceUsage = SharpD3D11.Usage.Default;
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

                        Resource = device.Device.CreateTexture1D(d3dTextureDesc);
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
                            SampleDescription = new SampleDescription((int)description.Samples, 0),
                            OptionFlags = optionFlags,
                        };

                        Resource = device.Device.CreateTexture2D(d3dTextureDesc);
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

                        Resource = device.Device.CreateTexture3D(d3dTextureDesc);
                    }
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Resource.Dispose();
        }

        public TextureViewD3D11 GetView(int mostDetailedMip = 0, int mipCount = TextureViewDescriptor.MaxPossible, int firstArraySlice = 0, int arraySize = TextureViewDescriptor.MaxPossible)
        {
            var key = new TextureViewDescriptor(mostDetailedMip, mipCount, firstArraySlice, arraySize);
            if (!_views.TryGetValue(key, out TextureViewD3D11 view))
            {
                view = new TextureViewD3D11((DeviceD3D11)Device, this, key);
                _views.Add(key, view);
            }

            return view;
        }
    }
}
