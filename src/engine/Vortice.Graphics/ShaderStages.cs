// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
	/// Bitmask specifying a pipeline stage.
	/// </summary>
    [Flags]
    public enum ShaderStages
    {
        /// <summary>
        /// None shader stage.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies the vertex stage.
        /// </summary>
        Vertex = 1 << 0,

        /// <summary>
        /// Specifies the hull (tessellation control) stage.
        /// </summary>
        Hull = 1 << 1,

        /// <summary>
        /// Specifies the domain (tessellation evaluation) stage.
        /// </summary>
        Domain = 1 << 2,

        /// <summary>
        /// Specifies the geometry stage.
        /// </summary>
        Geometry = 1 << 3,

        /// <summary>
        /// Specifies the pixel/fragment stage.
        /// </summary>
        Pixel = 1 << 4,

        /// <summary>
        /// Specifies the compute stage.
        /// </summary>
        Compute = 1 << 5
    }
}
