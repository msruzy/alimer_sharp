// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Diagnostics
{
    /// <summary>
	/// Interface for logging functionalities.
	/// </summary>
    public interface ILogger
    {
        /// <summary>
		/// Gets or sets enable state of log.
		/// </summary>
		bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the log level.
        /// </summary>
        LogLevel Level { get; set; }

        /// <summary>
		/// Gets whether given log level is enabled.
		/// </summary>
		/// <param name="level">The level to check.</param>
		/// <returns><c>True</c> if enabled, <c>false</c> otherwise.</returns>
		bool IsLevelEnabled(LogLevel level);

        /// <summary>
        /// Logs a message at the specified log level.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="message">Log message.</param>
        /// <param name="exception">Optional exception to log with the message.</param>
        void Log(LogLevel level, string message, Exception exception = null);

        /// <summary>
        /// Logs a message at the specified log level.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="tag">Log tag.</param>
        /// <param name="message">Log message.</param>
        /// <param name="exception">Optional exception to log with the message.</param>
        void Log(LogLevel level, string tag, string message, Exception exception = null);
    }
}
