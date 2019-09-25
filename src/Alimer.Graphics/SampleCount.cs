// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
{
    /// <summary>
	/// Enum describing the number of samples.
	/// </summary>
	public enum SampleCount : byte
    {
        /// <summary>
        /// 1 sample (no multi-sampling).
        /// </summary>
        Count1 = 1,

        /// <summary>
        /// 2 Samples.
        /// </summary>
        Count2 = 2,

        /// <summary>
        /// 4 Samples.
        /// </summary>
        Count4 = 4,

        /// <summary>
        /// 8 Samples.
        /// </summary>
        Count8 = 8,

        /// <summary>
        /// 16 Samples.
        /// </summary>
        Count16 = 16,

        /// <summary>
        /// 32 Samples.
        /// </summary>
        Count32 = 32,
    }
}
