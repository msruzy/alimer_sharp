// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Describes a <see cref="GraphicsBuffer"/>.
    /// </summary>
    public struct BufferDescriptor
    {
        /// <summary>
        /// The size in bytes of the buffer.
        /// </summary>
        public int SizeInBytes;

        /// <summary>
        /// The <see cref="BufferUsage"/>  of the buffer.
        /// </summary>
        public BufferUsage BufferUsage;

        /// <summary>
        /// The <see cref="GraphicsResourceUsage"/>  of the buffer.
        /// </summary>
        public GraphicsResourceUsage Usage;

        /// <summary>
        /// Create new instance of <see cref="BufferDescriptor"/> struct.
        /// </summary>
        /// <param name="sizeInBytes">Size in bytes of the buffer.</param>
        /// <param name="bufferUsage">The buffer usage.</param>
        /// <param name="usage">The buffer resource usage.</param>
        public BufferDescriptor(
            int sizeInBytes,
            BufferUsage bufferUsage,
            GraphicsResourceUsage usage = GraphicsResourceUsage.Default)
        {
            SizeInBytes = sizeInBytes;
            BufferUsage = bufferUsage;
            Usage = usage;
        }
    }
}
