// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Alimer;
using Microsoft.Extensions.DependencyInjection;

namespace DrawTriangle
{
	public static class Program
	{
        private class TestGameContext : GameContext
        {
            public override void ConfigureServices(IServiceCollection services)
            {
                base.ConfigureServices(services);
            }
        }

		public static void Main()
		{
            using (var game = new DrawTriangleGame(new TestGameContext()))
			{
                game.Run();
			}
		}
	}
}
