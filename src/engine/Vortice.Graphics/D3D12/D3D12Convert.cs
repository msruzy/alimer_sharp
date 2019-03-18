// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpD3D12;
using SharpDXGI.Direct3D;

namespace Vortice.Graphics.D3D12
{
    internal static class D3D12Convert
    {
        public static TextureType Convert(ResourceDimension dimension)
        {
            switch (dimension)
            {
                case ResourceDimension.Texture1D:
                    return TextureType.Texture1D;

                case ResourceDimension.Texture2D:
                    return TextureType.Texture2D;

                case ResourceDimension.Texture3D:
                    return TextureType.Texture3D;

                default:
                    return TextureType.Unknown;
            }
        }

        public static TextureUsage Convert(ResourceFlags flags)
        {
            var usage = TextureUsage.None;

            if ((flags & ResourceFlags.DenyShaderResource) == 0)
            {
                usage |= TextureUsage.ShaderRead;
            }

            if ((flags & ResourceFlags.AllowRenderTarget) != 0
                || (flags & ResourceFlags.AllowDepthStencil) != 0)
            {
                usage |= TextureUsage.RenderTarget;
            }

            if ((flags & ResourceFlags.AllowUnorderedAccess) != 0)
            {
                usage |= TextureUsage.ShaderWrite;
            }

            return usage;
        }

        public static ResourceFlags Convert(TextureUsage usage, PixelFormat format)
        {
            var flags = ResourceFlags.None;
            if ((usage & TextureUsage.ShaderRead) == 0)
            {
                flags |= ResourceFlags.DenyShaderResource;
            }

            if ((usage & TextureUsage.ShaderWrite) != 0)
            {
                flags |= ResourceFlags.AllowUnorderedAccess;
            }

            if ((usage & TextureUsage.RenderTarget) != 0)
            {
                if (!PixelFormatUtil.IsDepthStencilFormat(format))
                {
                    flags |= ResourceFlags.AllowRenderTarget;
                }
                else
                {
                    flags |= ResourceFlags.AllowDepthStencil;
                }
            }

            return flags;
        }

        public static unsafe FeatureLevel CheckMaxSupportedFeatureLevel(this ID3D12Device device, params FeatureLevel[] levels)
        {
            fixed (FeatureLevel* levelsPtr = &levels[0])
            {
                var featureData = new FeatureDataFeatureLevels
                {
                    NumFeatureLevels = levels.Length,
                    PFeatureLevelsRequested = new IntPtr(levelsPtr)
                };

                device.CheckFeatureSupport(Feature.FeatureLevels, ref featureData);
                return featureData.MaxSupportedFeatureLevel;
            }
        }

        public static unsafe FeatureDataShaderModel CheckShaderModel(this ID3D12Device device, ShaderModel highestShaderModel)
        {
            var featureData = new FeatureDataShaderModel
            {
                HighestShaderModel = highestShaderModel
            };
            device.CheckFeatureSupport(Feature.ShaderModel, ref featureData);
            return featureData;
        }

        public static unsafe FeatureDataD3D12Options1 GetD3D12Options1(this ID3D12Device device)
        {
            var featureData = new FeatureDataD3D12Options1();
            device.CheckFeatureSupport(Feature.D3D12Options1, ref featureData);
            return featureData;
        }

        public static unsafe FeatureDataGpuVirtualAddressSupport GetGpuVirtualAddressSupport(this ID3D12Device device)
        {
            var featureData = new FeatureDataGpuVirtualAddressSupport();
            device.CheckFeatureSupport(Feature.GpuVirtualAddressSupport, ref featureData);
            return featureData;
        }
    }
}
