// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a graphics buffer class.
    /// </summary>
    public abstract class GraphicsBuffer : GraphicsResource
    {
        /// <summary>
        /// Gets the <see cref="BufferUsage"/>.
        /// </summary>
        public BufferUsage Usage { get; }

        /// <summary>
        /// Create a new instance of <see cref="GraphicsBuffer"/> class.
        /// </summary>
        /// <param name="device">The creation device</param>
        /// <param name="usage">The buffer usage.</param>
        protected GraphicsBuffer(GraphicsDevice device, BufferUsage usage)
            : base(device, GraphicsResourceType.Buffer)
        {
            Guard.IsTrue(usage != BufferUsage.Unknown, nameof(usage), $"BufferUsage cannot be {nameof(BufferUsage.Unknown)}");

            Usage = usage;
        }
    }
}
