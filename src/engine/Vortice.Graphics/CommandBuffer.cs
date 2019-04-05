// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Vortice.Mathematics;

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
        public GraphicsDevice Device => CommandQueue.Device;

        /// <summary>
        /// Gets the creation <see cref="CommandQueue"/>.
        /// </summary>
        public CommandQueue CommandQueue { get; }

        /// <summary>
        /// Create a new instance of <see cref="CommandBuffer"/> class.
        /// </summary>
        /// <param name="commandQueue">The creation <see cref="CommandQueue"/></param>
        protected CommandBuffer(CommandQueue commandQueue)
        {
            CommandQueue = commandQueue;
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
        /// <param name="descriptor">The <see cref="RenderPassDescriptor"/></param>
        public void BeginRenderPass(in RenderPassDescriptor descriptor)
        {
            BeginRenderPassCore(descriptor);
        }

        public void EndRenderPass()
        {
            EndRenderPassCore();
        }

        public void SetViewport(Viewport viewport)
        {
            SetViewportImpl(viewport);
        }

        public void SetViewports(params Viewport[] viewports)
        {
            SetViewportsImpl(viewports, viewports.Length);
        }

        public void SetViewports(Viewport[] viewports, int count)
        {
            SetViewportsImpl(viewports, count);
        }

        public void SetScissorRect(RectI rect)
        {
            SetScissorRectImpl(rect);
        }

        public void SetScissorRects(RectI[] scissorRects, int count)
        {
            SetScissorRectsImpl(scissorRects, count);
        }

        public void SetScissorRects(params RectI[] scissorRects)
        {
            SetScissorRectsImpl(scissorRects, scissorRects.Length);
        }

        public void Commit()
        {
            CommandQueue.Submit(this);
        }

        protected abstract void Destroy();
        internal abstract void BeginRenderPassCore(in RenderPassDescriptor descriptor);
        protected abstract void EndRenderPassCore();
        protected abstract void SetViewportImpl(Viewport viewport);
        protected abstract void SetViewportsImpl(Viewport[] viewports, int count);
        protected abstract void SetScissorRectImpl(RectI scissorRect);
        protected abstract void SetScissorRectsImpl(RectI[] scissorRects, int count);
    }
}
