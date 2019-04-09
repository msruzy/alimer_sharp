// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// An encoder that writes GPU commands into a command buffer.
    /// </summary>
    public abstract class CommandEncoder
    {
        protected readonly CommandBuffer _commandBuffer;

        protected CommandEncoder(CommandBuffer commandBuffer)
        {
            _commandBuffer = commandBuffer;
        }

        public void EndPass()
        {
            EndPassImpl();
            _commandBuffer.IsEncodingPass = false;
        }

        protected abstract void EndPassImpl();
    }
}
