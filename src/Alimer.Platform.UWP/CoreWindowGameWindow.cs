// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace Alimer
{
    internal class CoreWindowGameWindow : GameWindow
    {
        private readonly CoreWindow _coreWindow;
        private RectangleF _clientBounds;

        /// <inheritdoc/>
        public override string Title
        {
            get => ApplicationView.GetForCurrentView()?.Title ?? string.Empty;
            set
            {
                var appView = ApplicationView.GetForCurrentView();
                if (appView != null)
                {
                    appView.Title = value;
                }
            }
        }

        /// <inheritdoc/>
        public override RectangleF ClientBounds => _clientBounds;

        public CoreWindowGameWindow(CoreWindow coreWindow)
        {
            Guard.NotNull(coreWindow, nameof(coreWindow));

            _coreWindow = coreWindow;
            _coreWindow.SizeChanged += CoreWindow_SizeChanged;
            UpdateClientBounds();
        }


        private void UpdateClientBounds()
        {
            double resolutionScale = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            _clientBounds = new RectangleF(
                (float)_coreWindow.Bounds.X,
                (float)_coreWindow.Bounds.X,
                Math.Max(1, (float)(_coreWindow.Bounds.Width * resolutionScale)),
                Math.Max(1, (float)(_coreWindow.Bounds.Height * resolutionScale)));
        }

        private void CoreWindow_SizeChanged(CoreWindow sender, WindowSizeChangedEventArgs e)
        {
            UpdateClientBounds();
            OnSizeChanged();
        }
    }
}
