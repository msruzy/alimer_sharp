// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Describes capabilities supported by given instance of <see cref="GraphicsDevice"/>.
    /// </summary>
    public sealed class GraphicsDeviceCapabilities
    {
        /// <summary>
        /// Specifies all supported hardware features.
        /// </summary>
        public GraphicsDeviceFeatures Features { get; } = new GraphicsDeviceFeatures();

        /// <summary>
        /// Specifies all rendering limitations.
        /// </summary>
        public GraphicsDeviceLimits Limits { get; } = new GraphicsDeviceLimits();
    }
}
