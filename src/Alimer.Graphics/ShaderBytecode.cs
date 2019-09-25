// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Alimer.Graphics
{
    /// <summary>
	/// Defines a shader bytecode.
	/// </summary>
    public readonly struct ShaderBytecode
    {
        /// <summary>
        /// The stage of the bytecode.
        /// </summary>
        public readonly ShaderStages Stage;

        /// <summary>
        /// Gets the bytecode data.
        /// </summary>
        public readonly byte[] Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderBytecode"/> struct.
        /// </summary>
        /// <param name="stage">The stage of the bytecode.</param>
        /// <param name="data">The bytecode data</param>
        public ShaderBytecode(ShaderStages stage, byte[] data)
        {
            Stage = stage;
            Data = data;
        }
    }
}
