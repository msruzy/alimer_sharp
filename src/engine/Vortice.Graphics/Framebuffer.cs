// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a framebuffer class.
    /// </summary>
    public sealed class Framebuffer : GraphicsResource
    {
        internal readonly IFramebuffer Backend;

        /// <summary>
        /// Create a new instance of <see cref="Framebuffer"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        /// <param name="colorAttachments">The color attachments.</param>
        public Framebuffer(GraphicsDevice device, FramebufferAttachment[] colorAttachments)
            : base(device, GraphicsResourceType.Framebuffer, GraphicsResourceUsage.Default)
        {
            Backend = device.CreateFramebuffer(colorAttachments);
        }

        protected override void Destroy()
        {
            Backend.Destroy();
        }
    }

    internal interface IFramebuffer
    {
        void Destroy();
    }
}
