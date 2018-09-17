// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vortice
{
    /// <summary>
	/// Defines a 2D integer rectangle.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    //[DataContract(Name = nameof(Rectangle))]
    public readonly struct Rectangle : IEquatable<Rectangle>
	{
        /// <summary>
        /// The total size, in bytes, of an <see cref="Rectangle"/> value.
        /// </summary>
        public static readonly int SizeInBytes = 16;

        /// <summary>
        /// Returns a <see cref="Rectangle"/> with all of its values set to zero.
        /// </summary>
        public static readonly Rectangle Empty = new Rectangle();

        /// <summary>
        /// Specifies the x-coordinate of the rectangle.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Specifies the y-coordinate of the rectangle.
        /// </summary>
        public readonly int Y;

        /// <summary>
        /// Specifies the width of the rectangle.
        /// </summary>
        public readonly int Width;

        /// <summary>
        /// Specifies the height of the rectangle.
        /// </summary>
        public readonly int Height;

        /// <summary>
		/// Gets the x-coordinate of the left side of the rectangle.
		/// </summary>
		public int Left => X;

        /// <summary>
        /// Gets the y-coordinate of the top of the rectangle.
        /// </summary>
        public int Top => Y;

        /// <summary>
        /// Gets the x-coordinate of the right side of the rectangle.
        /// </summary>
        public int Right => unchecked(X + Width);

        /// <summary>
        /// Gets the y-coordinate of the bottom of the rectangle.
        /// </summary>
        public int Bottom => unchecked(Y + Height);

        /// <summary>
		/// Gets a value that indicates whether the <see cref="Rectangle"/> is empty.
		/// </summary>
		public bool IsEmpty => Equals(Empty);

        /// <summary>
		/// Gets or sets the upper-left value of the <see cref="Rectangle"/>.
		/// </summary>
		public Point Location
        {
            get => new Point(X, Y);
        }

        /// <summary>
		/// Gets or sets the size of this <see cref="Rectangle"/>.
		/// </summary>
		public Size Size
        {
            get => new Size(Width, Height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        public Rectangle(int width, int height)
        {
            X = 0;
            Y = 0;
            Width = width;
            Height = height;
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="Rectangle"/> struct.
		/// </summary>
		/// <param name="x">The x-coordinate of the rectangle.</param>
		/// <param name="y">The y-coordinate of the rectangle.</param>
		/// <param name="width">Width of the rectangle.</param>
		/// <param name="height">Height of the rectangle.</param>
		public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Compares two <see cref="Rectangle"/> objects for equality.
        /// </summary>
        /// <param name="left">The Rectangle on the left hand of the operand.</param>
        /// <param name="right">The Rectangle on the right hand of the operand.</param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Rectangle left, Rectangle right) => left.Equals(ref right);

        /// <summary>
        /// Compares two <see cref="Rectangle"/> objects for inequality.
        /// </summary>
        /// <param name="left">The Rectangle on the left hand of the operand.</param>
        /// <param name="right">The Rectangle on the right hand of the operand.</param>
        /// <returns>
        /// True if the current left is unequal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Rectangle left, Rectangle right) => !left.Equals(ref right);

		/// <inheritdoc/>
		public override int GetHashCode() => HashHelpers.Combine(
            X.GetHashCode(), Y.GetHashCode(),
            Width.GetHashCode(), Height.GetHashCode());

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"Rectangle [ X={X}, Y={Y}, Width={Width}, Height={Height} ]";
		}

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is Rectangle rect && Equals(ref rect);

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Rectangle other) => Equals(ref other);

        /// <summary>
		/// Determines whether the specified <see cref="Rectangle"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="Rectangle"/> to compare with this instance.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref Rectangle other)
        {
            return X == other.X
                 && Y == other.Y
                 && Width == other.Width
                 && Height == other.Height;
        }
    }
}
