// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a graphics pipeline class.
    /// </summary>
    public abstract class Pipeline : GraphicsResource
    {
        /// <summary>
        /// Create a new instance of <see cref="Pipeline"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        protected Pipeline(GraphicsDevice device)
            : base(device, GraphicsResourceType.Pipeline, GraphicsResourceUsage.Default)
        {
        }
    }
}
