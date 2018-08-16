// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Vortice.Graphics;

namespace Vortice
{
    /// <summary>
    /// Cross platform application class that handles main loop and <see cref="View"/> logic.
    /// </summary>
    public class Application : IDisposable
    {
        private readonly ApplicationHost _host;
        private bool _isExiting;
        private bool _endRunRequired;
        private bool _firstUpdateDone;

        private GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Gets the current <see cref="Application"/>.
        /// </summary>
        public static Application Current { get; private set; }

        /// <summary>
        /// Gets the main <see cref="View"/>.
        /// </summary>
        public View MainView => _host.MainView;

        /// <summary>
        /// Gets a value indicating whether the applicationis is active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the applicationis running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Occurs when the <see cref="Application"/> is activated (gains focus).
        /// </summary>
        public event TypedEventHandler<Application> Activated;

        /// <summary>
        /// Occurs when the <see cref="Application"/> is deactivated (loses focus).
        /// </summary>
        public event TypedEventHandler<Application> Deactivated;

        public Application()
        {
            if (Current != null)
            {
                throw new InvalidOperationException("Cannot create another Application.");
            }

            Current = this;

            // Create the handle.
            _host = ApplicationHost.Create(this);
            _host.Activated += Host_Activated;
            _host.Deactivated += Host_Deactivated;
            _host.Idle += Host_Idle;

            // Set default to active.
            IsActive = true;
        }

        public void Dispose()
        {
            _host.Activated -= Host_Activated;
            _host.Deactivated -= Host_Deactivated;
            _host.Idle -= Host_Idle;
        }

        public void Tick()
        {
            DrawFrame();
        }

        public void Run()
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("Cannot run this instance while it is already running");
            }

            if (!_host.IsAsyncLoop)
            {
                InitializeBeforeRun();
            }

            try
            {
                _host.Run();

                if (!_host.IsAsyncLoop)
                {
                    // If the previous call was blocking, then we can call Endrun
                    EndRun();
                }
                else
                {
                    // EndRun will be executed on Exit
                    _endRunRequired = true;
                }
            }
            finally
            {
                if (!_endRunRequired)
                {
                    IsRunning = false;
                }
            }
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        public void Exit()
        {
            _isExiting = true;
            _host.Exit();
            if (IsRunning
                && _endRunRequired)
            {
                EndRun();
                IsRunning = false;
            }
        }

        /// <summary>
        /// Raises the <see cref="Activated"/> event. 
        /// Override this method to add code to handle when the application gains focus.
        /// </summary>
        /// <param name="sender">The application.</param>
        protected virtual void OnActivated(Application sender)
        {
            Activated?.Invoke(sender);
        }

        /// <summary>
        /// Raises the <see cref="Deactivated"/> event. 
        /// Override this method to add code to handle when the application loses focus.
        /// </summary>
        /// <param name="sender">The application.</param>
        protected virtual void OnDeactivated(Application sender)
        {
            Deactivated?.Invoke(sender);
        }

        /// <summary>Called after the <see cref="Application"/> and <see cref="GraphicsDevice"/> are created, but before <see cref="LoadContent"/>.</summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected virtual void LoadContent()
        {
        }

        /// <summary>
        /// Called after all components are initialized but before the first update in the main loop.
        /// </summary>
        protected virtual void BeginRun()
        {
        }

        /// <summary>
        /// Called after the main loop has stopped running before exiting.
        /// </summary>
        protected virtual void EndRun()
        {
        }

        private void InitializeBeforeRun()
        {
            var clientSize = MainView.ClientSize;
            var swapchainDesc = new PresentationParameters
            {
                BackBufferWidth = (int)clientSize.Width,
                BackBufferHeight = (int)clientSize.Height,
                DeviceWindowHandle = MainView.NativeHandle
            };
            _graphicsDevice = new GraphicsDevice(swapchainDesc);

            // Initialize this instance and all systems.
            Initialize();

            // Load the content of the game
            LoadContent();

            IsRunning = true;
            BeginRun();

            //timer.Reset();
            //gameTime.Update(totalGameTime, TimeSpan.Zero, false);
            //gameTime.FrameCount = 0;

            // Run the first time an update
            //Update(gameTime);

            _firstUpdateDone = true;
        }

        private void DrawFrame()
        {
            _graphicsDevice.Present();
        }

        #region Host Events
        private void Host_Activated(object sender, EventArgs e)
        {
            if (!IsActive)
            {
                IsActive = true;
                OnActivated(this);
            }
        }

        private void Host_Deactivated(object sender, EventArgs e)
        {
            if (IsActive)
            {
                IsActive = false;
                OnDeactivated(this);
            }
        }

        private void Host_Idle(object sender, EventArgs e)
        {
            Tick();
        }
        #endregion
    }
}
