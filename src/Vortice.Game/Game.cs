﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Vortice.Graphics;

namespace Vortice
{
    /// <summary>
    /// Cross platform game class that handles main loop and main <see cref="View"/> creation logic.
    /// </summary>
    public abstract class Game : IDisposable
    {
        private readonly GameHost _host;
        private bool _isExiting;
        private bool _endRunRequired;
        private bool _firstUpdateDone;
        private bool _suppressDraw;

        protected readonly GameTime _time;
        private readonly TimerTick _timer;
        private readonly TimeSpan _maximumElapsedTime;
        private TimeSpan _accumulatedElapsedTime;
        private TimeSpan _lastFrameElapsedTime;
        private TimeSpan _totalTime;

        private bool _drawRunningSlowly;
        private bool _forceElapsedTimeToZero;

        private readonly int[] _lastUpdateCount;
        private int _nextLastUpdateCountIndex;
        private readonly float _updateCountAverageSlowLimit;

        private GraphicsDeviceFactory _graphicsDeviceFactory;
        private GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Gets the current <see cref="Game"/>.
        /// </summary>
        public static Game Current { get; private set; }

        /// <summary>
        /// Gets the main <see cref="View"/>.
        /// </summary>
        public View MainView => _host.MainView;

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> created by this application.
        /// </summary>
        public GraphicsDevice GraphicsDevice => _graphicsDevice;

        /// <summary>
        /// Gets a value indicating whether the applicationis is active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the applicationis running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fixed time step.
        /// </summary>
        /// <value><c>true</c> if this instance is fixed time step; <c>false</c> otherwise.</value>
        public bool IsFixedTimeStep { get; set; }

        /// <summary>
        /// Gets or sets the target elapsed time.
        /// </summary>
        public TimeSpan TargetElapsedTime { get; set; }

        /// <summary>
        /// Gets or sets the time to sleep when the application is inactive.
        /// </summary>
        public TimeSpan InactiveSleepTime { get; set; } = TimeSpan.FromMilliseconds(20.0);

        /// <summary>
        /// Gets the collection of <see cref="GameSystem"/> registered.
        /// </summary>
        public GameSystemCollection GameSystems { get; }

        /// <summary>
        /// Occurs when the <see cref="Game"/> is activated (gains focus).
        /// </summary>
        public event TypedEventHandler<Game> Activated;

        /// <summary>
        /// Occurs when the <see cref="Game"/> is deactivated (loses focus).
        /// </summary>
        public event TypedEventHandler<Game> Deactivated;

        /// <summary>
        /// Create new instance of <see cref="Game"/> class.
        /// </summary>
        protected Game()
        {
            if (Current != null)
            {
                throw new InvalidOperationException("Cannot create another Application.");
            }

            Current = this;

            _time = new GameTime();
            _timer = new TimerTick();
            _maximumElapsedTime = TimeSpan.FromMilliseconds(500.0);
            TargetElapsedTime = TimeSpan.FromTicks(10000000 / 60); // target elapsed time is by default 60Hz
            _lastUpdateCount = new int[4];
            _nextLastUpdateCountIndex = 0;

            // Calculate the updateCountAverageSlowLimit (assuming moving average is >=3 )
            // Example for a moving average of 4:
            // updateCountAverageSlowLimit = (2 * 2 + (4 - 2)) / 4 = 1.5f
            const int BadUpdateCountTime = 2; // number of bad frame (a bad frame is a frame that has at least 2 updates)
            var maxLastCount = 2 * Math.Min(BadUpdateCountTime, _lastUpdateCount.Length);
            _updateCountAverageSlowLimit = (float)(maxLastCount + (_lastUpdateCount.Length - maxLastCount)) / _lastUpdateCount.Length;

            // Init systems
            GameSystems = new GameSystemCollection();

            // Create the handle.
            _host = GameHost.Create(this);
            _host.Activated += Host_Activated;
            _host.Deactivated += Host_Deactivated;
            _host.Idle += Host_Idle;

            // Set default to active.
            IsActive = true;
        }

