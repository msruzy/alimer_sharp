// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Drawing;

namespace Alimer
{
    /// <summary>
    /// Defines a <see cref="Game"/> window.
    /// </summary>
    public abstract class GameWindow : IDisposable
    {
        public event EventHandler SizeChanged;
        public event EventHandler Tick;

        public bool IsExiting { get; private set; }

        /// <summary>
        /// Gets the screen dimensions of the game window's client rectangle.
        /// </summary>
        public abstract RectangleF ClientBounds { get; }

        /// <summary>
        /// Gets and sets the title of the window.
        /// </summary>
        public abstract string Title { get; set; }

        public virtual void Dispose()
        {
        }

        public abstract void Run();

        public void Exit()
        {
            IsExiting = true;
            Dispose();
        }

        /// <summary>
        /// Occurs when window size changed.
        /// </summary>
        protected virtual void OnSizeChanged()
        {
            SizeChanged?.Invoke(this, EventArgs.Empty);
        }

        protected void OnTick()
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }
    }
}
