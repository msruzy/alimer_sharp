// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Vortice.Mathematics;

namespace Vortice.Graphics
{
    /// <summary>
    /// A color render target attachment.
    /// </summary>
    public struct RenderPassColorAttachmentDescriptor : IEquatable<RenderPassColorAttachmentDescriptor>
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
        /// The color load action.
        /// </summary>
        public LoadAction LoadAction;

        /// <summary>
        /// The color store action.
        /// </summary>
        public StoreAction StoreAction;

        /// <summary>
        /// Gets or sets the clear <see cref="Color4"/>.
        /// </summary>
        public Color4 ClearColor;

        /// <summary>
        /// Compares two <see cref="RenderPassColorAttachmentDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="RenderPassColorAttachmentDescriptor"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="RenderPassColorAttachmentDescriptor"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(RenderPassColorAttachmentDescriptor left, RenderPassColorAttachmentDescriptor right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="RenderPassColorAttachmentDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="RenderPassColorAttachmentDescriptor"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="RenderPassColorAttachmentDescriptor"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(RenderPassColorAttachmentDescriptor left, RenderPassColorAttachmentDescriptor right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public bool Equals(RenderPassColorAttachmentDescriptor other)
        {
            return Texture == other.Texture
                && Level == other.Level
                && Slice == other.Slice
                && LoadAction == other.LoadAction
                && StoreAction == other.StoreAction
                && ClearColor.Equals(other.ClearColor);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is RenderPassColorAttachmentDescriptor other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Texture?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ Level.GetHashCode();
                hashCode = (hashCode * 397) ^ Slice.GetHashCode();
                hashCode = (hashCode * 397) ^ LoadAction.GetHashCode();
                hashCode = (hashCode * 397) ^ StoreAction.GetHashCode();
                hashCode = (hashCode * 397) ^ ClearColor.GetHashCode();
                return hashCode;
            }
        }
    }
}
