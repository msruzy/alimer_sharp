// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
	/// Describes gpu adapter power preference.
	/// </summary>
	public enum PowerPreference
    {
        /// <summary>
        /// Default not specified gpu adapter.
        /// </summary>
        Default,
        /// <summary>
        /// Low power gpu adapter.
        /// </summary>
        LowPower,
        /// <summary>
        /// High performance (discrete) gpu adapter.
        /// </summary>
        HighPerformance
    }
}
