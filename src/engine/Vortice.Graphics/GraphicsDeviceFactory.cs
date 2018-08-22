// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Vortice.Graphics
{
    /// <summary>
    /// Class for enumerating the supported <see cref="GraphicsAdapter"/> and <see cref="GraphicsDevice"/> creation.
    /// </summary>
    public class GraphicsDeviceFactory : IDisposable
    {
        private readonly IGraphicsDeviceFactory _implementation;

        public GraphicsBackend Backend { get; }

        public bool Validation => _implementation.Validation;

        /// <summary>
		/// Gets the best supported default <see cref="GraphicsAdapter"/>.
		/// </summary>
		public GraphicsAdapter DefaultAdapter => Adapters[0];

        /// <summary>
        /// Gets collection of <see cref="GraphicsAdapter"/>.
        /// </summary>
        public IReadOnlyList<GraphicsAdapter> Adapters => _implementation.Adapters;

        /// <summary>
        /// Create a new instance of <see cref="GraphicsDeviceFactory"/> class.
        /// </summary>
        /// <param name="backend">The factory <see cref="GraphicsDeviceFactory"/> type.</param>
        /// <param name="validation">Enable debug/validation support.</param>
        public GraphicsDeviceFactory(GraphicsBackend backend, bool validation)
        {
            if (backend == GraphicsBackend.Default)
            {
                // Find best supported per platform.
                if (Platform.PlatformType == PlatformType.Windows
                    || Platform.PlatformType == PlatformType.WindowsUniversal)
                {
                    if (IsSupported(GraphicsBackend.Direct3D12))
                    {
                        backend = GraphicsBackend.Direct3D12;
                    }
                    else if (IsSupported(GraphicsBackend.Direct3D11))
                    {
                        backend = GraphicsBackend.Direct3D11;
                    }
                }
            }

            if (!IsSupported(backend))
                throw new GraphicsException($"Backend {backend} is not supported");

            switch (backend)
            {
                case GraphicsBackend.Direct3D11:
#if VORTICE_D3D11
                    _implementation = new D3D11.D3D11GraphicsDeviceFactory(validation);
#endif
                    break;

                case GraphicsBackend.Direct3D12:
#if VORTICE_D3D12
                    _implementation = new D3D12.D3D12GraphicsDeviceFactory(validation);
#endif
                    break;
            }

            Backend = backend;
        }

        public void Dispose()
        {
            _implementation.Destroy();
        }

        /// <summary>
        /// Check if given <see cref="GraphicsBackend"/> is supported.
        /// </summary>
        /// <param name="backend">The <see cref="GraphicsBackend"/> to check.</param>
        /// <returns>True if supported, false otherwise.</returns>
        public static bool IsSupported(GraphicsBackend backend)
        {
            switch (backend)
            {
                case GraphicsBackend.Direct3D11:
#if VORTICE_D3D11
                    return D3D11.D3D11GraphicsDeviceFactory.IsSupported();
#else
                    return false;
#endif

                case GraphicsBackend.Direct3D12:
#if VORTICE_D3D12
                    return D3D12.D3D12GraphicsDeviceFactory.IsSupported();
#else
                    return false;
#endif

                default:
                    return false;
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

            return _implementation.CreateGraphicsDevice(adapter, presentationParameters);
        }
    }
}
