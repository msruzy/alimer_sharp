// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Vortice
{
	internal static class HashHelpers
	{
		public static int Combine(int value1, int value2)
		{
			unchecked
			{
				// RyuJIT optimizes this to use the ROL instruction
				// Related GitHub pull request: dotnet/coreclr#1830
				uint rol5 = ((uint)value1 << 5) | ((uint)value1 >> 27);
				return ((int)rol5 + value1) ^ value2;
			}
		}

        public static int Combine(int value1, int value2, int value3)
        {
            return Combine(Combine(value1, value2), value3);
        }

        public static int Combine(int value1, int value2, int value3, int value4)
        {
            return Combine(Combine(Combine(value1, value2), value3), value4);
        }

        public static int Combine(int value1, int value2, int value3, int value4, int value5)
        {
            return Combine(Combine(Combine(Combine(value1, value2), value3), value4), value5);
        }
    }
}
