// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// A bitmask indicating usage of a <see cref="Texture"/>
    /// </summary>
    [Flags]
    public enum TextureUsage
    {
        /// <summary>
        /// Unknown usage.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// An option that enables reading or sampling from the texture.
        /// </summary>
        ShaderRead = 1 << 0,

        /// <summary>
        /// An option that enables writing to the texture.
        /// </summary>
        ShaderWrite = 1 << 1,

        /// <summary>
        /// An option that enables using the texture as a color, depth, or stencil render target in a <see cref="Framebuffer"/>.
        /// </summary>
        RenderTarget = 1 << 2,
    }
}