        public void Dispose()
        {
            _graphicsDevice.Dispose();

            _host.Activated -= Host_Activated;
            _host.Deactivated -= Host_Deactivated;
            _host.Idle -= Host_Idle;
        }

        public void Tick()
        {
            // If this instance is existing, then don't make any further update/draw
            if (_isExiting)
            {
                return;
            }

            // If this instance is not active, sleep for an inactive sleep time
            if (!IsActive)
            {
                Thread.Sleep(InactiveSleepTime);
            }

            // Update the timer
            _timer.Tick();

            var elapsedAdjustedTime = _timer.ElapsedAdjustedTime;
            if (_forceElapsedTimeToZero)
            {
                elapsedAdjustedTime = TimeSpan.Zero;
                _forceElapsedTimeToZero = false;
            }

            if (elapsedAdjustedTime > _maximumElapsedTime)
            {
                elapsedAdjustedTime = _maximumElapsedTime;
            }

            bool suppressNextDraw = true;
            int updateCount = 1;
            var singleFrameElapsedTime = elapsedAdjustedTime;

            if (IsFixedTimeStep)
            {
                // If the rounded TargetElapsedTime is equivalent to current ElapsedAdjustedTime
                // then make ElapsedAdjustedTime = TargetElapsedTime. We take the same internal rules as XNA 
                if (Math.Abs(elapsedAdjustedTime.Ticks - TargetElapsedTime.Ticks) < (TargetElapsedTime.Ticks >> 6))
                {
                    elapsedAdjustedTime = TargetElapsedTime;
                }

                // Update the accumulated time
                _accumulatedElapsedTime += elapsedAdjustedTime;

                // Calculate the number of update to issue
                updateCount = (int)(_accumulatedElapsedTime.Ticks / TargetElapsedTime.Ticks);

                // If there is no need for update, then exit
                if (updateCount == 0)
                {
                    // check if we can sleep the thread to free CPU resources
                    var sleepTime = TargetElapsedTime - _accumulatedElapsedTime;
                    if (sleepTime > TimeSpan.Zero)
                    {
                        Thread.Sleep(sleepTime);
                    }

                    return;
                }

                // Calculate a moving average on updateCount
                _lastUpdateCount[_nextLastUpdateCountIndex] = updateCount;
                float updateCountMean = 0;
                for (int i = 0; i < _lastUpdateCount.Length; i++)
                {
                    updateCountMean += _lastUpdateCount[i];
                }

                updateCountMean /= _lastUpdateCount.Length;
                _nextLastUpdateCountIndex = (_nextLastUpdateCountIndex + 1) % _lastUpdateCount.Length;

                // Test when we are running slowly
                _drawRunningSlowly = updateCountMean > _updateCountAverageSlowLimit;

                // We are going to call Update updateCount times, so we can subtract this from accumulated elapsed game time
                _accumulatedElapsedTime = new TimeSpan(_accumulatedElapsedTime.Ticks - (updateCount * TargetElapsedTime.Ticks));
                singleFrameElapsedTime = TargetElapsedTime;
            }
            else
            {
                Array.Clear(_lastUpdateCount, 0, _lastUpdateCount.Length);
                _nextLastUpdateCountIndex = 0;
                _drawRunningSlowly = false;
            }

            // Reset the time of the next frame.
            for (_lastFrameElapsedTime = TimeSpan.Zero; updateCount > 0 && !_isExiting; updateCount--)
            {
                _time.Update(_totalTime, singleFrameElapsedTime, _drawRunningSlowly, false);

                try
                {
                    Update(_time);

                    // If there is no exception, then we can draw the frame
                    suppressNextDraw &= _suppressDraw;
                    _suppressDraw = false;
                }
                finally
                {
                    _lastFrameElapsedTime += singleFrameElapsedTime;
                    _totalTime += singleFrameElapsedTime;
                }
            }

            if (!suppressNextDraw)
            {
                DrawFrame();
            }
        }

