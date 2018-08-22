// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice
{
    /// <summary>
    /// Current timing used for variable-step (real time) or fixed-step (application time) apps.
    /// </summary>
    public sealed class ApplicationTime
    {
        /// <summary>
        /// Gets the amount of time since the start of the <see cref="Application"/>.
        /// </summary>
        /// <value>The total application time.</value>
        public TimeSpan TotalTime { get; private set; }

        /// <summary>
        /// Gets the elapsed game time since the last update
        /// </summary>
        /// <value>The elapsed game time.</value>
        public TimeSpan ElapsedTime { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the game is running slowly than its TargetElapsedTime. This can be used for example to render less details...etc.
        /// </summary>
        /// <value><c>true</c> if this instance is running slowly; otherwise, <c>false</c>.</value>
        public bool IsRunningSlowly { get; private set; }

        /// <summary>
        /// Gets the current frame count since the start of the <see cref="Application"/>.
        /// </summary>
        public int FrameCount { get; private set; }

        internal void Reset(TimeSpan totalTime)
        {
            TotalTime = totalTime;
            ElapsedTime = TimeSpan.Zero;
            IsRunningSlowly = false;
            FrameCount = 0;
        }

        internal void Update(TimeSpan totalTime, TimeSpan elapsedTime, bool isRunningSlowly, bool incrementFrame)
        {
            TotalTime = totalTime;
            ElapsedTime = elapsedTime;
            IsRunningSlowly = isRunningSlowly;

            if (incrementFrame)
            {
                FrameCount++;

                // TODO: Calculate FPS
            }
        }
    }
}
