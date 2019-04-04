// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a queue that organizes command buffers to be executed by a GPU.
    /// </summary>
    public abstract class CommandQueue
    {
        protected readonly ConcurrentQueue<CommandBuffer> _availableCommandBuffers = new ConcurrentQueue<CommandBuffer>();

        /// <summary>
        /// Gets the creation <see cref="GraphicsDevice"/>.
        /// </summary>
        public GraphicsDevice Device { get; }

        /// <summary>
        /// Get the type of queue.
        /// </summary>
        public CommandQueueType QueueType { get; }

        /// <summary>
        /// Create a new instance of <see cref="CommandBuffer"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        /// <param name="queueType">The type of queue.</param>
        protected CommandQueue(GraphicsDevice device, CommandQueueType queueType)
        {
            Device = device;
            QueueType = queueType;
        }

        /// <summary>
        /// Get an available command buffer from the command queue.
        /// </summary>
        /// <returns></returns>
        public CommandBuffer GetCommandBuffer()
        {
            if (_availableCommandBuffers.TryDequeue(out var commandBuffer))
            {
                return commandBuffer;
            }

            return CreateCommandBuffer();
        }

        internal void Submit(CommandBuffer commandBuffer)
        {
            SubmitImpl(commandBuffer);
        }

        protected abstract CommandBuffer CreateCommandBuffer();
        protected abstract void SubmitImpl(CommandBuffer commandBuffer);
    }
}
