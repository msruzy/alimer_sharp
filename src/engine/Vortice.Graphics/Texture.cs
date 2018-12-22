// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a graphics texture class.
    /// </summary>
    public class Texture : GraphicsResource
    {
        internal readonly GPUTexture _backend;

        /// <summary>
        /// Gets the texture type.
        /// </summary>
        public TextureType TextureType { get; }

        /// <summary>
		/// Gets the total width of this instance, in texels.
		/// </summary>
		public int Width { get; }

        /// <summary>
        /// Gets the total height of this instance, in texels.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the total depth of this instance, in texels.
        /// </summary>
        public int Depth { get; }

        /// <summary>
		/// Gets the total number of mipmap levels in this instance.
		/// </summary>
		public int MipLevels { get; }

        /// <summary>
        /// Gets the total number of array layers in this instance.
        /// </summary>
        public int ArrayLayers { get; }

        /// <summary>
		/// Gets the <see cref="PixelFormat"/> of individual texture elements.
		/// </summary>
		public PixelFormat Format { get; }

        /// <summary>
        /// Gets the texture usage.
        /// </summary>
        public TextureUsage TextureUsage { get; }

        /// <summary>
		/// Gets the number of samples.
		/// </summary>
        public SampleCount Samples { get; }

        /// <summary>
        /// Create a new instance of <see cref="Texture"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        /// <param name="description">The texture description</param>
        protected Texture(GraphicsDevice device, in TextureDescription description)
            : base(device, GraphicsResourceType.Texture, GraphicsResourceUsage.Default)
        {
            Guard.IsTrue(description.TextureType != TextureType.Unknown, nameof(description), $"TextureType cannot be {nameof(TextureType.Unknown)}");
            Guard.MustBeGreaterThanOrEqualTo(description.Width, 1, nameof(description.Width));
            Guard.MustBeGreaterThanOrEqualTo(description.Height, 1, nameof(description.Height));
            Guard.MustBeGreaterThanOrEqualTo(description.Depth, 1, nameof(description.Depth));

            _backend = device.CreateTexture(description);
            TextureType = description.TextureType;
            Width = description.Width;
            Height = description.Height;
            Depth = description.Depth;
            MipLevels = description.MipLevels;
            ArrayLayers = description.ArrayLayers;
            Format = description.Format;
            TextureUsage = description.TextureUsage;
            Samples = description.Samples;
        }

        internal Texture(GraphicsDevice device, GPUTexture backend)
            : base(device, GraphicsResourceType.Texture, GraphicsResourceUsage.Default)
        {
            _backend = backend;
            TextureType = backend.TextureType;
            Width = backend.Width;
            Height = backend.Height;
            Depth = backend.Depth;
            MipLevels = backend.MipLevels;
            ArrayLayers = backend.ArrayLayers;
            Format = backend.Format;
            TextureUsage = backend.TextureUsage;
            Samples = backend.Samples;
        }

        protected override void Destroy()
        {
        }
    }
}
