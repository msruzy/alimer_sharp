// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Diagnostics
{
    /// <summary>
	/// Default platform logger.
	/// </summary>
    internal class DefaultLogger : ILogger
    {
        public DefaultLogger()
        {
#if DEBUG
            Level = LogLevel.Debug;
#else
            Level = LogLevel.Info;
#endif
        }

        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc/>
        public LogLevel Level { get; set; }

        /// <inheritdoc/>
		bool ILogger.IsLevelEnabled(LogLevel level) => (int)level < (int)Level;

        /// <inheritdoc/>
		public void Log(LogLevel level, string message, Exception exception)
        {
            Log(level, null, message, exception);
        }

        /// <inheritdoc/>
		public void Log(LogLevel level, string tag, string message, Exception exception)
        {
            if (!IsEnabled || (int)level < (int)Level)
            {
                return;
            }

#if ANDROID
			Android.Util.LogPriority logPriority = Android.Util.LogPriority.Verbose;
            switch (level)
            {
                case LogLevel.Trace:
                    logPriority = Android.Util.LogPriority.Verbose;
                    break;
                case LogLevel.Debug:
                    logPriority = Android.Util.LogPriority.Debug;
                    break;
                case LogLevel.Info:
                    logPriority = Android.Util.LogPriority.Info;
                    break;
                case LogLevel.Warn:
                    logPriority = Android.Util.LogPriority.Warn;
                    break;
                case LogLevel.Error:
                    logPriority = Android.Util.LogPriority.Error;
                    break;
                case LogLevel.Critical:
                    logPriority = Android.Util.LogPriority.Assert;
                    break;
            }
            Android.Util.Log.WriteLine(logPriority, tag ?? "Vortice", message);
#else
            string printMessage;
            if (!string.IsNullOrEmpty(tag))
            {
                printMessage = $"{level.ToString().ToUpperInvariant()}/{tag} : {message}";
            }
            else
            {
                printMessage = $"{level.ToString().ToUpperInvariant()} : {message}";
            }
            System.Diagnostics.Debug.WriteLine(printMessage);
#endif
        }
    }
}
