// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Alimer.Graphics
{
    /// <summary>
    /// Defines a graphics buffer class.
    /// </summary>
    public abstract class GraphicsBuffer : GraphicsResource
    {
        /// <summary>
        /// Gets the size in bytes of the buffer.
        /// </summary>
        public uint SizeInBytes { get; }

        /// <summary>
        /// Gets the <see cref="BufferUsage"/>.
        /// </summary>
        public BufferUsage Usage { get; }

        /// <summary>
        /// Create a new instance of <see cref="GraphicsBuffer"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        /// <param name="descriptor">The descriptor</param>
        protected GraphicsBuffer(GraphicsDevice device, in BufferDescriptor descriptor)
            : base(device, GraphicsResourceType.Buffer, descriptor.ResourceUsage)
        {
            SizeInBytes = descriptor.SizeInBytes;
            Usage = descriptor.Usage;
        }
    }
}
