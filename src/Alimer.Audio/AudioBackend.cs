// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Audio
{
    /// <summary>
    /// Defines the type of <see cref="AudioEngine"/>.
    /// </summary>
    public enum AudioBackend
    {
        /// <summary>
        /// Best supported device on running platform.
        /// </summary>
        Default,

        /// <summary>
		/// Custom plugin backend.
		/// </summary>
		Custom,

        /// <summary>
        /// OpenAL backend.
        /// </summary>
        OpenAL,
    }
}
