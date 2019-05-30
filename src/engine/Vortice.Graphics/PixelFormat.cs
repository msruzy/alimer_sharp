// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines format of single pixel in <see cref="Texture"/>.
    /// </summary>
    public enum PixelFormat
    {
        /// <summary>
        /// Invalid pixel format.
        /// </summary>
        Invalid = 0,

        // 8-bit pixel formats

        R8UNorm,
        R8SNorm,
        R8UInt,
        R8SInt,

        // 16-bit pixel formats
        R16UNorm,
        R16SNorm,
        R16UInt,
        R16SInt,
        R16Float,
        RG8UNorm,
        RG8SNorm,
        RG8UInt,
        RG8SInt,

        // Packed 16-bit pixel formats
        R5G6B5UNorm,
        RGBA4UNorm,

        // 32-bit pixel formats
        R32UInt,
        R32SInt,
        R32Float,
        RG16UNorm,
        RG16SNorm,
        RG16UInt,
        RG16SInt,
        RG16Float,
        RGBA8UNorm,
        RGBA8UNormSrgb,
        RGBA8SNorm,
        RGBA8UInt,
        RGBA8SInt,
        BGRA8UNorm,
        BGRA8UNormSrgb,

        // Packed 32-Bit Pixel formats
        RGB10A2UNorm,
        RGB10A2UInt,
        RG11B10Float,
        RGB9E5Float,

        // 64-Bit Pixel Formats
        RG32UInt,
        RG32SInt,
        RG32Float,
        RGBA16UNorm,
        RGBA16SNorm,
        RGBA16UInt,
        RGBA16SInt,
        RGBA16Float,

        // 128-Bit Pixel Formats
        RGBA32UInt,
        RGBA32SInt,
        RGBA32Float,

        // Depth-stencil
        Depth16UNorm,
        Depth32Float,
        Depth24UNormStencil8,
        Depth32FloatStencil8,
        Stencil8,

        // Compressed BC formats
        BC1UNorm,       // DXT1
        BC1UNormSrgb,
        BC2UNorm,       // DXT3
        BC2UNormSrgb,
        BC3UNorm,       // DXT5
        BC3UNormSrgb,
        BC4UNorm,   // RGTC Unsigned Red
        BC4SNorm,   // RGTC Signed Red
        BC5UNorm,   // RGTC Unsigned RG
        BC5SNorm,   // RGTC Signed RG

        /// <summary>
        /// Compressed format with four floating-point components.
        /// </summary>
        BC6HS16,

        /// <summary>
        /// Compressed format with four unsigned floating-point components.
        /// </summary>
        BC6HU16,

        BC7UNorm,
        BC7UNormSrgb,

        // Compressed PVRTC Pixel Formats
        PVRTC_RGB2,
        PVRTC_RGBA2,
        PVRTC_RGB4,
        PVRTC_RGBA4,

        // Compressed ETC Pixel Formats
        ETC2_RGB8,
        ETC2_RGB8A1,

        // Compressed ASTC Pixel Formats
        ASTC4x4,
        ASTC5x5,
        ASTC6x6,
        ASTC8x5,
        ASTC8x6,
        ASTC8x8,
        ASTC10x10,
        ASTC12x12,
        Count
    }
}
