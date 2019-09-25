// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Alimer.Graphics
{
    public readonly struct TextureViewDescriptor : IEquatable<TextureViewDescriptor>
    {
        /// <summary>
        /// Base mip level.
        /// </summary>
        public readonly int BaseMipLevel;

        /// <summary>
        ///  If <see cref="MipLevelCount"/> == 0, the texture view will cover all the mipmap levels starting from <see cref="BaseMipLevel"/>.
        /// </summary>
        public readonly int MipLevelCount;

        /// <summary>
        /// Base view array level.
        /// </summary>
        public readonly int BaseArrayLayer;

        /// <summary>
        /// If <see cref="ArrayLayerCount"/> == 0, the texture view will cover all the array layers starting from <see cref="BaseArrayLayer"/>.
        /// </summary>
        public readonly int ArrayLayerCount;

        public TextureViewDescriptor(
            int baseMipLevel = 0,
            int mipLevelCount = 0, 
            int baseArrayLayer = 0,
            int arrayLayerCount = 0)
        {
            BaseMipLevel = baseMipLevel;
            MipLevelCount = mipLevelCount;
            BaseArrayLayer = baseArrayLayer;
            ArrayLayerCount = arrayLayerCount;
        }

        public static bool operator ==(TextureViewDescriptor left, TextureViewDescriptor right) => left.Equals(right);
        public static bool operator !=(TextureViewDescriptor left, TextureViewDescriptor right) => !left.Equals(right);

        /// <inheritdoc />
        public bool Equals(TextureViewDescriptor other) =>
            BaseMipLevel == other.BaseMipLevel
            && MipLevelCount == other.MipLevelCount
            && BaseArrayLayer == other.BaseArrayLayer
            && ArrayLayerCount == other.ArrayLayerCount;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is TextureViewDescriptor other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = BaseMipLevel.GetHashCode();
                hashCode = (hashCode * 397) ^ MipLevelCount.GetHashCode();
                hashCode = (hashCode * 397) ^ BaseArrayLayer.GetHashCode();
                hashCode = (hashCode * 397) ^ ArrayLayerCount.GetHashCode();
                return hashCode;
            }
        }
    }
}
