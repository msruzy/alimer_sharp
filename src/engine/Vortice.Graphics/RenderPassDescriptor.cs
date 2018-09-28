// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// A collection of attachments describing the render pass.
    /// </summary>
    public readonly struct RenderPassDescriptor
    {
        /// <summary>
        /// Gets the array of color attachments.
        /// </summary>
        public RenderPassColorAttachmentDescriptor[] ColorAttachments { get; }

        /// <summary>
        /// Gets or sets the depth-stencil attachment.
        /// </summary>
        public RenderPassDepthStencilAttachmentDescriptor? DepthStencilAttachment { get; }

        /// <summary>
        /// The width, in pixels, to constain the render target to.
        /// </summary>
        public int RenderTargetWidth { get; }

        /// <summary>
        /// The height, in pixels, to constain the render target to.
        /// </summary>
        public int RenderTargetHeight { get; }

        /// <summary>
        /// The number of active layers that all attachments must have for layered rendering.
        /// </summary>
        public int RenderTargetArrayLayers { get; }

        public RenderPassDescriptor(
            in RenderPassColorAttachmentDescriptor[] colorAttachments,
            in RenderPassDepthStencilAttachmentDescriptor? depthStencilAttachment = null,
            int renderTargetWidth = 0,
            int renderTargetHeight = 0,
            int renderTargetArrayLayers = 1)
        {
            ColorAttachments = colorAttachments;
            DepthStencilAttachment = depthStencilAttachment;
            RenderTargetWidth = renderTargetWidth;
            RenderTargetHeight = renderTargetHeight;
            RenderTargetArrayLayers = renderTargetArrayLayers;
        }
    }
}
