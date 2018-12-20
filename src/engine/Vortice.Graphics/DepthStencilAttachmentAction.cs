// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// A depth-stencil render target attachment.
    /// </summary>
    public struct DepthStencilAttachmentAction : IEquatable<DepthStencilAttachmentAction>
    {
        public LoadAction DepthLoadAction;
        public StoreAction DepthStoreAction;

        /// <summary>
        /// Gets the clear depth value.
        /// </summary>
        public float ClearDepth;

        public LoadAction StencilLoadAction;
        public StoreAction StencilStoreAction;

        /// <summary>
        /// Gets the clear stencil value.
        /// </summary>
        public byte ClearStencil;

        /// <summary>
        /// Compares two <see cref="DepthStencilAttachmentAction"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="DepthStencilAttachmentAction"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="DepthStencilAttachmentAction"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(DepthStencilAttachmentAction left, DepthStencilAttachmentAction right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="DepthStencilAttachmentAction"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="DepthStencilAttachmentAction"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="DepthStencilAttachmentAction"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(DepthStencilAttachmentAction left, DepthStencilAttachmentAction right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public bool Equals(DepthStencilAttachmentAction other) =>
            DepthLoadAction == other.DepthLoadAction
            && DepthStoreAction == other.DepthStoreAction
            && ClearDepth == other.ClearDepth
            && StencilLoadAction == other.StencilLoadAction
            && StencilStoreAction == other.StencilStoreAction
            && ClearStencil == other.ClearStencil;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is DepthStencilAttachmentAction other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = DepthLoadAction.GetHashCode();
                hashCode = (hashCode * 397) ^ DepthStoreAction.GetHashCode();
                hashCode = (hashCode * 397) ^ ClearDepth.GetHashCode();
                hashCode = (hashCode * 397) ^ StencilLoadAction.GetHashCode();
                hashCode = (hashCode * 397) ^ StencilStoreAction.GetHashCode();
                hashCode = (hashCode * 397) ^ ClearStencil.GetHashCode();
                return hashCode;
            }
        }
    }
}
