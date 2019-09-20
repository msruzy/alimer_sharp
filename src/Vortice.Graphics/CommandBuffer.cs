// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
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
        /// Gets whether command buffer is inside render pass.
        /// </summary>
        public bool IsInRenderPass { get; private set; }

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
        /// Set the <see cref="PipelineState"/> to use as graphics or compute pipeline.
        /// </summary>
        /// <param name="pipelineState"></param>
        public void SetPipelineState(PipelineState pipelineState)
        {
            Guard.NotNull(pipelineState, nameof(pipelineState));

            SetPipelineStateImpl(pipelineState);
        }

        public void BeginRenderPass(in RenderPassDescriptor descriptor)
        {
            IsInRenderPass = true;
            BeginRenderPassImpl(descriptor);
        }

        public void EndRenderPass()
        {
            EndRenderPassImpl();
            IsInRenderPass = false;
        }

        public void SetVertexBuffer(GraphicsBuffer buffer, int offset, int index)
        {
            Guard.NotNull(buffer, nameof(buffer));
            Guard.IsTrue((buffer.Usage & BufferUsage.Vertex) != BufferUsage.None, nameof(buffer), $"Buffer must have {BufferUsage.Vertex}");

            SetVertexBufferImpl(buffer, offset, index);
        }

        public void SetViewport(in Viewport viewport)
        {
            SetViewportCore(viewport);
        }

        protected abstract void SetViewportCore(in Viewport viewport);

        public void SetScissorRect(in Rectangle scissorRect)
        {
            SetScissorRectCore(scissorRect);
        }

        protected abstract void SetScissorRectCore(in Rectangle scissorRect);

        /// <summary>
        /// Specifies the constant blend color.
        /// </summary>
        /// <param name="blendColor">The color and alpha value for blend constant color.</param>
        public void SetBlendColor(Color4 blendColor)
        {
            SetBlendColor(ref blendColor);
        }

        /// <summary>
        /// Specifies the constant blend color.
        /// </summary>
        /// <param name="blendColor">The color and alpha value for blend constant color.</param>
        public abstract void SetBlendColor(ref Color4 blendColor);

        public abstract void SetStencilReference(int reference);

        public void SetConstantBuffer(ShaderStages stages, int index, GraphicsBuffer buffer)
        {
            SetConstantBufferImpl(stages, index, buffer);
        }

        public void Draw(int vertexCount, int instanceCount, int firstVertex, int firstInstance)
        {
            DrawImpl(vertexCount, instanceCount, firstVertex, firstInstance);
        }

        public void Dispatch(int groupCountX, int groupCountY, int groupCountZ)
        {
            DispatchCore(groupCountX, groupCountY, groupCountZ);
        }

        public void Commit()
        {
            if (IsInRenderPass)
            {
                throw new GraphicsException($"Cannot commit command buffer while inside render pass, call EndRenderPass first");
            }

            CommandQueue.Submit(this);
        }

        protected abstract void Destroy();

        protected abstract void SetPipelineStateImpl(PipelineState pipelineState);
        protected abstract void BeginRenderPassImpl(in RenderPassDescriptor descriptor);
        protected abstract void EndRenderPassImpl();
        protected abstract void SetVertexBufferImpl(GraphicsBuffer buffer, int offset, int index);
        protected abstract void DrawImpl(int vertexCount, int instanceCount, int firstVertex, int firstInstance);
        protected abstract void DispatchCore(int groupCountX, int groupCountY, int groupCountZ);

        protected abstract void SetConstantBufferImpl(ShaderStages stages, int index, GraphicsBuffer buffer);
    }
}
