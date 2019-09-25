// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Alimer.Graphics;
using Microsoft.Extensions.DependencyInjection;

namespace Alimer
{
    public abstract class Game : IDisposable
    {
        private readonly object _tickLock = new object();

        private bool _isExiting;
        private readonly Stopwatch _stopwatch = new Stopwatch();
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
        }

        public GameContext Context { get; }

        public GameWindow? Window { get; private set; }

        public IServiceProvider Services { get; }

        /// <summary>
        /// Gets value whether the game is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        public GraphicsDevice? GraphicsDevice { get; set; }
        public IList<GameSystem> GameSystems { get; } = new List<GameSystem>();
        public GameTime Time { get; } = new GameTime();

        /// <summary>
        /// Create a new instance of <see cref="Game"/> class.
        /// </summary>
        /// <param name="context">The <see cref="GameContext"/> instance.</param>
        protected Game(GameContext context)
        {
            Guard.NotNull(context, nameof(context));

            Context = context;

            // Configure and build services
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();

            // Resolve GameWindow from context.
            Window = Services.GetService<GameWindow>();
            if (Window != null)
            {
                Window.Tick += (s, e) => Tick();
            }

            GraphicsDevice = Services.GetService<GraphicsDevice>();

            // Set as current application.
            s_current = this;
        }

        public virtual void Dispose()
        {
            foreach (var gameSystem in GameSystems)
            {
                gameSystem.Dispose();
            }

            Window?.Exit();
            Window = null;
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

            Initialize();
            LoadContentAsync();

            _stopwatch.Start();
            Time.Update(_stopwatch.Elapsed, TimeSpan.Zero);

            BeginRun();

            Window?.Run();
        }

        public void Tick()
        {
            try
            {
                lock (_tickLock)
                {
                    if (_isExiting)
                    {
                        CheckEndRun();
                        return;
                    }

                    var elapsedTime = _stopwatch.Elapsed - Time.Total;
                    Time.Update(_stopwatch.Elapsed, elapsedTime);

                    Update(Time);

                    BeginDraw();
                    Draw(Time);
                }
            }
            finally
            {
                EndDraw();

                CheckEndRun();
            }
        }

        protected virtual void Initialize()
        {
            foreach (var gameSystem in GameSystems)
            {
                gameSystem.Initialize();
            }
        }

        protected virtual Task LoadContentAsync()
        {
            List<Task> loadingTasks = new List<Task>(GameSystems.Count);

            foreach (var gameSystem in GameSystems)
            {
                loadingTasks.Add(gameSystem.LoadContentAsync());
            }

            return Task.WhenAll(loadingTasks);
        }

        protected virtual void Update(GameTime gameTime)
        {
            foreach (var gameSystem in GameSystems)
            {
                gameSystem.Update(gameTime);
            }
        }

        protected virtual void BeginDraw()
        {
            foreach (var gameSystem in GameSystems)
            {
                gameSystem.BeginDraw();
            }
        }

        protected virtual void Draw(GameTime gameTime)
        {
            foreach (var gameSystem in GameSystems)
            {
                gameSystem.Draw(gameTime);
            }
        }

        protected virtual void EndDraw()
        {
            foreach (var gameSystem in GameSystems)
            {
                gameSystem.EndDraw();
            }
        }

        protected virtual void BeginRun()
        {
        }

        protected virtual void EndRun()
        {
        }

        private void CheckEndRun()
        {
            if (_isExiting && IsRunning)
            {
                EndRun();
                IsRunning = false;
                _stopwatch.Stop();
            }
        }
    }
}
