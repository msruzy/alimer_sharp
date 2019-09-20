// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
	public enum SamplerAddressMode
    {
        /// <summary>
        /// Texture coordinates are clamped between 0.0 and 1.0, inclusive.
        /// </summary>
        ClampToEdge,

        /// <summary>
        /// Between -1.0 and 1.0, the texture coordinates are mirrored across the axis. Outside -1.0 and 1.0, the texture coordinates are clamped.
        /// </summary>
        MirrorClampToEdge,

        /// <summary>
        /// Texture coordinates wrap to the other side of the texture, effectively keeping only the fractional part of the texture coordinate.
        /// </summary>
        Repeat,

        /// <summary>
        /// Between -1.0 and 1.0, the texture coordinates are mirrored across the axis. Outside -1.0 and 1.0, the image is repeated.
        /// </summary>
        MirrorRepeat,

        /// <summary>
        /// Out-of-range texture coordinates return the value specified by the sampler's border color.
        /// </summary>
        ClampToBorder
    }
}
