// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    public readonly struct VertexAttributeDescriptor : IEquatable<VertexAttributeDescriptor>
    {
        /// <summary>
        /// The shader location
        /// </summary>
        public readonly int Location;

        /// <summary>
        /// The <see cref="VertexFormat"/>.
        /// </summary>
        public readonly VertexFormat Format;

        /// <summary>
        /// The offset
        /// </summary>
        public readonly int Offset;

        public VertexAttributeDescriptor(int location, VertexFormat format, int offset)
        {
            Location = location;
            Format = format;
            Offset = offset;
        }

        public static bool operator ==(VertexAttributeDescriptor left, VertexAttributeDescriptor right) => left.Equals(right);
        public static bool operator !=(VertexAttributeDescriptor left, VertexAttributeDescriptor right) => !left.Equals(right);

        /// <inheritdoc />
        public bool Equals(VertexAttributeDescriptor other)
        {
            return Location == other.Location
                && Format == other.Format
                && Offset == other.Offset;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is VertexAttributeDescriptor other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Location.GetHashCode();
                hashCode = (hashCode * 397) ^ Format.GetHashCode();
                hashCode = (hashCode * 397) ^ Offset.GetHashCode();
                return hashCode;
            }
        }
    }
}
