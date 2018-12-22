// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Utility class for <see cref="PixelFormat"/>.
    /// </summary>
    public static class PixelFormatUtil
    {
        /// <summary>
        /// Checks if given format is depth.
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/> to check</param>
        /// <returns><c>true</c> if is depth, <c>false</c> otherwise.</returns>
        public static bool IsDepthFormat(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Depth16UNorm:
                case PixelFormat.Depth24UNormStencil8:
                case PixelFormat.Depth32Float:
                case PixelFormat.Depth32FloatStencil8:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if given format is stencil.
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/> to check</param>
        /// <returns><c>true</c> if is stencil, <c>false</c> otherwise.</returns>
        public static bool IsStencilFormat(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Depth24UNormStencil8:
                case PixelFormat.Depth32FloatStencil8:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if given format is depth-stencil.
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/> to check</param>
        /// <returns><c>true</c> if is depth-stencil, <c>false</c> otherwise.</returns>
        public static bool IsDepthStencilFormat(PixelFormat format)
        {
            return IsDepthFormat(format) || IsStencilFormat(format);
        }

        /// <summary>
        /// Checks if given format is compressed.
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/> to check</param>
        /// <returns><c>true</c> if is compressed, <c>false</c> otherwise.</returns>
        public static bool IsCompressed(PixelFormat format)
        {
            return format >= PixelFormat.BC1UNorm 
                && format <= PixelFormat.BC7UNormSrgb;
        }
    }
}
