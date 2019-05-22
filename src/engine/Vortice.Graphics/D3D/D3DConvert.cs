// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vortice.DirectX.DXGI;

namespace Vortice.Graphics
{
    internal static class D3DConvert
    {
        private static readonly PixelFormatMap _formatsMap = new PixelFormatMap
        {
            { PixelFormat.Unknown,          Format.Unknown },
            // 8-bit pixel formats
            { PixelFormat.R8UNorm,          Format.R8_UNorm },
            { PixelFormat.R8SNorm,          Format.R8_UNorm },
            { PixelFormat.R8UInt,           Format.R8_UNorm },
            { PixelFormat.R8SInt,           Format.R8_UNorm },

            // 16-bit pixel formats
            { PixelFormat.R16UNorm,         Format.R16_UNorm },
            { PixelFormat.R16SNorm,         Format.R16_SNorm },
            { PixelFormat.R16UInt,          Format.R16_UInt },
            { PixelFormat.R16SInt,          Format.R16_SInt },
            { PixelFormat.R16Float,         Format.R16_Float },
            { PixelFormat.RG8UNorm,         Format.R8G8_UNorm },
            { PixelFormat.RG8SNorm,         Format.R8G8_SNorm },
            { PixelFormat.RG8UInt,          Format.R8G8_UInt },
            { PixelFormat.RG8SInt,          Format.R8G8_SInt },

            // Packed 16-bit pixel formats
            { PixelFormat.R5G6B5UNorm,      Format.B5G6R5_UNorm },
            { PixelFormat.RGBA4UNorm,       Format.B4G4R4A4_UNorm },

            // 32-bit pixel formats
            { PixelFormat.R32UInt,          Format.R32_UInt },
            { PixelFormat.R32SInt,          Format.R32_SInt },
            { PixelFormat.R32Float,         Format.R32_Float },
            { PixelFormat.RG16UNorm,        Format.R16G16_UNorm },
            { PixelFormat.RG16SNorm,        Format.R16G16_SNorm },
            { PixelFormat.RG16UInt,         Format.R16G16_UInt },
            { PixelFormat.RG16SInt,         Format.R16G16_SInt },
            { PixelFormat.RG16Float,        Format.R16G16_Float },
            { PixelFormat.RGBA8UNorm,       Format.R8G8B8A8_UNorm },
            { PixelFormat.RGBA8UNormSrgb,   Format.R8G8B8A8_UNorm_SRgb },
            { PixelFormat.RGBA8SNorm,       Format.R8G8B8A8_SNorm },
            { PixelFormat.RGBA8UInt,        Format.R8G8B8A8_UInt },
            { PixelFormat.RGBA8SInt,        Format.R8G8B8A8_SInt },
            { PixelFormat.BGRA8UNorm,       Format.B8G8R8A8_UNorm },
            { PixelFormat.BGRA8UNormSrgb,   Format.B8G8R8A8_UNorm_SRgb },

            // Packed 32-Bit Pixel formats
            { PixelFormat.RGB10A2UNorm,     Format.R10G10B10A2_UNorm },
            { PixelFormat.RGB10A2UInt,      Format.R10G10B10A2_UInt },
            { PixelFormat.RG11B10Float,     Format.R11G11B10_Float },
            { PixelFormat.RGB9E5Float,      Format.R9G9B9E5_SharedExp },

            // 64-Bit Pixel Formats
            { PixelFormat.RG32UInt,         Format.R32G32_UInt },
            { PixelFormat.RG32SInt,         Format.R32G32_SInt },
            { PixelFormat.RG32Float,        Format.R32G32_Float },
            { PixelFormat.RGBA16UNorm,      Format.R16G16B16A16_UNorm },
            { PixelFormat.RGBA16SNorm,      Format.R16G16B16A16_SNorm },
            { PixelFormat.RGBA16UInt,       Format.R16G16B16A16_UInt },
            { PixelFormat.RGBA16SInt,       Format.R16G16B16A16_SInt },
            { PixelFormat.RGBA16Float,      Format.R16G16B16A16_Float },

            // 128-Bit Pixel Formats
            { PixelFormat.RGBA32UInt,       Format.R32G32B32A32_UInt },
            { PixelFormat.RGBA32SInt,       Format.R32G32B32A32_SInt },
            { PixelFormat.RGBA32Float,      Format.R32G32B32A32_Float },

            // Depth-stencil
            { PixelFormat.Depth16UNorm,     Format.D16_UNorm },
            { PixelFormat.Depth32Float,     Format.D32_Float },
            { PixelFormat.Depth24UNormStencil8, Format.D24_UNorm_S8_UInt },
            { PixelFormat.Depth32FloatStencil8, Format.D32_Float_S8X24_UInt },
            { PixelFormat.Stencil8,             Format.D24_UNorm_S8_UInt },

            // Compressed BC formats
            { PixelFormat.BC1UNorm,     Format.BC1_UNorm },
            { PixelFormat.BC1UNormSrgb, Format.BC1_UNorm_SRgb },
            { PixelFormat.BC2UNorm,     Format.BC2_UNorm },
            { PixelFormat.BC2UNormSrgb, Format.BC2_UNorm_SRgb },
            { PixelFormat.BC3UNorm,     Format.BC3_UNorm },
            { PixelFormat.BC3UNormSrgb, Format.BC3_UNorm_SRgb },
            { PixelFormat.BC4UNorm,     Format.BC4_UNorm },
            { PixelFormat.BC4SNorm,     Format.BC4_SNorm },
            { PixelFormat.BC5UNorm,     Format.BC5_UNorm },
            { PixelFormat.BC5SNorm,     Format.BC5_SNorm },

            { PixelFormat.BC6HS16,      Format.BC6H_Sf16 },
            { PixelFormat.BC6HU16,      Format.BC6H_Uf16 },
            { PixelFormat.BC7UNorm,     Format.BC7_UNorm },
            { PixelFormat.BC7UNormSrgb, Format.BC7_UNorm_SRgb },

            // Compressed PVRTC Pixel Formats
            { PixelFormat.PVRTC_RGB2,   Format.Unknown },
            { PixelFormat.PVRTC_RGBA2,  Format.Unknown },
            { PixelFormat.PVRTC_RGB4,   Format.Unknown },
            { PixelFormat.PVRTC_RGBA4,  Format.Unknown },

            // Compressed ETC Pixel Formats
            { PixelFormat.ETC2_RGB8,    Format.Unknown },
            { PixelFormat.ETC2_RGB8A1,  Format.Unknown },

            // Compressed ASTC Pixel Formats
            { PixelFormat.ASTC4x4,      Format.Unknown },
            { PixelFormat.ASTC5x5,      Format.Unknown },
            { PixelFormat.ASTC6x6,      Format.Unknown },
            { PixelFormat.ASTC8x5,      Format.Unknown },
            { PixelFormat.ASTC8x6,      Format.Unknown },
            { PixelFormat.ASTC8x8,      Format.Unknown },
            { PixelFormat.ASTC10x10,    Format.Unknown },
            { PixelFormat.ASTC12x12,    Format.Unknown },
        };

