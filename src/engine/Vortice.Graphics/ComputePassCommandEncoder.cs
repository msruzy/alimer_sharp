// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// An encoder that writes compute commands into a command buffer.
    /// </summary>
    public abstract class ComputePassCommandEncoder : CommandEncoder
    {
        /// <summary>
        /// Create a new instance of <see cref="ComputePassCommandEncoder"/> class.
        /// </summary>
        /// <param name="commandBuffer">The creation <see cref="CommandBuffer"/>.</param>
        protected ComputePassCommandEncoder(CommandBuffer commandBuffer)
            : base(commandBuffer)
        {
        }

        public void Dispatch(int groupCountX, int groupCountY, int groupCountZ)
        {
            DispatchCore(groupCountX, groupCountY, groupCountZ);
        }

        protected abstract void DispatchCore(int groupCountX, int groupCountY, int groupCountZ);
    }
}
