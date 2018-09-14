// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace Vortice.Graphics.DirectX11
{
    internal class DirectX11Texture : Texture
    {
        public readonly DXGI.Format DXGIFormat;
        public readonly Resource Resource;
        private readonly Dictionary<RenderTargetViewEntry, RenderTargetView> _rtvViews = new Dictionary<RenderTargetViewEntry, RenderTargetView>();

        public DirectX11Texture(DirectX11GraphicsDevice device, in TextureDescription description, Resource nativeTexture)
            : base(device, description)
        {
            DXGIFormat = DirectX11Utils.Convert(description.Format);
            if (nativeTexture == null)
            {
                // Create new one.
                var cpuFlags = CpuAccessFlags.None;
                var resourceUsage = ResourceUsage.Default;
                var bindFlags = DirectX11Utils.Convert(description.TextureUsage, description.Format);
                var optionFlags = ResourceOptionFlags.None;

                var arraySize = description.ArrayLayers;
                if(description.TextureType == TextureType.TextureCube)
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

                            Resource = new Texture1D(device.Device, d3dTextureDesc);
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

                            Resource = new Texture2D(device.Device, d3dTextureDesc);
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

                            Resource = new Texture3D(device.Device, d3dTextureDesc);
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
            foreach (var rtvView in _rtvViews.Values)
            {
                rtvView.Dispose();
            }

            Resource.Dispose();
        }

        public RenderTargetView GetRenderTargetView(int mipLevel, int slice)
        {
            var viewEntry = new RenderTargetViewEntry(mipLevel, slice);
            if (!_rtvViews.TryGetValue(viewEntry, out var view))
            {
                // Not found, create new.
                var viewDesc = new RenderTargetViewDescription
                {
                    Format = DXGIFormat
                };

                var arrayLayers = ArrayLayers;
                var samples = Samples;

                switch (TextureType)
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

                view = new RenderTargetView(((DirectX11GraphicsDevice)Device).Device, Resource, viewDesc);
                _rtvViews.Add(viewEntry, view);
            }

            return view;
        }

        readonly struct RenderTargetViewEntry : IEquatable<RenderTargetViewEntry>
        {
            public readonly int MipLevel;
            public readonly int Slice;

            public RenderTargetViewEntry(int mipLevel, int slice)
            {
                MipLevel = mipLevel;
                Slice = slice;
            }

            public static bool operator ==(RenderTargetViewEntry left, RenderTargetViewEntry right) => left.Equals(right);
            public static bool operator !=(RenderTargetViewEntry left, RenderTargetViewEntry right) => !left.Equals(right);

            /// <inheritdoc />
            public bool Equals(RenderTargetViewEntry other) =>
                MipLevel == other.MipLevel
                && Slice == other.Slice;

            /// <inheritdoc/>
            public override bool Equals(object obj)
            {
                return obj is RenderTargetViewEntry other && this.Equals(other);
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = MipLevel.GetHashCode();
                    hashCode = (hashCode * 397) ^ Slice.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}