        public void Run()
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("Cannot run this instance while it is already running");
            }

            try
            {
                // Create GPU device first.
                _graphicsDeviceFactory = CreateGraphicsFactory(validation: true);
                _graphicsDevice = _graphicsDeviceFactory.CreateDevice(PowerPreference.Default);
                MainView.SetDevice(_graphicsDevice);

                // Enter main loop.
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
        /// Resets the elapsed time counter.
        /// </summary>
        public void ResetElapsedTime()
        {
            _forceElapsedTimeToZero = true;
            _drawRunningSlowly = false;
            Array.Clear(_lastUpdateCount, 0, _lastUpdateCount.Length);
            _nextLastUpdateCountIndex = 0;
        }

        /// <summary>
        /// Raises the <see cref="Activated"/> event. 
        /// Override this method to add code to handle when the application gains focus.
        /// </summary>
        /// <param name="sender">The application.</param>
        protected virtual void OnActivated(Game sender)
        {
            Activated?.Invoke(sender);
        }

        /// <summary>
        /// Raises the <see cref="Deactivated"/> event. 
        /// Override this method to add code to handle when the application loses focus.
        /// </summary>
        /// <param name="sender">The application.</param>
        protected virtual void OnDeactivated(Game sender)
        {
            Deactivated?.Invoke(sender);
        }

        /// <summary>
        /// Called after the <see cref="Game"/> and <see cref="GraphicsDevice"/> are created, but before <see cref="LoadContent"/>.
        /// </summary>
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

        /// <summary>
        /// Called when the application has determined that logic needs to be processed. 
        /// This might include the management of the application state, the processing of user input, or the updating of simulation data. 
        /// Override this method with application-specific logic.
        /// </summary>
        /// <param name="time">Time passed since the last call to Update.</param>
        protected virtual void Update(GameTime time)
        {
        }

        /// <summary>
        /// Starts the drawing of a frame. 
        /// This method is followed by calls to <see cref="Draw"/> and <see cref="EndDraw"/>.
        /// </summary>
        /// <returns>True to continue drawing, false to not call <see cref="Draw"/> and <see cref="EndDraw"/></returns>
        protected virtual bool BeginDraw()
        {
            // TODO:
            return true;
        }

        /// <summary>
        /// Called when the application determines it is time to draw a frame. 
        /// Override this method with application-specific rendering code.
        /// </summary>
        /// <param name="time">Time passed since the last call to Draw.</param>
        protected virtual void Draw(GameTime time)
        {
        }

        /// <summary>
        /// Ends the drawing of a frame. 
        /// This method is preceded by calls to Draw and BeginDraw.
        /// </summary>
        protected virtual void EndDraw()
        {
            // Present main view content.
            MainView.Present();

            // Tick graphics device.
            _graphicsDevice.Frame();
        }

        protected abstract GraphicsDeviceFactory CreateGraphicsFactory(bool validation);

        internal void InitializeBeforeRun()
        {
            // Initialize this instance and all systems.
            Initialize();

            // Load the content of the game
            LoadContent();

            IsRunning = true;
            BeginRun();

            _timer.Reset();
            _time.Reset(_totalTime);

            // Run the first time an update
            Update(_time);

            _firstUpdateDone = true;
        }

        private void DrawFrame()
        {
            try
            {
                if (!_isExiting
                    && _firstUpdateDone
                    && !MainView.IsMinimized
                    && BeginDraw())
                {
                    _time.Update(_totalTime, _lastFrameElapsedTime, _drawRunningSlowly, true);

                    Draw(_time);

                    EndDraw();
                }
            }
            finally
            {
                _lastFrameElapsedTime = TimeSpan.Zero;
            }
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
