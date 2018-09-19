// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice
{
    public abstract partial class ApplicationHost
    {
        public readonly Game Game;

        protected ApplicationHost(Game game)
        {
            Guard.NotNull(game, nameof(game));
            Game = game;
        }

        public event EventHandler<EventArgs> Activated;
        public event EventHandler<EventArgs> Deactivated;
        public event EventHandler<EventArgs> Idle;

        public virtual bool IsAsyncLoop => false;

        public abstract View MainView { get; }

        public abstract void Run();

        public abstract void Exit();

        protected void OnActivated()
        {
            Activated?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDeactivated()
        {
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        protected void OnIdle()
        {
            Idle(this, EventArgs.Empty);
        }
    }
}
