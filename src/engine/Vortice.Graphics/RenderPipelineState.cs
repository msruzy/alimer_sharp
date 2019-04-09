// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a rendering pipeline state to be bound in <see cref="RenderPassCommandEncoder"/>.
    /// </summary>
    public abstract class RenderPipelineState : GraphicsResource
    {
        /// <summary>
        /// Create a new instance of <see cref="RenderPipelineState"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        protected RenderPipelineState(GraphicsDevice device)
            : base(device, GraphicsResourceType.Pipeline, GraphicsResourceUsage.Default)
        {
        }
    }
}
