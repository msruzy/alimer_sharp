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

        public void SetScissorRect(Rect rect)
        {
            SetScissorRectImpl(rect);
        }

        public void SetScissorRects(Rect[] scissorRects, int count)
        {
            SetScissorRectsImpl(scissorRects, count);
        }

        public void SetScissorRects(params Rect[] scissorRects)
        {
            SetScissorRectsImpl(scissorRects, scissorRects.Length);
        }

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

        protected abstract void SetViewportImpl(Viewport viewport);
        protected abstract void SetViewportsImpl(Viewport[] viewports, int count);
        protected abstract void SetScissorRectImpl(Rect scissorRect);
        protected abstract void SetScissorRectsImpl(Rect[] scissorRects, int count);
    }
}
