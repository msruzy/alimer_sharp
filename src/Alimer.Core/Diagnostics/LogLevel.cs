// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Diagnostics
{
    /// <summary>
	/// Enum describing level of logging.
	/// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Trace log level.
        /// </summary>
        Trace = 0,

        /// <summary>
        /// Debug log level.
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Information log level.
        /// </summary>
        Info = 2,

        /// <summary>
        /// Warning log level.
        /// </summary>
        Warn = 3,

        /// <summary>
        /// Error log level.
        /// </summary>
        Error = 4,

        /// <summary>
        /// Critical/Fatal log level.
        /// </summary>
        Critical = 5
    }
}
