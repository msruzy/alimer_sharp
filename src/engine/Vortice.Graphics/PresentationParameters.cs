// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Contains <see cref="GraphicsDevice"/> presentation parameters.
    /// </summary>
    public sealed class PresentationParameters
    {
        /// <summary>
        /// Gets or Sets a value indicating the width of the SwapChain back buffer.
        /// </summary>
        public int BackBufferWidth { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating the height of the SwapChain back buffer.
        /// </summary>
        public int BackBufferHeight { get; set; }

        /// <summary>
        /// Gets or Sets the native handle to the device window.
        /// </summary>
        public object DeviceWindowHandle { get; set; }
    }
}
