// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a framebuffer class.
    /// </summary>
    public sealed class Framebuffer : GraphicsResource
    {
        internal readonly GPUFramebuffer Backend;

        public readonly FramebufferAttachment? DepthStencilAttachment;
        public readonly IReadOnlyList<FramebufferAttachment> ColorAttachments;

        /// <summary>
        /// Create a new instance of <see cref="Framebuffer"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        /// <param name="depthStencilAttachment">Optional depth stencil attachment.</param>
        /// <param name="colorAttachments">The color attachments.</param>
        public Framebuffer(GraphicsDevice device, FramebufferAttachment? depthStencilAttachment, params FramebufferAttachment[] colorAttachments)
            : base(device, GraphicsResourceType.Framebuffer, GraphicsResourceUsage.Default)
        {
            DepthStencilAttachment = depthStencilAttachment;
            ColorAttachments = Array.AsReadOnly(colorAttachments);
            Backend = device.CreateFramebuffer(colorAttachments, depthStencilAttachment);
        }

        protected override void Destroy()
        {
            Backend.Destroy();
        }
    }
}
