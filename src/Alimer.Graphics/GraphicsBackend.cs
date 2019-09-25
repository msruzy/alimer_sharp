// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
{
    /// <summary>
    /// Defines the type of <see cref="GraphicsDevice"/> to create.
    /// </summary>
    public enum GraphicsBackend
    {
        /// <summary>
        /// Invalid backend.
        /// </summary>
        Invalid,

        /// <summary>
        /// Null backend.
        /// </summary>
        Null,

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
    }
}
