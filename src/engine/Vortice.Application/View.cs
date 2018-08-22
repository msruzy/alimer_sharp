// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace Vortice
{
    /// <summary>
    /// Defines an <see cref="Application"/> view.
    /// </summary>
    public abstract class View
    {
        protected string _title;

        /// <summary>
        /// Occurs directly after <see cref="Close"/> is called, and can be handled to cancel <see cref="View"/> closure.
        /// </summary>
        public event TypedEventHandler<View, CancelEventArgs> Closing;

        /// <summary>
        /// Occurs directly when view is closed.
        /// </summary>
        public event TypedEventHandler<View> Closed;

        /// <summary>
        /// Gets whether the view is minimized.
        /// </summary>
        public virtual bool IsMinimized => false;

        /// <summary>
        /// Gets the view client size.
        /// </summary>
        public abstract Size ClientSize { get; }

        /// <summary>
        /// The text that is displayed in the title bar of the window (if it has a title bar).
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    SetTitle(value);
                }
            }
        }

        public abstract object NativeHandle { get; }

        protected View(string title)
        {
            _title = title;
        }

        /// <summary>
        /// Request close.
        /// </summary>
        public void Close()
        {
            // Ask for cancelling close.
            var e = new CancelEventArgs();
            OnClosing(e);

            if (!e.Cancel)
            {
                Destroy();

                // Fire closed event.
                OnClosed();
            }
        }

        /// <summary>
        /// Raises <see cref="Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs"/> that contains the event data.</param>
        protected virtual void OnClosing(CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="Closed"/> event.
        /// </summary>
        protected virtual void OnClosed()
        {
            Closed?.Invoke(this);
        }

        protected abstract void Destroy();
        protected abstract void SetTitle(string newTitle);
    }
}
