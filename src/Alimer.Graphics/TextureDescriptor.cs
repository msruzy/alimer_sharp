// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Alimer.Graphics
{
    /// <summary>
    /// Describes <see cref="Texture"/>.
    /// </summary>
    public readonly struct TextureDescriptor : IEquatable<TextureDescriptor>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureDescriptor"/> struct.
        /// </summary>
        /// <param name="textureType">The texture type</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="depth">The depth</param>
        /// <param name="mipLevels">The mipLevels</param>
        /// <param name="arrayLayers">The array layers</param>
        /// <param name="format">The <see cref="PixelFormat"/></param>
        /// <param name="usage">The texture usage</param>
        /// <param name="samples">The samples</param>
        public TextureDescriptor(TextureType textureType,
            int width, int height, int depth,
            int mipLevels, int arrayLayers,
            PixelFormat format,
            TextureUsage usage,
            SampleCount samples)
        {
            TextureType = textureType;
            Width = width;
            Height = height;
            Depth = depth;
            MipLevels = mipLevels;
            ArrayLayers = arrayLayers;
            Format = format;
            Usage = usage;
            Samples = samples;
        }

        /// <summary>
        /// Create new instance of <see cref="TextureDescriptor"/> struct describing 2D texture.
        /// </summary>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="mipLevels">The number of mip levels.</param>
        /// <param name="arrayLayers">The array layers count.</param>
        /// <param name="format">The <see cref="PixelFormat"/></param>
        /// <param name="textureUsage">The texture usage</param>
        /// <param name="samples">The number of samples.</param>
        /// <returns></returns>
        public static TextureDescriptor Texture2D(
            int width,
            int height,
            int mipLevels = 1,
            int arrayLayers = 1,
            PixelFormat format = PixelFormat.RGBA8UNorm,
            TextureUsage textureUsage = TextureUsage.ShaderRead,
            SampleCount samples = SampleCount.Count1)
        {
            width = Math.Max(width, 1);
            height = Math.Max(height, 1);
            mipLevels = Math.Max(mipLevels, 1);
            arrayLayers = Math.Max(arrayLayers, 1);
            return new TextureDescriptor(TextureType.Texture2D, width, height, 1, mipLevels, arrayLayers, format, textureUsage, samples);
        }

        /// <summary>
        /// Create new instance of <see cref="TextureDescriptor"/> struct describing 2D texture.
        /// </summary>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="mipMap">Whether to compute mip levels from width and height, otherwise 1.</param>
        /// <param name="arrayLayers">The array layers count.</param>
        /// <param name="format">The <see cref="PixelFormat"/></param>
        /// <param name="textureUsage">The texture usage.</param>
        /// <param name="samples">The number of samples.</param>
        /// <returns></returns>
        public static TextureDescriptor Texture2D(
            int width,
            int height,
            bool mipMap,
            int arrayLayers = 1,
            PixelFormat format = PixelFormat.RGBA8UNorm,
            TextureUsage textureUsage = TextureUsage.ShaderRead,
            SampleCount samples = SampleCount.Count1)
        {
            int mipLevels = 1;
            if (mipMap)
            {
                while (height > 1 || width > 1)
                {
                    ++mipLevels;

                    if (height > 1)
                        height >>= 1;

                    if (width > 1)
                        width >>= 1;
                }
            }

            width = Math.Max(width, 1);
            height = Math.Max(height, 1);
            arrayLayers = Math.Max(arrayLayers, 1);

            return new TextureDescriptor(TextureType.Texture2D, width, height, 1, mipLevels, arrayLayers, format, textureUsage, samples);
        }

        /// <summary>
        /// Gets the texture type.
        /// </summary>
        public TextureType TextureType { get; }

        /// <summary>
		/// Gets the total width of this instance, in texels.
		/// </summary>
		public int Width { get; }

        /// <summary>
        /// Gets the total height of this instance, in texels.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the total depth of this instance, in texels.
        /// </summary>
        public int Depth { get; }

        /// <summary>
		/// Gets the total number of mipmap levels in this instance.
		/// </summary>
		public int MipLevels { get; }

        /// <summary>
        /// Gets the total number of array layers in this instance.
        /// </summary>
        public int ArrayLayers { get; }

        /// <summary>
		/// Gets the <see cref="PixelFormat"/> of individual texture elements.
		/// </summary>
		public PixelFormat Format { get; }

        /// <summary>
        /// Gets the texture usage.
        /// </summary>
        public TextureUsage Usage { get; }

        /// <summary>
		/// Gets the number of samples.
		/// </summary>
        public SampleCount Samples { get; }

        /// <summary>
        /// Compares two <see cref="TextureDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="TextureDescriptor"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="TextureDescriptor"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(in TextureDescriptor left, in TextureDescriptor right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="TextureDescriptor"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="TextureDescriptor"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="TextureDescriptor"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator !=(in TextureDescriptor left, in TextureDescriptor right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public bool Equals(TextureDescriptor other)
        {
            return TextureType == other.TextureType
                && Width == other.Width
                && Height == other.Height
                && Depth == other.Depth
                && MipLevels == other.MipLevels
                && ArrayLayers == other.ArrayLayers
                && Format == other.Format
                && Usage == other.Usage
                && Samples == other.Samples;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is TextureDescriptor other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = TextureType.GetHashCode();
                hashCode = (hashCode * 397) ^ Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                hashCode = (hashCode * 397) ^ Depth.GetHashCode();
                hashCode = (hashCode * 397) ^ MipLevels.GetHashCode();
                hashCode = (hashCode * 397) ^ ArrayLayers.GetHashCode();
                hashCode = (hashCode * 397) ^ Format.GetHashCode();
                hashCode = (hashCode * 397) ^ Usage.GetHashCode();
                hashCode = (hashCode * 397) ^ Samples.GetHashCode();
                return hashCode;
            }
        }
    }
}
