// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using Alimer.Graphics;

namespace Alimer
{
    /// <summary>
    /// Defines a <see cref="Game"/> window.
    /// </summary>
    public abstract class GameWindow : IDisposable
    {
        private GraphicsDevice? _device;
        private SwapChainHandle? _handle;

        public event EventHandler? SizeChanged;

        public bool IsExiting { get; private set; }

        /// <summary>
        /// Gets the screen dimensions of the game window's client rectangle.
        /// </summary>
        public abstract RectangleF ClientBounds { get; }

        /// <summary>
        /// Gets and sets the title of the window.
        /// </summary>
        public abstract string Title { get; set; }

        /// <summary>
        /// Gets or Sets device used to create graphics objects.
        /// </summary>
        public GraphicsDevice? Device
        {
            get => _device;
            set
            {
                _device = value;
                if (_handle != null)
                {
                    CreateSwapChain();
                }
            }
        }

        /// <summary>
        /// Gets the configured <see cref="Graphics.SwapChain"/>.
        /// </summary>
        public SwapChain? SwapChain { get; set; }

        public virtual void Dispose()
        {
        }

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

        protected void ConfigureSwapChain(SwapChainHandle handle)
        {
            SwapChain?.Dispose();

            // Store handle for later creation.
            _handle = handle;

            if (Device != null)
            {
                CreateSwapChain();
            }
        }

        private void CreateSwapChain()
        {
            SwapChain = Device?.CreateSwapChain(new SwapChainDescriptor
            {
                Width = (int)ClientBounds.Width,
                Height = (int)ClientBounds.Height,
                PreferredColorFormat = PixelFormat.BGRA8UNorm,
                PreferredDepthStencilFormat = PixelFormat.Depth32Float,
                Handle = _handle,
            });
        }
    }
}
