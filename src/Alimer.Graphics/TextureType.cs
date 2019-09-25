// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
{
    /// <summary>
	/// Defines the type of a <see cref="Texture"/>.
	/// </summary>
	public enum TextureType
    {
        /// <summary>
        /// Unknown texture type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The texture is 1D.
        /// </summary>
        Texture1D,

        /// <summary>
        /// The texture is 2D.
        /// </summary>
        Texture2D,

        /// <summary>
        /// The texture is 3D.
        /// </summary>
        Texture3D,

        /// <summary>
        /// The texture is a CubeMap.
        /// </summary>
        TextureCube,
    }
}
