// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
{
    /// <summary>
    /// Defines a graphics sampler class.
    /// </summary>
    public abstract class Sampler : GraphicsResource
    {
        /// <summary>
        /// Create a new instance of <see cref="Sampler"/> class.
        /// </summary>
        /// <param name="device">The creation device.</param>
        /// <param name="descriptor">The sampler descriptor.</param>
        protected Sampler(GraphicsDevice device, in SamplerDescriptor descriptor)
            : base(device, GraphicsResourceType.Sampler, GraphicsResourceUsage.Default)
        {
        }
    }
}
