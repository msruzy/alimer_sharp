// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D12;
using SharpDX.DXGI;

namespace Vortice.Graphics.D3D12
{
    internal static class DirectX12Convert
    {
        public static Format Convert(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R8UNorm:
                    return Format.R8_UNorm;

                case PixelFormat.RG8UNorm:
                    return Format.R8G8_UNorm;

                case PixelFormat.RGBA8UNorm:
                    return Format.R8G8B8A8_UNorm;

                case PixelFormat.BGRA8UNorm:
                    return Format.B8G8R8A8_UNorm;

                case PixelFormat.Depth16UNorm:
                    return Format.D16_UNorm;

                case PixelFormat.Depth24UNormStencil8:
                    return Format.D24_UNorm_S8_UInt;

                case PixelFormat.Depth32Float:
                    return Format.D32_Float;

                case PixelFormat.Depth32FloatStencil8:
                    return Format.D32_Float_S8X24_UInt;

                case PixelFormat.BC1:
                    return Format.BC1_UNorm;

                case PixelFormat.BC1_sRGB:
                    return Format.BC1_UNorm_SRgb;

                case PixelFormat.BC2:
                    return Format.BC2_UNorm;

                case PixelFormat.BC2_sRGB:
                    return Format.BC2_UNorm_SRgb;

                case PixelFormat.BC3:
                    return Format.BC3_UNorm;

                case PixelFormat.BC3_sRGB:
                    return Format.BC3_UNorm_SRgb;

                case PixelFormat.BC4UNorm:
                    return Format.BC4_UNorm;

                case PixelFormat.BC4SNorm:
                    return Format.BC4_SNorm;

                case PixelFormat.BC5UNorm:
                    return Format.BC5_UNorm;

                case PixelFormat.BC5SNorm:
                    return Format.BC5_SNorm;

                case PixelFormat.BC6HSFloat:
                    return Format.BC6H_Sf16;

                case PixelFormat.BC6HUFloat:
                    return Format.BC6H_Uf16;

                default:
                    return Format.Unknown;
            }
        }

        public static PixelFormat Convert(Format format)
        {
            switch (format)
            {
                case Format.R8_UNorm:
                    return PixelFormat.R8UNorm;

                case Format.R8G8_UNorm:
                    return PixelFormat.RG8UNorm;

                case Format.R8G8B8A8_UNorm:
                    return PixelFormat.RGBA8UNorm;

                case Format.B8G8R8A8_UNorm:
                    return PixelFormat.BGRA8UNorm;

                case Format.D16_UNorm:
                    return PixelFormat.Depth16UNorm;

                case Format.D24_UNorm_S8_UInt:
                    return PixelFormat.Depth24UNormStencil8;

                case Format.D32_Float:
                    return PixelFormat.Depth32Float;

                case Format.D32_Float_S8X24_UInt:
                    return PixelFormat.Depth32FloatStencil8;

                case Format.BC1_UNorm:
                    return PixelFormat.BC1;

                case Format.BC1_UNorm_SRgb:
                    return PixelFormat.BC1_sRGB;

                case Format.BC2_UNorm:
                    return PixelFormat.BC2;

                case Format.BC2_UNorm_SRgb:
                    return PixelFormat.BC2_sRGB;

                case Format.BC3_UNorm:
                    return PixelFormat.BC3;

                case Format.BC3_UNorm_SRgb:
                    return PixelFormat.BC3_sRGB;

                case Format.BC4_UNorm:
                    return PixelFormat.BC4UNorm;

                case Format.BC4_SNorm:
                    return PixelFormat.BC4SNorm;

                case Format.BC5_UNorm:
                    return PixelFormat.BC5UNorm;

                case Format.BC5_SNorm:
                    return PixelFormat.BC5SNorm;

                case Format.BC6H_Sf16:
                    return PixelFormat.BC6HSFloat;

                case Format.BC6H_Uf16:
                    return PixelFormat.BC6HUFloat;

                case Format.BC7_UNorm:

                default:
                    return PixelFormat.Unknown;
            }
        }

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
            var usage = TextureUsage.Unknown;

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
    }
}
