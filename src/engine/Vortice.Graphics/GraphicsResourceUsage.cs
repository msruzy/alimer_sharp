// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Describes the usage of <see cref="GraphicsResource"/>.
    /// </summary>
    public enum GraphicsResourceUsage
    {
        /// <summary>
        /// A resource that requires read and write access by the GPU. 
        /// </summary>
        Default,

        /// <summary>
        /// A resource that can only be read by the GPU. It cannot be written by the GPU, and cannot be accessed at all by the CPU.
        /// </summary>
        Immutable,

        /// <summary>
        /// A resource that is accessible by both the GPU (read only) and the CPU (write only).
        /// </summary>
        Dynamic,

        /// <summary>
        /// A resource that supports data transfer (copy) from the GPU to the CPU.
        /// </summary>
        Staging
    }
}
