// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a queue that organizes the order in which command buffers are executed by the GPU
    /// </summary>
    public abstract class CommandQueue
    {
        /// <summary>
        /// Gets the creation <see cref="GraphicsDevice"/>.
        /// </summary>
        public GraphicsDevice Device { get; }

        /// <summary>
        /// Create a new instance of <see cref="CommandQueue"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        protected CommandQueue(GraphicsDevice device)
        {
            Device = device;
        }

        public abstract CommandBuffer CreateCommandBuffer();
    }
}
