// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    public readonly struct RenderPassDescription
    {
        public RenderPassColorAttachment[] ColorAttachments { get; }
        public RenderPassDepthStencilAttachment? DepthStencilAttachment { get; }

        /// <summary>
        /// The width, in pixels, to constain the render target to.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The height, in pixels, to constain the render target to.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// The number of active layers that all attachments must have for layered rendering.
        /// </summary>
        public int Layers { get; }

        public RenderPassDescription(
            in RenderPassColorAttachment[] colorAttachments,
            in RenderPassDepthStencilAttachment? depthStencilAttachment = null,
            int width = 0,
            int height = 0,
            int layers = 1)
        {
            ColorAttachments = colorAttachments;
            DepthStencilAttachment = depthStencilAttachment;
            Width = width;
            Height = height;
            Layers = Math.Max(layers, 1);
        }
    }
}
