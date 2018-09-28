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
        private RenderPassDescriptor[] _passDescriptors;

        /// <summary>
        /// The clear color to use for <see cref="CurrentRenderPassDescriptor"/>.
        /// </summary>
        public Color4 ClearColor { get; set; } = new Color4(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
        /// Create a new instance of <see cref="Swapchain"/> class.
        /// </summary>
        /// <param name="device">The creation <see cref="GraphicsDevice"/></param>
        protected Swapchain(GraphicsDevice device)
            : base(device, GraphicsResourceType.Swapchain, GraphicsResourceUsage.Default)
        {
        }

        public RenderPassDescriptor CurrentRenderPassDescriptor
        {
            get
            {
                var descriptor = _passDescriptors[GetBackbufferIndex()];
                descriptor.ColorAttachments[0].ClearColor = ClearColor;
                return descriptor;
            }
        }


        public void Present()
        {
            PresentCore();
        }

        protected void Initialize(Texture[] textures)
        {
            _passDescriptors = new RenderPassDescriptor[textures.Length];
            for (var i = 0; i < textures.Length; i++)
            {
                _passDescriptors[i] = new RenderPassDescriptor(new[]
                {
                    new RenderPassColorAttachmentDescriptor(textures[i].DefaultTextureView)
                });
            }
        }

        protected abstract int GetBackbufferIndex();
        protected abstract void PresentCore();
    }
}
