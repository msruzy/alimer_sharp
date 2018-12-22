// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a command buffer for storing recorded graphics commands.
    /// </summary>
    public abstract class CommandBuffer : DisposableBase
    {
        /// <summary>
        /// Gets the creation <see cref="GraphicsDevice"/>.
        /// </summary>
        public GraphicsDevice Device { get; }

        /// <summary>
        /// Create a new instance of <see cref="CommandBuffer"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        protected CommandBuffer(GraphicsDevice device)
        {
            Device = device;
        }

        /// <inheritdoc/>
        protected sealed override void Dispose(bool disposing)
        {
            if (disposing
                && !IsDisposed)
            {
                //DestroyAllResources();
                Destroy();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Begin rendering with given descriptor.
        /// </summary>
        /// <param name="framebuffer">The <see cref="Framebuffer"/></param>
        /// <param name="descriptor">The <see cref="RenderPassBeginDescriptor"/></param>
        public void BeginRenderPass(Framebuffer framebuffer, in RenderPassBeginDescriptor descriptor)
        {
            Guard.NotNull(framebuffer, nameof(framebuffer));

            BeginRenderPassCore(framebuffer.Backend, descriptor);
        }

        public void EndRenderPass()
        {
            EndRenderPassCore();
        }

        public void Commit()
        {
            CommitCore();
        }

        protected abstract void Destroy();
        internal abstract void BeginRenderPassCore(GPUFramebuffer framebuffer, in RenderPassBeginDescriptor descriptor);
        protected abstract void EndRenderPassCore();
        protected abstract void CommitCore();
    }
}
