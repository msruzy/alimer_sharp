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
        /// Specifies the tessellation control (hull) stage.
        /// </summary>
        TessellationControl = 1 << 1,

        /// <summary>
        /// Specifies the tessellation evaluation (domain) stage.
        /// </summary>
        TessellationEvaluation = 1 << 2,

        /// <summary>
        /// Specifies the geometry stage.
        /// </summary>
        Geometry = 1 << 3,

        /// <summary>
        /// Specifies the fragment stage.
        /// </summary>
        Fragment = 1 << 4,

        /// <summary>
        /// Specifies the compute stage.
        /// </summary>
        Compute = 1 << 5
    }
}
