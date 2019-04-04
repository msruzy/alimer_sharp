// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Vortice.Mathematics;

namespace Vortice.Graphics
{
    /// <summary>
    /// A color render target that specifies the color configuration and color operations associated with a render pipeline.
    /// </summary>
    public struct RenderPipelineColorAttachmentDescriptor : IEquatable<RenderPipelineColorAttachmentDescriptor>
    {
        /// <summary>
        /// The pixel format of the color attachment’s texture.
        /// </summary>
        public PixelFormat PixelFormat;

        /// <summary>
        /// Compares two <see cref="RenderPipelineColorAttachmentDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="RenderPipelineColorAttachmentDescriptor"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="RenderPipelineColorAttachmentDescriptor"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(RenderPipelineColorAttachmentDescriptor left, RenderPipelineColorAttachmentDescriptor right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="RenderPipelineColorAttachmentDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="RenderPipelineColorAttachmentDescriptor"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="RenderPipelineColorAttachmentDescriptor"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(RenderPipelineColorAttachmentDescriptor left, RenderPipelineColorAttachmentDescriptor right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public bool Equals(RenderPipelineColorAttachmentDescriptor other)
        {
            return PixelFormat.Equals(other.PixelFormat);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is RenderPipelineColorAttachmentDescriptor other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = PixelFormat.GetHashCode();
                return hashCode;
            }
        }
    }
}
