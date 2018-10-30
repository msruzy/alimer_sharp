// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a graphics texture class.
    /// </summary>
    public abstract class Texture : GraphicsResource
    {
        private TextureView _defaultTextureView;
        private readonly List<TextureView> _textureViews = new List<TextureView>();

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

        public TextureView DefaultTextureView => _defaultTextureView ?? (_defaultTextureView = CreateDefaultTextureView());

        /// <summary>
        /// Create a new instance of <see cref="Texture"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        /// <param name="description">The texture description</param>
        protected Texture(GraphicsDevice device, in TextureDescription description)
            : base(device, GraphicsResourceType.Texture, GraphicsResourceUsage.Default)
        {
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

        protected override void Destroy()
        {
            // Destroy all views created by this texture.
            foreach (var textureView in _textureViews)
            {
                textureView.Destroy();
            }

        }

        public TextureView CreateTextureView(in TextureViewDescriptor descriptor)
        {
            if (descriptor.Format == PixelFormat.Unknown)
            {
                throw new GraphicsException("Invalid TextureView format");
            }

            var textureView = CreateTextureViewCore(descriptor);
            _textureViews.Add(textureView);
            return textureView;
        }

        private TextureView CreateDefaultTextureView()
        {
            // TODO: add more props
            var textureViewDesc = new TextureViewDescriptor
            {
                Format = Format,
            };

            return CreateTextureView(textureViewDesc);
        }

        protected abstract TextureView CreateTextureViewCore(in TextureViewDescriptor descriptor);
    }
}
