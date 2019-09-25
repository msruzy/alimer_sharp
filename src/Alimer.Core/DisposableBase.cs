// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Vortice
{
    /// <summary>
    /// Base class for objects that implement <see cref="IDisposable"/>.
    /// Provides utility functions to easily disposable of child objects.
    /// </summary>
    public abstract class DisposableBase : IDisposable
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        /// <summary>
        /// Occurs when this instance is disposed.
        /// </summary>
        public event EventHandler<EventArgs> Disposed;

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DisposableBase"/> is reclaimed by garbage collection.
        /// </summary>
        ~DisposableBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                for (var i = _disposables.Count - 1; i >= 0; i--)
                {
                    _disposables[i].Dispose();
                }
                Disposed?.Invoke(this, EventArgs.Empty);
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Add an instance of <see cref="IDisposable"/> to dispose when this instance gets disposed.
        /// </summary>
        /// <typeparam name="T">Instance of <see cref="IDisposable"/>.</typeparam>
        /// <param name="instance">The instance of <see cref="IDisposable"/> to dispose.</param>
        protected internal T AddDisposable<T>(T instance) where T : IDisposable
        {
            if (!ReferenceEquals(instance, null))
            {
                _disposables.Add(instance);
            }

            return instance;
        }

        /// <summary>
        /// Dispose a disposable object and set the reference to null. 
        /// Removes this object from the ToDispose list.
        /// </summary>
        /// <typeparam name="T">Instance of <see cref="IDisposable"/>.</typeparam>
        /// <param name="objectToDispose">Object to dispose.</param>
        protected internal void RemoveAndDispose<T>(ref T objectToDispose)
            where T : class, IDisposable
        {
            if (!ReferenceEquals(objectToDispose, null))
            {
                _disposables.Remove(objectToDispose);
                objectToDispose.Dispose();
                objectToDispose = null;
            }
        }

        /// <summary>
        /// Removes a disposable object to the list of the objects to dispose.
        /// </summary>
        /// <typeparam name="T">Instance of <see cref="IDisposable"/>.</typeparam>
        /// <param name="toDisposeArg">To dispose.</param>
        /// <returns>True if removed, false otherwise</returns>
        protected internal bool RemoveToDispose<T>(T toDisposeArg) where T : IDisposable
        {
            if (!ReferenceEquals(toDisposeArg, null))
            {
                return _disposables.Remove(toDisposeArg);
            }

            return false;
        }

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
