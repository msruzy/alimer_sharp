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
        /// Unknown pixel format.
        /// </summary>
        Unknown = 0,

        R8UNorm,
        R8SNorm,

        R16UNorm,
        R16SNorm,

        RG8UNorm,
        RG8SNorm,

        RG16UNorm,
        RG16SNorm,

        RGBA8UNorm,
        RGBA8UNormSrgb,
        RGBA8SNorm,

        BGRA8UNorm,
        BGRA8UNormSrgb,

        // Depth-stencil
        Depth16UNorm,
        Depth24UNormStencil8,
        Depth32Float,
        Depth32FloatStencil8,

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
        Count
    }
}
