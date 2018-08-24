// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// A color render target attachment.
    /// </summary>
    public readonly struct RenderPassColorAttachment : IEquatable<RenderPassColorAttachment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPassColorAttachment"/> struct.
        /// </summary>
        /// <param name="texture"><see cref="Texture"/> attachment</param>
        /// <param name="clearColor">The clear color</param>
        /// <param name="loadAction">The attachment <see cref="Graphics.LoadAction"/></param>
        /// <param name="storeAction">The attachment <see cref="Graphics.StoreAction"/></param>
        /// <param name="mipLevel">The attachment texture mip level</param>
        /// <param name="slice">The attachment texture slice</param>
        public RenderPassColorAttachment(
            Texture texture,
            Color4 clearColor,
            LoadAction loadAction = LoadAction.Clear,
            StoreAction storeAction = StoreAction.Store,
            int mipLevel = 0,
            int slice = 0)
        {
            Texture = texture;
            ClearColor = clearColor;
            LoadAction = loadAction;
            StoreAction = storeAction;
            MipLevel = mipLevel;
            Slice = slice;
        }

        /// <summary>
        /// Gets the attachment <see cref="Texture"/>.
        /// </summary>
        public Texture Texture { get; }

        /// <summary>
        /// Gets the clear <see cref="Color4"/>.
        /// </summary>
        public Color4 ClearColor { get; }

        public LoadAction LoadAction { get; }
        public StoreAction StoreAction { get; }

        /// <summary>
        /// The mipmap level of the texture used for rendering to the attachment.
        /// </summary>
        public int MipLevel { get; }

        /// <summary>
        /// The slice of the texture used for rendering to the attachment.
        /// </summary>
        public int Slice { get; }

        /// <summary>
        /// Compares two <see cref="RenderPassColorAttachment"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="RenderPassColorAttachment"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="RenderPassColorAttachment"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(RenderPassColorAttachment left, RenderPassColorAttachment right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="RenderPassColorAttachment"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="RenderPassColorAttachment"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="RenderPassColorAttachment"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(RenderPassColorAttachment left, RenderPassColorAttachment right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public bool Equals(RenderPassColorAttachment other) =>
            Texture == other.Texture
            && ClearColor.Equals(other.ClearColor)
            && LoadAction == other.LoadAction
            && StoreAction == other.StoreAction
            && MipLevel == other.MipLevel
            && Slice == other.Slice;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is RenderPassColorAttachment other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Texture?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ ClearColor.GetHashCode();
                hashCode = (hashCode * 397) ^ LoadAction.GetHashCode();
                hashCode = (hashCode * 397) ^ StoreAction.GetHashCode();
                hashCode = (hashCode * 397) ^ MipLevel.GetHashCode();
                hashCode = (hashCode * 397) ^ Slice.GetHashCode();
                return hashCode;
            }
        }
    }
}
