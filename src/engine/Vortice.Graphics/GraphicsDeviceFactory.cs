// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Vortice.Graphics
{
    /// <summary>
    /// Class for enumerating the supported <see cref="GraphicsAdapter"/> and <see cref="GraphicsDevice"/> creation.
    /// </summary>
    public abstract class GraphicsDeviceFactory : DisposableBase
    {
        protected readonly List<GraphicsAdapter> _adapters = new List<GraphicsAdapter>();

        public GraphicsBackend Backend { get; }

        public bool Validation { get; protected set; }

        /// <summary>
		/// Gets the best supported default <see cref="GraphicsAdapter"/>.
		/// </summary>
		public GraphicsAdapter DefaultAdapter => Adapters[0];

        /// <summary>
        /// Gets collection of <see cref="GraphicsAdapter"/>.
        /// </summary>
        public IReadOnlyList<GraphicsAdapter> Adapters => _adapters;

        /// <summary>
        /// Create a new instance of <see cref="GraphicsDeviceFactory"/> class.
        /// </summary>
        /// <param name="backend">The factory <see cref="GraphicsDeviceFactory"/> type.</param>
        /// <param name="validation">Enable debug/validation support.</param>
        protected GraphicsDeviceFactory(GraphicsBackend backend, bool validation)
        {
            Backend = backend;
            Validation = validation;
        }

        /// <inheritdoc/>
        protected sealed override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                Destroy();
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="GraphicsDevice"/> using adapter.
        /// </summary>
        /// <param name="adapter">The <see cref="GraphicsAdapter"/> to use.</param>
        /// <returns>New instance of <see cref="GraphicsDevice"/></returns>
        public GraphicsDevice CreateGraphicsDevice(GraphicsAdapter adapter, PresentationParameters presentationParameters)
        {
            Guard.NotNull(adapter, nameof(adapter));
            Guard.NotNull(presentationParameters, nameof(presentationParameters));

            return CreateGraphicsDeviceImpl(adapter, presentationParameters);
        }

        /// <summary>
        /// Unconditionally destroy this factory.
        /// </summary>
        protected abstract void Destroy();

        /// <summary>
        /// <see cref="CreateGraphicsDevice"/> implementation.
        /// </summary>
        protected abstract GraphicsDevice CreateGraphicsDeviceImpl(GraphicsAdapter adapter, PresentationParameters presentationParameters);
    }
}
