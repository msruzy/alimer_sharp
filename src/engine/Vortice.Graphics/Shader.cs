// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

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
        /// <param name="bytecode">The shader byte.</param>
        protected Shader(GraphicsDevice device, ShaderBytecode bytecode)
            : base(device, GraphicsResourceType.Shader, GraphicsResourceUsage.Default)
        {
            Stage = bytecode.Stage;
        }
    }
}
