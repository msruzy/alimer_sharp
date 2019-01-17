// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    internal readonly struct TextureViewDescriptor : IEquatable<TextureViewDescriptor>
    {
        public const int MaxPossible = ~0;

        public readonly int MostDetailedMip;
        public readonly int MipCount;
        public readonly int FirstArraySlice;
        public readonly int ArraySize;

        public TextureViewDescriptor(int mostDetailedMip = 0, int mipCount = MaxPossible, int firstArraySlice = 0, int arraySize = MaxPossible)
        {
            MostDetailedMip = mostDetailedMip;
            MipCount = mipCount;
            FirstArraySlice = firstArraySlice;
            ArraySize = arraySize;
        }

        public static bool operator ==(TextureViewDescriptor left, TextureViewDescriptor right) => left.Equals(right);
        public static bool operator !=(TextureViewDescriptor left, TextureViewDescriptor right) => !left.Equals(right);

        /// <inheritdoc />
        public bool Equals(TextureViewDescriptor other) =>
            MostDetailedMip == other.MostDetailedMip
            && MipCount == other.MipCount
            && FirstArraySlice == other.FirstArraySlice
            && ArraySize == other.ArraySize;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is TextureViewDescriptor other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = MostDetailedMip.GetHashCode();
                hashCode = (hashCode * 397) ^ MipCount.GetHashCode();
                hashCode = (hashCode * 397) ^ FirstArraySlice.GetHashCode();
                hashCode = (hashCode * 397) ^ ArraySize.GetHashCode();
                return hashCode;
            }
        }
    }
}
