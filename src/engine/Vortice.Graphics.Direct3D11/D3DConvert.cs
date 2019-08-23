// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.
using System.Collections;
using System.Collections.Generic;
using Vortice.DirectX.Direct3D;
using Vortice.DirectX.DXGI;
using Vortice.Interop;
using Vortice.Mathematics;
namespace Vortice.Graphics
{
    internal static class D3DConvert
    {
        private static readonly FormatMap<PixelFormat, Format> _formatsMap = new FormatMap<PixelFormat, Format>
        {
            { PixelFormat.Invalid,          Format.Unknown },
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
            { PixelFormat.B5G6R5UNorm,      Format.B5G6R5_UNorm },
            { PixelFormat.BGRA4UNorm,       Format.B4G4R4A4_UNorm },
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
            { PixelFormat.RG11B10Float,     Format.R11G11B10_Float },
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
            // Compressed BC formats
            { PixelFormat.BC1RGBAUNorm,     Format.BC1_UNorm },
            { PixelFormat.BC1RGBAUNormSrgb, Format.BC1_UNorm_SRgb },
            { PixelFormat.BC2RGBAUNorm,     Format.BC2_UNorm },
            { PixelFormat.BC2RGBAUNormSrgb, Format.BC2_UNorm_SRgb },
            { PixelFormat.BC3RGBAUNorm,     Format.BC3_UNorm },
            { PixelFormat.BC3RGBAUNormSrgb, Format.BC3_UNorm_SRgb },
            { PixelFormat.BC4RUNorm,     Format.BC4_UNorm },
            { PixelFormat.BC4RSNorm,     Format.BC4_SNorm },
            { PixelFormat.BC5RGUNorm,     Format.BC5_UNorm },
            { PixelFormat.BC5RGSNorm,     Format.BC5_SNorm },
            { PixelFormat.BC6HRGBSFloat,      Format.BC6H_Sf16 },
            { PixelFormat.BC6HRGBUFloat,      Format.BC6H_Uf16 },
            { PixelFormat.BC7RGBAUNorm,     Format.BC7_UNorm },
            { PixelFormat.BC7RGBAUNormSrgb, Format.BC7_UNorm_SRgb },
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
        private static readonly FormatMap<VertexFormat, Format> _vertexFormatsMap = new FormatMap<VertexFormat, Format>
        {
            { VertexFormat.Invalid,             Format.Unknown },
            { VertexFormat.UChar2,              Format.R8G8_UInt },
            { VertexFormat.UChar4,              Format.R8G8B8A8_UInt },
            { VertexFormat.Char2,               Format.R8G8_SInt },
            { VertexFormat.Char4,               Format.R8G8B8A8_SInt },
            { VertexFormat.UChar2Norm,          Format.R8G8_UNorm },
            { VertexFormat.UChar4Norm,          Format.R8G8B8A8_UNorm },
            { VertexFormat.Char2Norm,           Format.R8G8_SNorm },
            { VertexFormat.Char4Norm,           Format.R8G8B8A8_SNorm },
            { VertexFormat.UShort2,             Format.R16G16_UInt },
            { VertexFormat.UShort4,             Format.R16G16B16A16_UInt },
            { VertexFormat.Short2,              Format.R16G16_SInt },
            { VertexFormat.Short4,              Format.R16G16B16A16_SInt },
            { VertexFormat.UShort2Norm,         Format.R16G16_UNorm },
            { VertexFormat.UShort4Norm,         Format.R16G16B16A16_UNorm },
            { VertexFormat.Short2Norm,          Format.R16G16_SNorm },
            { VertexFormat.Short4Norm,          Format.R16G16B16A16_SNorm },
            { VertexFormat.Half,                Format.R16_Float },
            { VertexFormat.Half2,               Format.R16G16_Float },
            { VertexFormat.Half4,               Format.R16G16B16A16_Float },
            { VertexFormat.Float,               Format.R32_Float },
            { VertexFormat.Float2,              Format.R32G32_Float },
            { VertexFormat.Float3,              Format.R32G32B32_Float },
            { VertexFormat.Float4,              Format.R32G32B32A32_Float },
            { VertexFormat.UInt,                Format.R32_UInt },
            { VertexFormat.UInt2,               Format.R32G32_UInt },
            { VertexFormat.UInt3,               Format.R32G32B32_UInt },
            { VertexFormat.UInt4,               Format.R32G32B32A32_UInt },
            { VertexFormat.Int,                 Format.R32_SInt },
            { VertexFormat.Int2,                Format.R32G32_SInt },
            { VertexFormat.Int3,                Format.R32G32B32_SInt },
            { VertexFormat.Int4,                Format.R32G32B32A32_SInt },
            { VertexFormat.UInt1010102Norm,     Format.R10G10B10A2_UNorm },
        };
        #region ToDirectX Methods
        public static unsafe RawColor4 ToDirectX(this Color4 color)
        {
            return *(RawColor4*)&color;
        }

        public static unsafe RawViewport ToDirectX(this Viewport viewport)
        {
            return *(RawViewport*)&viewport;
        }

        public static Format ToDirectX(this PixelFormat format) => _formatsMap[format];
        public static Format ToDirectX(this VertexFormat format) => _vertexFormatsMap[format];
        public static DirectX.Direct3D.PrimitiveTopology ToDirectX(this PrimitiveTopology topology, int patches)
        {
            switch (topology)
            {
                case PrimitiveTopology.PointList:
                    return DirectX.Direct3D.PrimitiveTopology.PointList;
                case PrimitiveTopology.LineList:
                    return DirectX.Direct3D.PrimitiveTopology.LineList;
                case PrimitiveTopology.LineStrip:
                    return DirectX.Direct3D.PrimitiveTopology.LineStrip;
                case PrimitiveTopology.TriangeList:
                    return DirectX.Direct3D.PrimitiveTopology.TriangleList;
                case PrimitiveTopology.TriangleStrip:
                    return DirectX.Direct3D.PrimitiveTopology.TriangleStrip;
                case PrimitiveTopology.PatchList:
                    return (DirectX.Direct3D.PrimitiveTopology.PatchListWith1ControlPoints) + patches - 1;
                default:
                    return DirectX.Direct3D.PrimitiveTopology.Undefined;
            }
        }
        #endregion ToDirectX Methods
        #region FromDirectX Methods
        public static PixelFormat FromDirectXPixelFormat(this Format format) => _formatsMap[format];
        public static VertexFormat FromDirectXVertexFormat(this Format format) => _vertexFormatsMap[format];
        #endregion FromDirectX Methods
        public static string GetFeatureLevelToVersion(this FeatureLevel featureLevel)
        {
            switch (featureLevel)
            {
                case FeatureLevel.Level_12_1:
                    return "12.1";
                case FeatureLevel.Level_12_0:
                    return "12.0";
                case FeatureLevel.Level_11_1:
                    return "11.1";
                case FeatureLevel.Level_11_0:
                    return "11.0";
                case FeatureLevel.Level_10_1:
                    return "10.1";
                case FeatureLevel.Level_10_0:
                    return "10.0";
                case FeatureLevel.Level_9_3:
                    return "9.3";
                case FeatureLevel.Level_9_2:
                    return "9.2";
                case FeatureLevel.Level_9_1:
                    return "9.1";
            }
            return "";
        }
        private class FormatMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
        {
            private readonly Dictionary<TKey, TValue> _forward = new Dictionary<TKey, TValue>();
            private readonly Dictionary<TValue, TKey> _reverse = new Dictionary<TValue, TKey>();
            public void Add(TKey key, TValue value)
            {
                _forward.Add(key, value);
                if (!_reverse.ContainsKey(value))
                {
                    _reverse.Add(value, key);
                }
            }
            public TValue this[TKey key] => _forward[key];
            public TKey this[TValue value] => _reverse[value];
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return _forward.GetEnumerator();
            }
        }
    }
}
