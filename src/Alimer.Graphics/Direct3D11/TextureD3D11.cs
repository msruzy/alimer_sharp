﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace Alimer.Graphics.Direct3D11
{
    internal class TextureD3D11 : Texture
    {
        public readonly ID3D11Resource Resource;
        public readonly Format DXGIFormat;
        private readonly Dictionary<TextureViewDescriptor, TextureViewD3D11> _views = new Dictionary<TextureViewDescriptor, TextureViewD3D11>();

        public TextureD3D11(DeviceD3D11 device, in TextureDescriptor descriptor, ID3D11Texture2D nativeTexture, Format dxgiFormat)
           : base(device, descriptor)
        {
            Resource = nativeTexture;
            DXGIFormat = dxgiFormat;
        }

        public TextureD3D11(DeviceD3D11 device, in TextureDescriptor descriptor)
            : base(device, descriptor)
        {
            // Create new one.
            DXGIFormat = descriptor.Format.ToDirectX();

            var cpuFlags = CpuAccessFlags.None;
            var resourceUsage = Vortice.Direct3D11.Usage.Default;
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
                            ArraySize = arraySize,
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

        protected override TextureView CreateView()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Texture"/> to <see cref="ID3D11Resource"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ID3D11Resource(TextureD3D11 value) => value.Resource;

        public TextureViewD3D11 GetView(int baseMipLevel = 0, int mipLevelCount = 0, int baseArrayLayer = 0, int arrayLayerCount = 0)
        {
            var key = new TextureViewDescriptor(PixelFormat.Undefined, baseMipLevel, mipLevelCount, baseArrayLayer, arrayLayerCount);
            if (!_views.TryGetValue(key, out TextureViewD3D11 view))
            {
                view = new TextureViewD3D11((DeviceD3D11)Device, this, key);
                _views.Add(key, view);
            }

            return view;
        }
    }
}
