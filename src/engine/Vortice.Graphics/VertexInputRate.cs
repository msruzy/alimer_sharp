// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines the rate at which vertex attributes are pulled from buffers
    /// </summary>
    public enum VertexInputRate
    {
        /// <summary>
        /// Specifies that vertex attribute addressing is a function of the vertex index.
        /// </summary>
        Vertex,

        /// <summary>
        /// Specifies that vertex attribute addressing is a function of the instance index.
        /// </summary>
        Instance
    }
}
