// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.Audio;

namespace DrawTriangle
{
	public static class Program
	{
		public static void Main()
		{
            var audio = new AudioEngine();
            using (var game = new DrawTriangleGame())
			{
                game.Run();
			}
		}
	}
}
