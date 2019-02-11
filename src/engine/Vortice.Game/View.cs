// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Vortice.Graphics;

namespace Vortice
{
    /// <summary>
    /// Defines an <see cref="Game"/> view.
    /// </summary>
    public abstract class View
    {
        protected string _title;
        private GraphicsDevice _device;
        private SwapChain _swapChain;

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

        public SwapChainHandle Handle { get; private set; }
        public SwapChain SwapChain => _swapChain;

        public RenderPassDescriptor CurrentRenderPassDescriptor
        {
            get
            {
                return new RenderPassDescriptor(new[]{
                    new RenderPassColorAttachmentDescriptor()
                });
            }
        }

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

        internal void SetDevice(GraphicsDevice device)
        {
            _device = device;
            if (Handle != null)
            {
                OnHandleCreated(Handle);
            }
        }

        protected void OnHandleCreated(SwapChainHandle handle)
        {
            Handle = handle;
            if (_device == null)
                return;

            var clientSize = ClientSize;

            var descriptor = new SwapChainDescriptor
            {
                Width = (int)clientSize.Width,
                Height = (int)clientSize.Height,
                PreferredColorFormat = PixelFormat.BGRA8UNorm,
                PreferredDepthStencilFormat = PixelFormat.Depth24UNormStencil8,
                Handle = handle
            };
            _swapChain = _device.CreateSwapChain(descriptor);
        }

        /// <summary>
        /// Present view content on screen.
        /// </summary>
        public void Present()
        {
            _swapChain?.Present();
        }

        protected abstract void Destroy();
        protected abstract void SetTitle(string newTitle);
    }
}
