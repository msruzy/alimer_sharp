// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Alimer.Graphics;
using Microsoft.Extensions.DependencyInjection;

namespace Alimer
{
    public abstract class Game : IDisposable
    {
        private static Game s_current;

        /// <summary>
        /// Gets the current instance of the <see cref="Game"/> class.
        /// </summary>
        /// <value>
        /// The current instance of the <see cref="Game"/> class.
        /// </value>
        public static Game Current
        {
            get => s_current;
            internal set => s_current = value;
        }

        public GameContext Context { get; }
        public IServiceProvider Services { get; }

        /// <summary>
        /// Gets value whether the game is running.
        /// </summary>
        public bool IsRunning { get; private set; }


        public GraphicsDevice? GraphicsDevice { get; set; }

        /// <summary>
        /// Create a new instance of <see cref="Game"/> class.
        /// </summary>
        /// <param name="context">The <see cref="GameContext"/> instance.</param>
        protected Game(GameContext context)
        {
            Guard.NotNull(context, nameof(context));

            Context = context;

            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            Services = services.BuildServiceProvider();
            GraphicsDevice = Services.GetService<GraphicsDevice>();

            // Set as current application.
            Current = this;
        }

        public virtual void Dispose()
        {

        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            Context.ConfigureServices(services);

            services.AddSingleton(this);
            //services.AddSingleton<IContentManager, ContentManager>();
        }

        public void Run()
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("This game is already running.");
            }

            IsRunning = true;
        }
    }
}
