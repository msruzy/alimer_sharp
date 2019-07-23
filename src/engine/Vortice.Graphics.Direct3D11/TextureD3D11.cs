// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Vortice.DirectX.Direct3D11;
using Vortice.DirectX.DXGI;

namespace Vortice.Graphics.Direct3D11
{
    internal class TextureD3D11 : Texture
    {
        public readonly ID3D11Resource Resource;
        public readonly Format DXGIFormat;
        //private readonly Dictionary<TextureViewDescriptor, TextureViewD3D11> _views = new Dictionary<TextureViewDescriptor, TextureViewD3D11>();

        public TextureD3D11(DeviceD3D11 device, ref TextureDescriptor descriptor, ID3D11Texture2D nativeTexture, Format dxgiFormat)
           : base(device, ref descriptor)
        {
            Resource = nativeTexture;
            DXGIFormat = dxgiFormat;
        }

        public TextureD3D11(DeviceD3D11 device, ref TextureDescriptor descriptor)
            : base(device, ref descriptor)
        {
            // Create new one.
            DXGIFormat = descriptor.Format.ToDirectX();

            var cpuFlags = CpuAccessFlags.None;
            var resourceUsage = Vortice.DirectX.Direct3D11.Usage.Default;
            var bindFlags = descriptor.Usage.ToDirectX(descriptor.Format);
            var optionFlags = ResourceOptionFlags.None;

            var arraySize = descriptor.ArrayLayers;
            if (descriptor.TextureType == TextureType.TextureCube)
            {
                arraySize *= 6;
                optionFlags = ResourceOptionFlags.TextureCube;
            }

            switch (descriptor.TextureType)
            {
                case TextureType.Texture1D:
                    {
                        var d3dTextureDesc = new Texture1DDescription()
                        {
                            Width = descriptor.Width,
                            MipLevels = descriptor.MipLevels,
                            ArraySize = descriptor.ArrayLayers,
                            Format = DXGIFormat,
                            BindFlags = bindFlags,
                            CpuAccessFlags = cpuFlags,
                            Usage = resourceUsage,
                            OptionFlags = optionFlags,
                        };

                        Resource = device.D3D11Device.CreateTexture1D(d3dTextureDesc);
                    }
                    break;

                case TextureType.Texture2D:
                case TextureType.TextureCube:
                    {
                        var d3dTextureDesc = new Texture2DDescription()
                        {
                            Width = descriptor.Width,
                            Height = descriptor.Height,
                            MipLevels = descriptor.MipLevels,
                            ArraySize = descriptor.ArrayLayers,
                            Format = DXGIFormat,
                            BindFlags = bindFlags,
                            CpuAccessFlags = cpuFlags,
                            Usage = resourceUsage,
                            SampleDescription = new SampleDescription((int)descriptor.Samples, 0),
                            OptionFlags = optionFlags,
                        };

                        Resource = device.D3D11Device.CreateTexture2D(d3dTextureDesc);
                    }
                    break;

                case TextureType.Texture3D:
                    {
                        var d3dTextureDesc = new Texture3DDescription()
                        {
                            Width = descriptor.Width,
                            Height = descriptor.Height,
                            Depth = descriptor.Depth,
                            MipLevels = descriptor.MipLevels,
                            Format = DXGIFormat,
                            BindFlags = bindFlags,
                            CpuAccessFlags = cpuFlags,
                            Usage = resourceUsage,
                            OptionFlags = optionFlags,
                        };

                        Resource = device.D3D11Device.CreateTexture3D(d3dTextureDesc);
                    }
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Resource.Dispose();
        }

        //public TextureViewD3D11 GetView(int mostDetailedMip = 0, int mipCount = TextureViewDescriptor.MaxPossible, int firstArraySlice = 0, int arraySize = TextureViewDescriptor.MaxPossible)
        //{
        //    var key = new TextureViewDescriptor(mostDetailedMip, mipCount, firstArraySlice, arraySize);
        //    if (!_views.TryGetValue(key, out TextureViewD3D11 view))
        //    {
        //        view = new TextureViewD3D11((DeviceD3D11)Device, this, key);
        //        _views.Add(key, view);
        //    }

        //    return view;
        //}
    }
}
