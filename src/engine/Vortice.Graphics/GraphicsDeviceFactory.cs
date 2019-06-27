// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Describes factory for enumerating physical devices and creation of <see cref="GraphicsDevice"/>.
    /// </summary>
    public abstract class GraphicsDeviceFactory
    {
        /// <summary>
        /// Gets the device <see cref="GraphicsBackend"/>.
        /// </summary>
        public GraphicsBackend Backend { get; }

        /// <summary>
        /// Gets value indicating if gpu validation is enabled.
        /// </summary>
        public bool Validation { get; protected set; }

        /// <summary>
        /// Create new instance of <see cref="GraphicsDeviceFactory"/> class.
        /// </summary>
        /// <param name="backend"></param>
        /// <param name="validation"></param>
        protected GraphicsDeviceFactory(GraphicsBackend backend, bool validation)
        {
            Guard.IsTrue(backend != GraphicsBackend.Default, nameof(backend), "Invalid backend");

            Backend = backend;
            Validation = validation;
        }

        public GraphicsDevice CreateDevice(PowerPreference powerPreference = PowerPreference.Default)
        {
            return CreateDeviceImpl(powerPreference);
        }

        protected abstract GraphicsDevice CreateDeviceImpl(PowerPreference powerPreference);
    }
}
