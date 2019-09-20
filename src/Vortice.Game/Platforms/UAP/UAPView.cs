﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Drawing;
using Vortice.Graphics;
using Vortice.Mathematics;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace Vortice
{
    internal class UAPView : View, IFrameworkViewSource, IFrameworkView
    {
        private CoreWindow _coreWindow;
        private bool _activated;
        private RectangleF _bounds;
        //private SwapChainHandle _swapChainHandle;

        public CoreApplicationView CoreApplicationView { get; internal set; }

        /// <inheritdoc/>
        public override RectangleF Bounds => _bounds;

        public UAPView(string title)
            : base(title)
        {
        }

        protected override void Destroy()
        {
        }

        protected override void SetTitle(string newTitle)
        {
            var applicationView = ApplicationView.GetForCurrentView();
            if (applicationView != null)
            {
                applicationView.Title = newTitle;
            }
        }

        IFrameworkView IFrameworkViewSource.CreateView()
        {
            return this;
        }

        void IFrameworkView.Initialize(CoreApplicationView applicationView)
        {
            CoreApplicationView = applicationView;
            applicationView.Activated += ApplicationView_Activated;
        }

        void IFrameworkView.SetWindow(CoreWindow window)
        {
            _coreWindow = window;
            UpdateSize(window);

            _coreWindow.SizeChanged += CoreWindow_SizeChanged;

            // Set handle.
            OnHandleCreated(SwapChainHandle.CreateUWPCoreWindow(_coreWindow));
        }

        void IFrameworkView.Load(string entryPoint)
        {
        }

        void IFrameworkView.Run()
        {
            var applicationView = ApplicationView.GetForCurrentView();
            applicationView.Title = _title;
            _coreWindow.Activate();

            /*while (!_platform.ShouldExit)
            {
                _coreWindow.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);

                // Tick
                if (_activated)
                {
                    _platform.Idle();
                }
            }*/
        }

        void IFrameworkView.Uninitialize()
        {
            // _platform.PostRun();
        }

        private void UpdateSize(CoreWindow window)
        {
            var uwpRect = window.Bounds;
            _bounds = new RectangleF((float)uwpRect.X, (float)uwpRect.Y, (float)uwpRect.Width, (float)uwpRect.Height);
        }

        private void ApplicationView_Activated(CoreApplicationView sender, IActivatedEventArgs args)
        {
            CoreWindow.GetForCurrentThread().Activate();

            if (!_activated)
            {
                _activated = true;
            }
        }

        private void CoreWindow_SizeChanged(CoreWindow sender, WindowSizeChangedEventArgs args)
        {
            UpdateSize(sender);
        }
    }
}
