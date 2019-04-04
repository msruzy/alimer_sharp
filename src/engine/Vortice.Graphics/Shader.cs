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
        public ShaderStages Stage { get; }

        /// <summary>
        /// Create a new instance of <see cref="Shader"/> class.
        /// </summary>
        /// <param name="device">The creation device.</param>
        /// <param name="stage">The shader stage.</param>
        protected Shader(GraphicsDevice device, ShaderStages stage)
            : base(device, GraphicsResourceType.Shader, GraphicsResourceUsage.Default)
        {
            Stage = stage;
        }
    }
}
