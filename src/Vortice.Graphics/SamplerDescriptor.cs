// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Describes <see cref="Sampler"/>.
    /// </summary>
    public struct SamplerDescriptor : IEquatable<SamplerDescriptor>
    {
        public SamplerAddressMode AddressModeU;
        public SamplerAddressMode AddressModeV;
        public SamplerAddressMode AddressModeW;
        public SamplerMinMagFilter MagFilter;
        public SamplerMinMagFilter MinFilter;
        public SamplerMipFilter MipmapFilter;
        public int MaxAnisotropy;
        public float LodMinClamp;
        public float LodMaxClamp;
        public CompareFunction CompareFunction;
        public SamplerBorderColor BorderColor;

        /// <summary>
        /// Default SamplerStateDescription values.
        /// </summary>
        public void SetDefault()
        {
            AddressModeU = SamplerAddressMode.ClampToEdge;
            AddressModeV = SamplerAddressMode.ClampToEdge;
            AddressModeW = SamplerAddressMode.ClampToEdge;
            MagFilter = SamplerMinMagFilter.Nearest;
            MinFilter = SamplerMinMagFilter.Nearest;
            MipmapFilter = SamplerMipFilter.Nearest;
            MaxAnisotropy = 1;
            LodMinClamp = 0.0f;
            LodMaxClamp = float.MaxValue;
            CompareFunction = CompareFunction.Never;
            BorderColor = SamplerBorderColor.TransparentBlack;
        }

        /// <summary>
        /// Compares two <see cref="SamplerDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="SamplerDescriptor"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="SamplerDescriptor"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(in SamplerDescriptor left, in SamplerDescriptor right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="SamplerDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="SamplerDescriptor"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="SamplerDescriptor"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(in SamplerDescriptor left, in SamplerDescriptor right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public bool Equals(ref SamplerDescriptor other)
        {
            return AddressModeU == other.AddressModeU
                && AddressModeV == other.AddressModeV
                && AddressModeW == other.AddressModeW
                && MagFilter == other.MagFilter
                && MinFilter == other.MinFilter
                && MipmapFilter == other.MipmapFilter
                && MaxAnisotropy == other.MaxAnisotropy
                && LodMinClamp == other.LodMinClamp
                && LodMaxClamp == other.LodMaxClamp
                && CompareFunction == other.CompareFunction
                && BorderColor == other.BorderColor;
        }

        /// <inheritdoc />
        public bool Equals(SamplerDescriptor other) => Equals(ref other);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SamplerDescriptor other && Equals(ref other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = AddressModeU.GetHashCode();
                hashCode = (hashCode * 397) ^ AddressModeV.GetHashCode();
                hashCode = (hashCode * 397) ^ AddressModeW.GetHashCode();
                hashCode = (hashCode * 397) ^ MagFilter.GetHashCode();
                hashCode = (hashCode * 397) ^ MinFilter.GetHashCode();
                hashCode = (hashCode * 397) ^ MipmapFilter.GetHashCode();
                hashCode = (hashCode * 397) ^ MaxAnisotropy.GetHashCode();
                hashCode = (hashCode * 397) ^ LodMinClamp.GetHashCode();
                hashCode = (hashCode * 397) ^ LodMaxClamp.GetHashCode();
                hashCode = (hashCode * 397) ^ CompareFunction.GetHashCode();
                hashCode = (hashCode * 397) ^ BorderColor.GetHashCode();
                return hashCode;
            }
        }
    }
}
