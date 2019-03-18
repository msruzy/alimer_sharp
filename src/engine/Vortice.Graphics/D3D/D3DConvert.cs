// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Drawing;
using System.Numerics;
using SharpDXGI;

namespace Vortice.Graphics
{
    internal static class D3DConvert
    {
        public static Vector4 Convert(in Color4 color) => new Vector4(color.R, color.G, color.B, color.A);
        //public static Color4 Convert(in RawColor4 color) => new Color4(color.R, color.G, color.B, color.A);

        public static Format Convert(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R8UNorm:
                    return Format.R8_UNorm;

                case PixelFormat.R8SNorm:
                    return Format.R8_SNorm;

                case PixelFormat.R16UNorm:
                    return Format.R16_UNorm;

                case PixelFormat.R16SNorm:
                    return Format.R16_SNorm;

                case PixelFormat.RG8UNorm:
                    return Format.R8G8_UNorm;

                case PixelFormat.RG8SNorm:
                    return Format.R8G8_SNorm;

                case PixelFormat.RG16UNorm:
                    return Format.R16G16_UNorm;

                case PixelFormat.RG16SNorm:
                    return Format.R16G16_SNorm;

                case PixelFormat.RGBA8UNorm:
                    return Format.R8G8B8A8_UNorm;

                case PixelFormat.RGBA8UNormSrgb:
                    return Format.R8G8B8A8_UNorm_SRgb;

                case PixelFormat.RGBA8SNorm:
                    return Format.R8G8B8A8_SNorm;

                case PixelFormat.BGRA8UNorm:
                    return Format.B8G8R8A8_UNorm;

                case PixelFormat.BGRA8UNormSrgb:
                    return Format.B8G8R8A8_UNorm_SRgb;

                // Depth
                case PixelFormat.Depth16UNorm:
                    return Format.D16_UNorm;

                case PixelFormat.Depth24UNormStencil8:
                    return Format.D24_UNorm_S8_UInt;

                case PixelFormat.Depth32Float:
                    return Format.D32_Float;

                case PixelFormat.Depth32FloatStencil8:
                    return Format.D32_Float_S8X24_UInt;

                case PixelFormat.BC1UNorm:
                    return Format.BC1_UNorm;

                case PixelFormat.BC1UNormSrgb:
                    return Format.BC1_UNorm_SRgb;

                case PixelFormat.BC2UNorm:
                    return Format.BC2_UNorm;

                case PixelFormat.BC2UNormSrgb:
                    return Format.BC2_UNorm_SRgb;

                case PixelFormat.BC3UNorm:
                    return Format.BC3_UNorm;

                case PixelFormat.BC3UNormSrgb:
                    return Format.BC3_UNorm_SRgb;

                case PixelFormat.BC4UNorm:
                    return Format.BC4_UNorm;

                case PixelFormat.BC4SNorm:
                    return Format.BC4_SNorm;

                case PixelFormat.BC5UNorm:
                    return Format.BC5_UNorm;

                case PixelFormat.BC5SNorm:
                    return Format.BC5_SNorm;

                case PixelFormat.BC6HS16:
                    return Format.BC6H_Sf16;

                case PixelFormat.BC6HU16:
                    return Format.BC6H_Uf16;

                case PixelFormat.BC7UNorm:
                    return Format.BC7_UNorm;

                case PixelFormat.BC7UNormSrgb:
                    return Format.BC7_UNorm_SRgb;

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

                case Format.R8_SNorm:
                    return PixelFormat.R8SNorm;

                case Format.R16_UNorm:
                    return PixelFormat.R16UNorm;

                case Format.R16_SNorm:
                    return PixelFormat.R16SNorm;

                case Format.R8G8_UNorm:
                    return PixelFormat.RG8UNorm;

                case Format.R8G8_SNorm:
                    return PixelFormat.RG8SNorm;

                case Format.R16G16_UNorm:
                    return PixelFormat.RG16UNorm;

                case Format.R16G16_SNorm:
                    return PixelFormat.RG16SNorm;

                case Format.R8G8B8A8_UNorm:
                    return PixelFormat.RGBA8UNorm;

                case Format.R8G8B8A8_UNorm_SRgb:
                    return PixelFormat.RGBA8UNormSrgb;

                case Format.R8G8B8A8_SNorm:
                    return PixelFormat.RGBA8SNorm;

                case Format.B8G8R8A8_UNorm:
                    return PixelFormat.BGRA8UNorm;

                case Format.B8G8R8A8_UNorm_SRgb:
                    return PixelFormat.BGRA8UNormSrgb;

                // Depth
                case Format.D16_UNorm:
                    return PixelFormat.Depth16UNorm;

                case Format.D24_UNorm_S8_UInt:
                    return PixelFormat.Depth24UNormStencil8;

                case Format.D32_Float:
                    return PixelFormat.Depth32Float;

                case Format.D32_Float_S8X24_UInt:
                    return PixelFormat.Depth32FloatStencil8;

                case Format.BC1_UNorm:
                    return PixelFormat.BC1UNorm;

                case Format.BC1_UNorm_SRgb:
                    return PixelFormat.BC1UNormSrgb;

                case Format.BC2_UNorm:
                    return PixelFormat.BC2UNorm;

                case Format.BC2_UNorm_SRgb:
                    return PixelFormat.BC2UNormSrgb;

                case Format.BC3_UNorm:
                    return PixelFormat.BC3UNorm;

                case Format.BC3_UNorm_SRgb:
                    return PixelFormat.BC3UNormSrgb;

                case Format.BC4_UNorm:
                    return PixelFormat.BC4UNorm;

                case Format.BC4_SNorm:
                    return PixelFormat.BC4SNorm;

                case Format.BC5_UNorm:
                    return PixelFormat.BC5UNorm;

                case Format.BC5_SNorm:
                    return PixelFormat.BC5SNorm;

                case Format.BC6H_Sf16:
                    return PixelFormat.BC6HS16;

                case Format.BC6H_Uf16:
                    return PixelFormat.BC6HU16;

                case Format.BC7_UNorm:
                    return PixelFormat.BC7UNorm;

                case Format.BC7_UNorm_SRgb:
                    return PixelFormat.BC7UNormSrgb;

                default:
                    return PixelFormat.Unknown;
            }
        }
    }
}
