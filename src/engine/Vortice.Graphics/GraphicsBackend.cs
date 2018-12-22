// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines the type of <see cref="GraphicsDevice"/> to create.
    /// </summary>
    public enum GraphicsBackend
    {
        /// <summary>
        /// Best supported device on running platform.
        /// </summary>
        Default,

        /// <summary>
		/// DirectX 11.1+ backend.
		/// </summary>
		Direct3D11,

        /// <summary>
        /// DirectX 12 backend.
        /// </summary>
        Direct3D12,

        /// <summary>
        /// Vulkan backend
        /// </summary>
        Vulkan,

        /// <summary>
        /// OpenGL 3.3+ backend.
        /// </summary>
        OpenGL,

        /// <summary>
        /// OpenGLES 2.0+ backend
        /// </summary>
        OpenGLES,
    }
}
