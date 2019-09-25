// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
{
    /// <summary>
    /// Describes a <see cref="GraphicsBuffer"/>.
    /// </summary>
    public struct BufferDescriptor
    {
        /// <summary>
        /// The size in bytes of the buffer.
        /// </summary>
        public uint SizeInBytes;

        /// <summary>
        /// The <see cref="BufferUsage"/>  of the buffer.
        /// </summary>
        public BufferUsage Usage;

        /// <summary>
        /// The <see cref="GraphicsResourceUsage"/>  of the buffer.
        /// </summary>
        public GraphicsResourceUsage ResourceUsage;

        /// <summary>
        /// Create new instance of <see cref="BufferDescriptor"/> struct.
        /// </summary>
        /// <param name="sizeInBytes">Size in bytes of the buffer.</param>
        /// <param name="usage">The buffer usage.</param>
        /// <param name="resourceUsage">The buffer resource usage.</param>
        public BufferDescriptor(
            uint sizeInBytes,
            BufferUsage usage,
            GraphicsResourceUsage resourceUsage = GraphicsResourceUsage.Default)
        {
            SizeInBytes = sizeInBytes;
            Usage = usage;
            ResourceUsage = resourceUsage;
        }
    }
}
