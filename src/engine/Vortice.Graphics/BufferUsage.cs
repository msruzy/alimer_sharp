// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines the usage of <see cref="GraphicsBuffer"/>.
    /// </summary>
    [Flags]
    public enum BufferUsage
    {
        /// <summary>
        /// Unknown buffer usage.
        /// </summary>
        Unknown = 0,

        Vertex = 1 << 0,
        Index = 1 << 1,
        Uniform = 1 << 2,
        Storage = 1 << 3,
        Indirect = 1 << 4,
    }
}
