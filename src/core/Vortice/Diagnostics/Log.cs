// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Diagnostics
{
    /// <summary>
	/// Interface for logging functionalities.
	/// </summary>
    public static class Log
    {
        public static ILogger Logger { get; set; } = new DefaultLogger();

        /// <summary>
		/// Logs the specified trace message with exception.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="exception">Exception to log with the message.</param>
		public static void Trace(string message, Exception exception = null)
        {
            Logger.Log(LogLevel.Trace, message, exception);
        }

        /// <summary>
		/// Logs the specified debug message with exception.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="exception">Exception to log with the message.</param>
		public static void Debug(string message, Exception exception = null)
        {
            Logger.Log(LogLevel.Debug, message, exception);
        }

        /// <summary>
		/// Logs the specified debug message with exception.
		/// </summary>
        /// <param name="tag">Log tag.</param>
		/// <param name="message">The message to log.</param>
		/// <param name="exception">Exception to log with the message.</param>
		public static void Debug(string tag, string message, Exception exception = null)
        {
            Logger.Log(LogLevel.Debug, tag, message, exception);
        }

        /// <summary>
		/// Logs the specified info message with exception.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="exception">Exception to log with the message.</param>
		public static void Info(string message, Exception exception = null)
        {
            Logger.Log(LogLevel.Info, message, exception);
        }

        /// <summary>
		/// Logs the specified warning message with exception.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="exception">Exception to log with the message.</param>
		public static void Warn(string message, Exception exception = null)
        {
            Logger.Log(LogLevel.Warn, message, exception);
        }

        /// <summary>
		/// Logs the specified error message with exception.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="exception">Exception to log with the message.</param>
		public static void Error(string message, Exception exception = null)
        {
            Logger.Log(LogLevel.Error, message, exception);
        }

        /// <summary>
		/// Logs the specified critical message with exception.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="exception">Exception to log with the message.</param>
		public static void Critical(string message, Exception exception = null)
        {
            Logger.Log(LogLevel.Critical, message, exception);
        }
    }
}
