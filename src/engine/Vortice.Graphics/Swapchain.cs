// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a graphics swap chain able to show content on window/view.
    /// </summary>
    public abstract class Swapchain : GraphicsResource
    {
        private Texture[] _backbufferTextures;
        private Framebuffer[] _framebuffers;

        /// <summary>
        /// Create a new instance of <see cref="Swapchain"/> class.
        /// </summary>
        /// <param name="device">The creation <see cref="GraphicsDevice"/></param>
        protected Swapchain(GraphicsDevice device)
            : base(device, GraphicsResourceType.Swapchain, GraphicsResourceUsage.Default)
        {
        }

        protected void Initialize(int count)
        {
            _backbufferTextures = new Texture[count];
            _framebuffers = new Framebuffer[count];
            for(var i = 0; i < count; i++)
            {
                _backbufferTextures[i] = GetBackbufferTexture(i);
                _framebuffers[i] = new Framebuffer(Device, new FramebufferAttachment(_backbufferTextures[i]));
            }
        }

        public Framebuffer GetCurrentFramebuffer() => _framebuffers[GetBackbufferIndex()];

        protected abstract Texture GetBackbufferTexture(int index);
        protected abstract int GetBackbufferIndex();
    }
}
