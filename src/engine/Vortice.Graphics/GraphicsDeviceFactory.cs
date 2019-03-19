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

        /// <summary>
        /// Create new instance of <see cref="GraphicsDeviceFactory"/>
        /// </summary>
        /// <param name="preferredBackend">The preferred of <see cref="GraphicsBackend"/></param>
        /// <param name="validation">Whether to enable validation if supported.</param>
        /// <returns>New instance of <see cref="GraphicsDeviceFactory"/>.</returns>
        public static GraphicsDeviceFactory Create(GraphicsBackend preferredBackend, bool validation)
        {
            if (preferredBackend == GraphicsBackend.Default)
            {
                preferredBackend = GetDefaultGraphicsPlatform(Platform.PlatformType);
            }

            if (!IsSupported(preferredBackend))
            {
                throw new GraphicsException($"Backend {preferredBackend} is not supported");
            }

            switch (preferredBackend)
            {
                case GraphicsBackend.Direct3D11:
                case GraphicsBackend.Direct3D12:
#if !VORTICE_NO_D3D11 && !VORTICE_NO_D3D12
                    return new GraphicsDeviceFactoryD3D(preferredBackend, validation);
#else
                    throw new GraphicsException($"{preferredBackend} backend is not supported");
#endif

                case GraphicsBackend.Vulkan:
#if !VORTICE_NO_VULKAN
                    return new Vulkan.VulkanGraphicsDevice(validation);
#else
                    throw new GraphicsException($"{GraphicsBackend.Vulkan} Backend is not supported");
#endif

                default:
                    throw new GraphicsException($"Invalid {preferredBackend} backend");
            }
        }

        /// <summary>
        /// Gets the best <see cref="GraphicsBackend"/> for given platform.
        /// </summary>
        /// <param name="platformType">The <see cref="PlatformType"/> to detect.</param>
        /// <returns></returns>
        public static GraphicsBackend GetDefaultGraphicsPlatform(PlatformType platformType)
        {
            switch (platformType)
            {
                case PlatformType.Windows:
                case PlatformType.UWP:
                    if (IsSupported(GraphicsBackend.Direct3D12))
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

        /// <summary>
        /// Check if given <see cref="GraphicsBackend"/> is supported.
        /// </summary>
        /// <param name="backend">The <see cref="GraphicsBackend"/> to check.</param>
        /// <returns>True if supported, false otherwise.</returns>
        public static bool IsSupported(GraphicsBackend backend)
        {
            if (backend == GraphicsBackend.Default)
            {
                backend = GetDefaultGraphicsPlatform(Platform.PlatformType);
            }

            switch (backend)
            {
                case GraphicsBackend.Direct3D11:
#if !VORTICE_NO_D3D11
                    return Platform.PlatformType == PlatformType.Windows
                        || Platform.PlatformType == PlatformType.UWP;
#else
                    return false;
#endif

                case GraphicsBackend.Direct3D12:
#if !VORTICE_NO_D3D12
                    return false;
                //return D3D12.DeviceD3D12.IsSupported();
#else
                    return false;
#endif

                case GraphicsBackend.Vulkan:
#if !VORTICE_NO_VULKAN
                    return Vulkan.VulkanGraphicsDevice.IsSupported();
#else
                    return false;
#endif

                default:
                    return false;
            }
        }

        protected abstract GraphicsDevice CreateDeviceImpl(PowerPreference powerPreference);
    }
}
