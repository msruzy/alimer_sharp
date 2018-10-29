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
        /// Create new instance of <see cref="GraphicsDeviceFactory"/>
        /// </summary>
        /// <param name="backend">The type of <see cref="GraphicsBackend"/></param>
        /// <param name="validation">Whether to enable validation if supported.</param>
        /// <returns></returns>
        public static GraphicsDeviceFactory Create(GraphicsBackend backend, bool validation)
        {
            if (backend == GraphicsBackend.Default)
            {
                backend = GetDefaultGraphicsPlatform(Platform.PlatformType);
            }

            if (!GraphicsDevice.IsSupported(backend))
            {
                throw new GraphicsException($"Backend {backend} is not supported");
            }

            switch (backend)
            {
                case GraphicsBackend.Direct3D11:
#if !VORTICE_NO_D3D11
                    return new D3D11.D3D11GraphicsDeviceFactory(validation);
#else
                    throw new GraphicsException($"{GraphicsBackend.Direct3D11} Backend is not supported");
#endif

                case GraphicsBackend.Direct3D12:
#if !VORTICE_NO_D3D12
                    return new D3D12.D3D12GraphicsDeviceFactory(validation);
#else
                    throw new GraphicsException($"{GraphicsBackend.Direct3D12} Backend is not supported");
#endif

                case GraphicsBackend.Vulkan:
#if !VORTICE_NO_D3D12
                    return new Vulkan.VulkanGraphicsDeviceFactory(validation);
#else
                    throw new GraphicsException($"{GraphicsBackend.Vulkan} Backend is not supported");
#endif

                default:
                    throw new GraphicsException($"Invalid {backend} backend");
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

        private static GraphicsBackend GetDefaultGraphicsPlatform(PlatformType platformType)
        {
            switch (platformType)
            {
                case PlatformType.Windows:
                case PlatformType.UWP:
                    if (D3D12.D3D12GraphicsDevice.IsSupported())
                    {
                        return GraphicsBackend.Direct3D12;
                    }

                    return GraphicsBackend.Direct3D11;
                case PlatformType.Android:
                case PlatformType.Linux:
                    return GraphicsBackend.OpenGLES;

                case PlatformType.iOS:
                case PlatformType.macOS:
                    return GraphicsBackend.OpenGL;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
