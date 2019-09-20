// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    public readonly struct VertexLayoutDescriptor : IEquatable<VertexLayoutDescriptor>
    {
        public readonly int Stride;
        public readonly VertexAttributeDescriptor[] Attributes;
        public readonly VertexInputRate InputRate;

        /// <summary>
        /// Create a new instance of <see cref="VertexLayoutDescriptor"/> struct.
        /// </summary>
        /// <param name="stride">The number of bytes in between successive elements in the <see cref="GraphicsBuffer"/>.</param>
        /// <param name="attributes">An array of <see cref="VertexAttributeDescriptor"/> objects, each describing a single element
        /// of vertex data.</param>
        public VertexLayoutDescriptor(int stride, params VertexAttributeDescriptor[] attributes)
        {
            Stride = stride;
            Attributes = attributes;
            InputRate = VertexInputRate.Vertex;
        }

        /// <summary>
        /// Create a new instance of <see cref="VertexLayoutDescriptor"/> struct.
        /// </summary>
        /// <param name="stride">The number of bytes in between successive elements in the <see cref="GraphicsBuffer"/>.</param>
        /// <param name="inputRate">The <see cref="VertexInputRate"/>.</param>
        /// <param name="attributes">An array of <see cref="VertexAttributeDescriptor"/> objects, each describing a single element
        /// of vertex data.</param>
        public VertexLayoutDescriptor(int stride, VertexInputRate inputRate, params VertexAttributeDescriptor[] attributes)
        {
            Stride = stride;
            Attributes = attributes;
            InputRate = inputRate;
        }

        /// <summary>
        /// Create a new instance of <see cref="VertexLayoutDescriptor"/> struct. The stride is assumed to be the sum of the size of all elements.
        /// </summary>
        /// <param name="attributes">An array of <see cref="VertexAttributeDescriptor"/> objects, each describing a single element
        /// of vertex data.</param>
        public VertexLayoutDescriptor(params VertexAttributeDescriptor[] attributes)
        {
            Attributes = attributes;
            var stride = 0;
            for (int i = 0; i < attributes.Length; i++)
            {
                int elementSize = VertexFormatUtil.GetSizeInBytes(attributes[i].Format);
                if (attributes[i].Offset != 0)
                {
                    stride = attributes[i].Offset + elementSize;
                }
                else
                {
                    stride += elementSize;
                }
            }

            Stride = stride;
            InputRate = VertexInputRate.Vertex;
        }

        public static bool operator ==(VertexLayoutDescriptor left, VertexLayoutDescriptor right) => left.Equals(right);
        public static bool operator !=(VertexLayoutDescriptor left, VertexLayoutDescriptor right) => !left.Equals(right);

        /// <inheritdoc />
        public bool Equals(VertexLayoutDescriptor other)
        {
            return Stride == other.Stride
                && Utilities.ArrayEqualsEquatable(Attributes, other.Attributes)
                && InputRate == other.InputRate;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is VertexLayoutDescriptor other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = HashCode.Combine(Stride.GetHashCode(), HashCode.Combine(Attributes));
                return HashCode.Combine(hashCode, InputRate.GetHashCode());
            }
        }
    }
}
