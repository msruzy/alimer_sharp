// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a pipeline state to be bound graphics or compute in <see cref="CommandBuffer"/>.
    /// </summary>
    public abstract class PipelineState : GraphicsResource
    {
        public bool IsCompute { get; }

        /// <summary>
        /// Create a new instance of <see cref="PipelineState"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        /// <param name="descriptor">The pipeline descriptor (see <see cref="RenderPipelineDescriptor"/>).</param>
        protected PipelineState(GraphicsDevice device, in RenderPipelineDescriptor descriptor)
            : base(device, GraphicsResourceType.PipelineState, GraphicsResourceUsage.Default)
        {
            IsCompute = false;
        }
    }
}
