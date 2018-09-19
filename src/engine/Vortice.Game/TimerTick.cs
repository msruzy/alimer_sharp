// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Vortice
{
    /// <summary>
    /// This provides timing information similar to <see cref="System.Diagnostics.Stopwatch"/> but an update occurring only on a <see cref="Tick"/> method.
    /// </summary>
    public class TimerTick
    {
        private long _startRawTime;
        private long _lastRawTime;
        private int _pauseCount;
        private long _pauseStartTime;
        private long _timePaused;

        /// <summary>
        /// Gets the elapsed time since the previous call to <see cref="Tick"/>.
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }

        /// <summary>
        /// Gets the elapsed adjusted time since the previous call to <see cref="Tick"/> taking into account <see cref="Pause"/> time.
        /// </summary>
        public TimeSpan ElapsedAdjustedTime { get; private set; }


        /// <summary>
        /// Gets the total time elapsed since the last reset or when this timer was created.
        /// </summary>
        public TimeSpan TotalTime { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is paused.
        /// </summary>
        /// <value><c>true</c> if this instance is paused; <c>false</c> otherwise.</value>
        public bool IsPaused => _pauseCount > 0;

        /// <summary>
        /// Create a new instance of the <see cref="TimerTick"/> class.
        /// </summary>
        public TimerTick()
        {
            Reset();
        }

        /// <summary>
        /// Resets this instance. <see cref="TotalTime"/> is set to zero.
        /// </summary>
        public void Reset()
        {
            TotalTime = TimeSpan.Zero;
            _startRawTime = Stopwatch.GetTimestamp();
            _lastRawTime = _startRawTime;
            _timePaused = 0L;
            _pauseStartTime = 0L;
            _pauseCount = 0;
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            _pauseCount++;
            if (_pauseCount == 1)
            {
                _pauseStartTime = Stopwatch.GetTimestamp();
            }
        }

        /// <summary>
        /// Resumes this instance, only if a call to <see cref="Pause"/> has been already issued.
        /// </summary>
        public void Resume()
        {
            _pauseCount--;
            if (_pauseCount <= 0)
            {
                _timePaused += Stopwatch.GetTimestamp() - _pauseStartTime;
                _pauseStartTime = 0L;
            }
        }

        public void Tick()
        {
            // Don't tick when this instance is paused.
            if (IsPaused)
            {
                // Reset elapsed time if paused.
                ElapsedTime = TimeSpan.Zero;
                return;
            }

            var rawTime = Stopwatch.GetTimestamp();
            TotalTime = ConvertRawToTimestamp(rawTime - _startRawTime);
            ElapsedTime = ConvertRawToTimestamp(rawTime - _lastRawTime);
            ElapsedAdjustedTime = ConvertRawToTimestamp(rawTime - (_lastRawTime + _timePaused));

            if (ElapsedAdjustedTime < TimeSpan.Zero)
            {
                ElapsedAdjustedTime = TimeSpan.Zero;
            }

            _timePaused = 0;
            _lastRawTime = rawTime;
        }

        /// <summary>
        /// Converts a <see cref="Stopwatch" /> raw time to a <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns>The <see cref="TimeSpan" />.</returns>
        private static TimeSpan ConvertRawToTimestamp(long delta) => new TimeSpan((delta * 10000000) / Stopwatch.Frequency);
    }
}
