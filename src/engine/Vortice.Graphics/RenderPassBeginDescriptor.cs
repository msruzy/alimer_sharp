// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// A collection of attachments describing the render pass.
    /// </summary>
    public readonly struct RenderPassBeginDescriptor
    {
        /// <summary>
        /// Gets the array of color attachments.
        /// </summary>
        public ColorAttachmentAction[] Colors { get; }

        /// <summary>
        /// Gets or sets the depth-stencil attachment.
        /// </summary>
        public DepthStencilAttachmentAction? DepthStencilAttachment { get; }

        /// <summary>
        /// The width, in pixels, to constain the render target to.
        /// </summary>
        public int RenderTargetWidth { get; }

        /// <summary>
        /// The height, in pixels, to constain the render target to.
        /// </summary>
        public int RenderTargetHeight { get; }

        public RenderPassBeginDescriptor(
            in ColorAttachmentAction[] colors,
            in DepthStencilAttachmentAction? depthStencil = null,
            int renderTargetWidth = 0,
            int renderTargetHeight = 0)
        {
            Colors = colors;
            DepthStencilAttachment = depthStencil;
            RenderTargetWidth = renderTargetWidth;
            RenderTargetHeight = renderTargetHeight;
        }
    }
}
