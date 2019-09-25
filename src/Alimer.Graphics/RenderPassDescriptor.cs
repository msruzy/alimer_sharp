// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Alimer.Graphics
{
    /// <summary>
    /// A collection of attachments describing the render pass.
    /// </summary>
    public readonly struct RenderPassDescriptor : IEquatable<RenderPassDescriptor>
    {
        /// <summary>
        /// Gets the array of color attachments.
        /// </summary>
        public RenderPassColorAttachmentDescriptor[] ColorAttachments { get; }

        /// <summary>
        /// Gets the depth-stencil attachment.
        /// </summary>
        public RenderPassDepthStencilAttachmentDescriptor? DepthStencilAttachment { get; }

        public RenderPassDescriptor(
            in RenderPassColorAttachmentDescriptor[] colorAttachments,
            in RenderPassDepthStencilAttachmentDescriptor? depthStencilAttachment = null)
        {
            ColorAttachments = colorAttachments;
            DepthStencilAttachment = depthStencilAttachment;
        }

        /// <summary>
        /// Compares two <see cref="RenderPassDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="RenderPassDescriptor"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="RenderPassDescriptor"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(in RenderPassDescriptor left, in RenderPassDescriptor right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="RenderPassDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="RenderPassDescriptor"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="RenderPassDescriptor"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(in RenderPassDescriptor left, in RenderPassDescriptor right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public bool Equals(RenderPassDescriptor other)
        {
            return Utilities.NullableEquals(DepthStencilAttachment, other.DepthStencilAttachment)
                && Utilities.ArrayEqualsEquatable(ColorAttachments, other.ColorAttachments);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is RenderPassDescriptor other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(DepthStencilAttachment.GetHashCode(), HashCode.Combine(ColorAttachments));
        }
    }
}
