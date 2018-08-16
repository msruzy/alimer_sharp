// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace DrawTriangle.UWP
{
    public static class Program
    {
        [MTAThread]
        public static void Main()
        {
            // Bootstrap game with platform.
            using (var game = new DrawTriangleGame())
            {
                game.Run();
            }
        }
    }
}
