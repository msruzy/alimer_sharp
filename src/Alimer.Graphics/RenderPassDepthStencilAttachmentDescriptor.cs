// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Alimer.Graphics
{
    /// <summary>
    /// A depth-stencil render target attachment.
    /// </summary>
    public struct RenderPassDepthStencilAttachmentDescriptor : IEquatable<RenderPassDepthStencilAttachmentDescriptor>
    {
        /// <summary>
        /// The texture attachment.
        /// </summary>
        public Texture Texture;

        /// <summary>
        /// The mipmap level of the texture used for rendering to the attachment.
        /// </summary>
        public int Level;

        /// <summary>
        /// The slice of the texture used for rendering to the attachment.
        /// </summary>
        public int Slice;

        /// <summary>
        /// Depth load action.
        /// </summary>
        public LoadAction DepthLoadAction;

        /// <summary>
        /// Depth store action.
        /// </summary>
        public StoreAction DepthStoreAction;

        /// <summary>
        /// Stencil load action.
        /// </summary>
        public LoadAction StencilLoadAction;

        /// <summary>
        /// Stencil store action.
        /// </summary>
        public StoreAction StencilStoreAction;

        /// <summary>
        /// Clear depth value.
        /// </summary>
        public float ClearDepth;

        /// <summary>
        /// Clear stencil value.
        /// </summary>
        public byte ClearStencil;

        /// <summary>
        /// Compares two <see cref="RenderPassDepthStencilAttachmentDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="RenderPassDepthStencilAttachmentDescriptor"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="RenderPassDepthStencilAttachmentDescriptor"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(RenderPassDepthStencilAttachmentDescriptor left, RenderPassDepthStencilAttachmentDescriptor right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="RenderPassDepthStencilAttachmentDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="RenderPassDepthStencilAttachmentDescriptor"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="RenderPassDepthStencilAttachmentDescriptor"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(RenderPassDepthStencilAttachmentDescriptor left, RenderPassDepthStencilAttachmentDescriptor right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public bool Equals(RenderPassDepthStencilAttachmentDescriptor other)
        {
            return Texture == other.Texture
                && Level == other.Level
                && Slice == other.Slice
                && DepthLoadAction == other.DepthLoadAction
                && DepthStoreAction == other.DepthStoreAction
                && StencilLoadAction == other.StencilLoadAction
                && StencilStoreAction == other.StencilStoreAction
                && ClearDepth == other.ClearDepth
                && ClearStencil == other.ClearStencil;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is RenderPassDepthStencilAttachmentDescriptor other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Texture?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ Level.GetHashCode();
                hashCode = (hashCode * 397) ^ Slice.GetHashCode();
                hashCode = (hashCode * 397) ^ DepthLoadAction.GetHashCode();
                hashCode = (hashCode * 397) ^ DepthStoreAction.GetHashCode();
                hashCode = (hashCode * 397) ^ StencilLoadAction.GetHashCode();
                hashCode = (hashCode * 397) ^ StencilStoreAction.GetHashCode();
                hashCode = (hashCode * 397) ^ ClearDepth.GetHashCode();
                hashCode = (hashCode * 397) ^ ClearStencil.GetHashCode();
                return hashCode;
            }
        }
    }
}
