// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a graphics Shader class.
    /// </summary>
    public abstract class Shader : GraphicsResource
    {
        public bool IsCompute { get; }

        /// <summary>
        /// Create a new instance of <see cref="GraphicsBuffer"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        /// <param name="isCompute">Is compute shader</param>
        protected Shader(GraphicsDevice device, bool isCompute)
            : base(device, GraphicsResourceType.Shader, GraphicsResourceUsage.Default)
        {
            IsCompute = isCompute;
        }
    }
}
