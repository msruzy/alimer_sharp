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

        private Texture _depthStencilTexture;
        private Framebuffer[] _framebuffers;

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
            ConfigureImpl(descriptor);

            _framebuffers = new Framebuffer[BackBufferCount];

            bool hasDepthStencil = descriptor.PreferredDepthStencilFormat != PixelFormat.Unknown;
            if (hasDepthStencil)
            {
                _depthStencilTexture = Device.CreateTexture(
                    TextureDescription.Texture2D(descriptor.Width, descriptor.Height,
                    false, 1,
                    descriptor.PreferredDepthStencilFormat, TextureUsage.RenderTarget)
                    );
            }

            FramebufferAttachment? depthStencilAttachment = null;
            for (var i = 0; i < BackBufferCount; i++)
            {
                var backbufferTexture = GetBackBufferTexture(i);
                if (hasDepthStencil)
                {
                    depthStencilAttachment = new FramebufferAttachment(_depthStencilTexture);
                }

                _framebuffers[i] = Device.CreateFramebuffer(new[] { new FramebufferAttachment(backbufferTexture) }, depthStencilAttachment);
            }
        }

        /// <summary>
        /// Get the current frame <see cref="Framebuffer"/>.
        /// </summary>
        public Framebuffer CurrentFramebuffer => _framebuffers[CurrentBackBuffer];

        public void Present()
        {
            PresentImpl();
        }

        protected abstract void ConfigureImpl(in SwapChainDescriptor descriptor);
        protected abstract Texture GetBackBufferTexture(int index);
        protected abstract void PresentImpl();
    }
}
