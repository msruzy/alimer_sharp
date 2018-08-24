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
        RG8UNorm,
        RGBA8UNorm,
        BGRA8UNorm,

        Depth16UNorm,
        Depth32Float,
        Depth24UNormStencil8,
        Depth32FloatStencil8,

        BC1,
        BC1_sRGB,
        BC2,
        BC2_sRGB,
        BC3,
        BC3_sRGB,
        BC4UNorm,
        BC4SNorm,
        BC5UNorm,
        BC5SNorm,

        /// <summary>
        /// Compressed format with four floating-point components.
        /// </summary>
        BC6HSFloat,

        /// <summary>
        /// Compressed format with four unsigned floating-point components.
        /// </summary>
        BC6HUFloat,
    }
}