        public static Format Convert(PixelFormat format)
        {
            return _formatsMap[format];
        }

        public static PixelFormat Convert(Format format)
        {
            return _formatsMap[format];
        }

        public static Vortice.DirectX.Direct3D.PrimitiveTopology Convert(PrimitiveTopology topology, int patches)
        {
            switch (topology)
            {
                case PrimitiveTopology.PointList:
                    return Vortice.DirectX.Direct3D.PrimitiveTopology.PointList;

                case PrimitiveTopology.LineList:
                    return Vortice.DirectX.Direct3D.PrimitiveTopology.LineList;

                case PrimitiveTopology.LineStrip:
                    return Vortice.DirectX.Direct3D.PrimitiveTopology.LineStrip;

                case PrimitiveTopology.TriangeList:
                    return Vortice.DirectX.Direct3D.PrimitiveTopology.TriangleList;

                case PrimitiveTopology.TriangleStrip:
                    return Vortice.DirectX.Direct3D.PrimitiveTopology.TriangleStrip;

                case PrimitiveTopology.PatchList:
                    return (Vortice.DirectX.Direct3D.PrimitiveTopology.PatchListWith1ControlPoints) + patches - 1;

                default:
                    return Vortice.DirectX.Direct3D.PrimitiveTopology.Undefined;
            }
        }

        public class PixelFormatMap : IEnumerable<KeyValuePair<PixelFormat, Format>>
        {
            private readonly Dictionary<PixelFormat, Format> _forward = new Dictionary<PixelFormat, Format>();
            private readonly Dictionary<Format, PixelFormat> _reverse = new Dictionary<Format, PixelFormat>();

            public void Add(PixelFormat pixelFormat, Format dxgiFormat)
            {
                _forward.Add(pixelFormat, dxgiFormat);
                if (!_reverse.ContainsKey(dxgiFormat))
                {
                    _reverse.Add(dxgiFormat, pixelFormat);
                }
            }

            public Format this[PixelFormat pixelFormat] => _forward[pixelFormat];
            public PixelFormat this[Format dxgiFormat] => _reverse[dxgiFormat];

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<KeyValuePair<PixelFormat, Format>> GetEnumerator()
            {
                return _forward.GetEnumerator();
            }
        }
    }
}
