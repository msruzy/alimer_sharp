// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a graphics swap chain able to show content on window/view.
    /// </summary>
    public class SwapChain : DisposableBase
    {
        internal GPUSwapChain _backend;

        /// <summary>
        /// Gets the creation <see cref="GraphicsDevice"/>.
        /// </summary>
        public GraphicsDevice Device { get; protected set; }

        public int BackBufferCount { get; private set; }

        private Texture[] _backbufferTextures;
        private Texture2D _depthStencilTexture;
        private Framebuffer[] _framebuffers;

        /// <summary>
        /// Create a new instance of <see cref="SwapChain"/> class.
        /// </summary>
        /// <param name="device">The creation <see cref="GraphicsDevice"/></param>
        public SwapChain(GraphicsDevice device)
        {
            Guard.NotNull(device, nameof(device), $"{nameof(GraphicsDevice)} cannot be null");

            Device = device;
        }

        /// <summary>
        /// Create a new instance of <see cref="SwapChain"/> class.
        /// </summary>
        protected SwapChain()
        {
        }

        /// <summary>
        /// Configures with given <see cref="SwapChainDescriptor"/>.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        public void Configure(in SwapChainDescriptor descriptor)
        {
            if (_backend == null)
            {
                _backend = Device.CreateSwapChain(descriptor);
            }
            else
            {
                _backend.Configure(descriptor);
            }

            BackBufferCount = _backend.BackBufferCount;
            _backbufferTextures = new Texture[BackBufferCount];
            _framebuffers = new Framebuffer[BackBufferCount];

            bool hasDepthStencil = descriptor.PreferredDepthStencilFormat != PixelFormat.Unknown;
            if (hasDepthStencil)
            {
                _depthStencilTexture = new Texture2D(Device,
                    descriptor.Width,
                    descriptor.Height,
                    false,
                    1,
                    descriptor.PreferredDepthStencilFormat,
                    TextureUsage.RenderTarget);
            }

            FramebufferAttachment? depthStencilAttachment = null;
            for (var i = 0; i < BackBufferCount; i++)
            {
                _backbufferTextures[i] = new Texture(Device, _backend.GetBackBufferTexture(i));
                if (hasDepthStencil)
                {
                    depthStencilAttachment = new FramebufferAttachment(_depthStencilTexture);
                }
                _framebuffers[i] = new Framebuffer(Device, depthStencilAttachment, new FramebufferAttachment(_backbufferTextures[i]));
            }
        }

        public Framebuffer GetCurrentFramebuffer()
        {
            return _framebuffers[_backend.CurrentBackBuffer];
        }

        public void Present()
        {
            _backend.Present();
        }
    }
}
