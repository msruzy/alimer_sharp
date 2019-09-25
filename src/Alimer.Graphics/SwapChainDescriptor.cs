// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Alimer.Graphics
{
    /// <summary>
    /// Describes <see cref="SwapChain"/>.
    /// </summary>
    public struct SwapChainDescriptor : IEquatable<SwapChainDescriptor>
    {
        /// <summary>
        /// Gets or Sets a value the width of the SwapChain back buffer.
        /// </summary>
        public int Width;

        /// <summary>
        /// Gets or Sets a value the height of the SwapChain back buffer.
        /// </summary>
        public int Height;

        /// <summary>
        /// Gets or Sets the preferred color format.
        /// </summary>
        public PixelFormat PreferredColorFormat;

        /// <summary>
        /// Gets or Sets the preferred color format.
        /// </summary>
        public PixelFormat PreferredDepthStencilFormat;

        /// <summary>
        /// Gets or Sets the <see cref="SwapChainHandle"/>.
        /// </summary>
        public SwapChainHandle Handle;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is SwapChainDescriptor other && this.Equals(other);
        }

        /// <inheritdoc />
        public bool Equals(SwapChainDescriptor other)
        {
            return Width == other.Width
                && Height == other.Height
                && PreferredColorFormat == other.PreferredColorFormat
                && PreferredDepthStencilFormat == other.PreferredDepthStencilFormat;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                hashCode = (hashCode * 397) ^ PreferredColorFormat.GetHashCode();
                hashCode = (hashCode * 397) ^ PreferredDepthStencilFormat.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(in SwapChainDescriptor left, in SwapChainDescriptor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(in SwapChainDescriptor left, in SwapChainDescriptor right)
        {
            return !(left == right);
        }
    }
}
