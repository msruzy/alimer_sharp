// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a graphics swap chain able to show content on window/view.
    /// </summary>
    public abstract class SwapChain : GraphicsResource
    {
        public abstract int BackBufferCount { get; }
        public abstract int CurrentBackBuffer { get; }

        private Texture[] _textures;
        private Texture _depthStencilTexture;

        /// <summary>
        /// Create a new instance of <see cref="SwapChain"/> class.
        /// </summary>
        /// <param name="device">The creation <see cref="GraphicsDevice"/></param>
        /// <param name="descriptor">The <see cref="SwapChainDescriptor"/></param>
        protected SwapChain(GraphicsDevice device, SwapChainDescriptor descriptor)
            : base(device, GraphicsResourceType.SwapChain, GraphicsResourceUsage.Default)
        {
            Guard.NotNull(device, nameof(device), $"{nameof(GraphicsDevice)} cannot be null");
        }

        /// <summary>
        /// Configures with given <see cref="SwapChainDescriptor"/>.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        public void Configure(in SwapChainDescriptor descriptor)
        {
            _textures = new Texture[BackBufferCount];
            for (var i = 0; i < BackBufferCount; i++)
            {
                _textures[i] = GetBackBufferTexture(i);
            }

            bool hasDepthStencil = descriptor.PreferredDepthStencilFormat != PixelFormat.Invalid;
            if (hasDepthStencil)
            {
                _depthStencilTexture = Device.CreateTexture(
                    TextureDescriptor.Texture2D(descriptor.Width, descriptor.Height,
                    false, 1,
                    descriptor.PreferredDepthStencilFormat, TextureUsage.RenderTarget)
                    );
            }
        }

        /// <summary>
        /// Get the current frame <see cref="Texture"/>.
        /// </summary>
        public Texture CurrentTexture => _textures[CurrentBackBuffer];

        /// <summary>
        /// Get the depth/stencil texture.
        /// </summary>
        public Texture DepthStencilTexture => _depthStencilTexture;

        public void Present()
        {
            PresentImpl();
        }

        protected abstract Texture GetBackBufferTexture(int index);
        protected abstract void PresentImpl();
    }
}
