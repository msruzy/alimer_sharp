// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Utilities class.
    /// </summary>
    internal static class Utilities
    {
        public static bool NullableEquals<T>(T? left, T? right) where T : struct, IEquatable<T>
        {
            if (left.HasValue && right.HasValue)
            {
                return left.Value.Equals(right.Value);
            }

            return left.HasValue == right.HasValue;
        }

        public static bool ArrayEqualsEquatable<T>(T[] left, T[] right) where T : struct, IEquatable<T>
        {
            if (left == null || right == null)
            {
                return left == right;
            }

            if (left.Length != right.Length)
            {
                return false;
            }

            for (int i = 0; i < left.Length; i++)
            {
                if (!left[i].Equals(right[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    internal static class HashCode
    {
        public static int Combine(int value1, int value2)
        {
            uint rol5 = ((uint)value1 << 5) | ((uint)value1 >> 27);
            return ((int)rol5 + value1) ^ value2;
        }

        public static int Combine<T>(T[] items)
        {
            if (items == null || items.Length == 0)
            {
                return 0;
            }

            int hash = items[0].GetHashCode();
            for (int i = 1; i < items.Length; i++)
            {
                hash = Combine(hash, items[i]?.GetHashCode() ?? i);
            }

            return hash;
        }
    }
}
