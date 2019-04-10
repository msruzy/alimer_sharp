// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.Mathematics;

namespace Vortice.Graphics
{
    /// <summary>
    /// An encoder that writes render pass and rendering commands into a command buffer.
    /// </summary>
    public abstract class RenderPassCommandEncoder : CommandEncoder
    {
        protected RenderPipelineState _pipelineState;

        /// <summary>
        /// Create a new instance of <see cref="RenderPassCommandEncoder"/> class.
        /// </summary>
        /// <param name="commandBuffer">The creation <see cref="CommandBuffer"/>.</param>
        protected RenderPassCommandEncoder(CommandBuffer commandBuffer)
            : base(commandBuffer)
        {
        }

        public void SetPipelineState(RenderPipelineState pipelineState)
        {
            Guard.NotNull(pipelineState, nameof(pipelineState));
            _pipelineState = pipelineState;

            SetPipelineStateImpl(pipelineState);
        }

        public void SetVertexBuffer(int slot, GraphicsBuffer buffer)
        {
            SetVertexBufferImpl(slot, buffer);
        }

        public void Draw(int vertexCount, int instanceCount, int firstVertex, int firstInstance)
        {
            DrawImpl(vertexCount, instanceCount, firstInstance, firstInstance);
        }

        public void SetViewport(Viewport viewport)
        {
            SetViewport(ref viewport);
        }

        public abstract void SetViewport(ref Viewport viewport);

        public void SetScissorRect(Rect scissorRect)
        {
            SetScissorRect(ref scissorRect);
        }

        public abstract void SetScissorRect(ref Rect scissorRect);

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

        //public abstract void SetStencilReference(int reference);

        protected abstract void SetPipelineStateImpl(RenderPipelineState pipelineState);
        protected abstract void SetVertexBufferImpl(int slot, GraphicsBuffer buffer);

        protected abstract void DrawImpl(int vertexCount, int instanceCount, int firstVertex, int firstInstance);
    }
}
