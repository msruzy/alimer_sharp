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

        public bool IsEncodingPass { get; internal set; }

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
                Destroy();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Begin rendering with given descriptor.
        /// </summary>
        /// <param name="descriptor">The <see cref="RenderPassDescriptor"/></param>
        /// <returns>Instance of <see cref="RenderPassCommandEncoder"/> for encoding commands.</returns>
        public RenderPassCommandEncoder BeginRenderPass(in RenderPassDescriptor descriptor)
        {
            if (IsEncodingPass)
            {
                throw new GraphicsException($"Cannot {nameof(BeginRenderPass)} while inside another encoder pass");
            }

            return BeginRenderPassCore(descriptor);
        }

        public ComputePassCommandEncoder BeginComputePass()
        {
            if (IsEncodingPass)
            {
                throw new GraphicsException($"Cannot {nameof(BeginComputePass)} while inside another encoder pass");
            }

            return BeginComputePassCore();
        }

        public void Commit()
        {
            if (IsEncodingPass)
            {
                throw new GraphicsException($"Cannot commit command buffer while encoder are recording");
            }

            CommandQueue.Submit(this);
        }

        protected abstract void Destroy();
        protected abstract RenderPassCommandEncoder BeginRenderPassCore(in RenderPassDescriptor descriptor);
        protected abstract ComputePassCommandEncoder BeginComputePassCore();
    }
}
