// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines the write mask of <see cref="RenderPipelineColorAttachmentDescriptor"/>.
    /// </summary>
    [Flags]
    public enum ColorWriteMask
    {
        /// <summary>
        /// None write mask.
        /// </summary>
        None = 0,

        /// <summary>
        /// The red color channel is enabled.
        /// </summary>
        Red = 1,

        /// <summary>
        /// The green color channel is enabled.
        /// </summary>
        Green = 2,

        /// <summary>
        /// The blue color channel is enabled.
        /// </summary>
        Blue = 4,

        /// <summary>
        /// The alpha color channel is enabled.
        /// </summary>
        Alpha = 8,

        /// <summary>
        /// All color channels are enabled.
        /// </summary>
        All = 15,
    }
}
