// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a 2D texture.
    /// </summary>
    public class Texture2D : Texture
    {
        /// <summary>
        /// Create a new instance of <see cref="Texture"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        /// <param name="width">The width in pixels of texture.</param>
        /// <param name="height">The height in pixels of texture.</param>
        /// <param name="mipMap">Whether to compute mip levels from width and height, otherwise 1.</param>
        /// <param name="arrayLayers">The array layers count.</param>
        /// <param name="format">The texture <see cref="PixelFormat"/>.</param>
        /// <param name="textureUsage">The texture usage.</param>
        /// <param name="samples">The number of samples.</param>
        public Texture2D(GraphicsDevice device, int width, int height, bool mipMap,
            int arrayLayers = 1,
            PixelFormat format = PixelFormat.RGBA8UNorm,
            TextureUsage textureUsage = TextureUsage.ShaderRead,
            SampleCount samples = SampleCount.Count1)
            : base(device, TextureDescription.Texture2D(width, height, mipMap, arrayLayers, format, textureUsage, samples))
        {
        }
    }
}
