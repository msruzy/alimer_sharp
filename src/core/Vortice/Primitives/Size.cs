// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vortice
{
    /// <summary>
    /// An immutable value type representing a size with an single precision floating points width and height.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    //[DataContract(Name = nameof(Size))]
    public readonly struct Size : IEquatable<Size>
	{
		/// <summary>
		/// The total size, in bytes, of an <see cref="Size"/> value.
		/// </summary>
		public static readonly int SizeInBytes = 8;

		/// <summary>
		/// A Size of (0, 0).
		/// </summary>
		public static readonly Size Empty = new Size();

		/// <summary>
		/// Width of the size.
		/// </summary>
		public readonly float Width;

		/// <summary>
		/// Height of the size.
		/// </summary>
		public readonly float Height;

		/// <summary>
		/// Gets the aspect ratio of the size.
		/// </summary>
		public float AspectRatio => Width / Height;

        /// <summary>
        /// Gets whether the size is empty.
        /// </summary>
        public bool IsEmpty => Equals(Empty);

		/// <summary>
		/// Create a <see cref="Size"/> with the given Width and Height.
		/// </summary>
		/// <param name="width">Width value.</param>
		/// <param name="height">Height value.</param>
		public Size(float width, float height)
		{
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Compares two <see cref="Size"/> objects for equality.
		/// </summary>
		/// <param name="left">The size on the left hand of the operand.</param>
		/// <param name="right">The size on the right hand of the operand.</param>
		/// <returns>
		/// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Size left, Size right) => left.Width.Equals(right.Width) && left.Height.Equals(right.Height);

		/// <summary>
		/// Compares two <see cref="Size"/> objects for inequality.
		/// </summary>
		/// <param name="left">The size on the left hand of the operand.</param>
		/// <param name="right">The size on the right hand of the operand.</param>
		/// <returns>
		/// True if the current left is unequal to the <paramref name="right"/> parameter; otherwise, false.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Size left, Size right) => !left.Equals(right);

		/// <inheritdoc/>
		public override int GetHashCode() => HashHelpers.Combine(Width.GetHashCode(), Height.GetHashCode());

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"Size [ Width={Width}, Height={Height} ]";
		}

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is Size size && Equals(size);

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Size other) => Width.Equals(other.Width) && Height.Equals(other.Height);
	}
}
