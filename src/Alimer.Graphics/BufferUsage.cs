// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Alimer.Graphics
{
    /// <summary>
    /// Defines the usage of <see cref="GraphicsBuffer"/>.
    /// </summary>
    [Flags]
    public enum BufferUsage
    {
        /// <summary>
        /// None buffer usage.
        /// </summary>
        None = 0,

        Vertex = 1 << 0,
        Index = 1 << 1,

        /// <summary>
        /// Constant buffer usage.
        /// </summary>
        Constant = 1 << 2,
        Storage = 1 << 3,
        Indirect = 1 << 4,
    }
}
