// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines the type of <see cref="GraphicsDeviceFactory"/> to create.
    /// </summary>
    public enum GraphicsBackend
    {
        /// <summary>
		/// DirectX 11.1+ backend.
		/// </summary>
		DirectX11,

        /// <summary>
        /// DirectX 12 backend.
        /// </summary>
        DirectX12,
    }
}
