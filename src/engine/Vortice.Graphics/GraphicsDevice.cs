// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a graphics device class.
    /// </summary>
    public abstract class GraphicsDevice : DisposableBase
    {
        private readonly object _resourceSyncRoot = new object();
        private readonly List<GraphicsResource> _resources = new List<GraphicsResource>();

        /// <summary>
        /// Gets the device <see cref="GraphicsBackend"/>.
        /// </summary>
        public GraphicsBackend Backend { get; }

        /// <summary>
        /// Gets value indicating gpu validation enable state.
        /// </summary>
        public bool Validation { get; protected set; }

        /// <summary>
        /// Gets the features of this device.
        /// </summary>
        public GraphicsDeviceFeatures Features { get; }

        /// <summary>
        /// Gets the immediate <see cref="CommandBuffer"/> created with device.
        /// </summary>
        public abstract CommandBuffer ImmediateContext { get; }

        /// <summary>
        /// Create new instance of <see cref="GraphicsDevice"/> class.
        /// </summary>
        /// <param name="backend"></param>
        protected GraphicsDevice(GraphicsBackend backend)
        {
            Guard.IsTrue(backend != GraphicsBackend.Default, nameof(backend), "Invalid backend");

            Backend = backend;
            Features = new GraphicsDeviceFeatures();
        }

        /// <inheritdoc/>
        protected sealed override void Dispose(bool disposing)
        {
            if (disposing
                && !IsDisposed)
            {
                DestroyAllResources();
                Destroy();
            }

            base.Dispose(disposing);
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
                    return D3D12.DeviceD3D12.IsSupported();
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

        /// <summary>
        /// Create new instance of <see cref="GraphicsDevice"/>
        /// </summary>
        /// <param name="preferredBackend">The preferred of <see cref="GraphicsBackend"/></param>
        /// <param name="validation">Whether to enable validation if supported.</param>
        /// <returns>New instance of <see cref="GraphicsDevice"/>.</returns>
        public static GraphicsDevice Create(GraphicsBackend preferredBackend, bool validation)
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
#if !VORTICE_NO_D3D11
                    return new D3D11.DeviceD3D11(validation);
#else
                    throw new GraphicsException($"{GraphicsBackend.Direct3D11} Backend is not supported");
#endif

                case GraphicsBackend.Direct3D12:
#if !VORTICE_NO_D3D12
                    return new D3D12.DeviceD3D12(validation);
#else
                    throw new GraphicsException($"{GraphicsBackend.Direct3D12} Backend is not supported");
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
        private static GraphicsBackend GetDefaultGraphicsPlatform(PlatformType platformType)
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
        /// Present current frame and advance to next frame.
        /// </summary>
        public void Frame()
        {
            FrameCore();
        }

        public void WaitIdle()
        {
            WaitIdleCore();
        }

        /// <summary>
        /// Create new <see cref="SwapChain"/>.
        /// </summary>
        /// <param name="descriptor">The <see cref="SwapChainDescriptor"/> that describes the swap chain.</param>
        /// <returns>New instance of <see cref="SwapChain"/>.</returns>
        public SwapChain CreateSwapChain(in SwapChainDescriptor descriptor)
        {
            return CreateSwapChainImpl(descriptor);
        }

        public GraphicsBuffer CreateBuffer(in BufferDescriptor descriptor, IntPtr initialData)
        {
            Guard.IsTrue(descriptor.BufferUsage != BufferUsage.Unknown, nameof(descriptor.Usage), $"BufferUsage cannot be {nameof(BufferUsage.Unknown)}");
            Guard.MustBeGreaterThan(descriptor.SizeInBytes, 0, nameof(descriptor.SizeInBytes));

            if (descriptor.Usage == GraphicsResourceUsage.Immutable
                && initialData == IntPtr.Zero)
            {
                throw new GraphicsException("Immutable buffer needs valid initial data.");
            }

            return CreateBufferImpl(descriptor, initialData);
        }

        public GraphicsBuffer CreateBuffer(in BufferDescriptor descriptor) => CreateBuffer(descriptor, IntPtr.Zero);

        public GraphicsBuffer CreateBuffer<T>(BufferDescriptor descriptor, T[] initialData) where T : struct
        {
            Guard.NotNull(initialData, nameof(initialData));
            Guard.MustBeGreaterThan(initialData.Length, 0, nameof(initialData));

            // Calculate size in bytes if not provided.
            if (descriptor.SizeInBytes == 0)
            {
                descriptor.SizeInBytes = Unsafe.SizeOf<T>() * initialData.Length;
            }

            var handle = GCHandle.Alloc(initialData, GCHandleType.Pinned);
            var buffer = CreateBuffer(descriptor, handle.AddrOfPinnedObject());
            handle.Free();
            return buffer;
        }

        /// <summary>
        /// Create a new immutable <see cref="GraphicsBuffer"/>.
        /// </summary>
        /// <typeparam name="T">Type of data</typeparam>
        /// <param name="bufferUsage">The <see cref="BufferUsage"/></param>
        /// <param name="initialData">Valid iniitial data</param>
        /// <returns>New instance of <see cref="GraphicsBuffer"/></returns>
        public GraphicsBuffer CreateBuffer<T>(BufferUsage bufferUsage, T[] initialData) where T : struct
        {
            return CreateBuffer(
                new BufferDescriptor(Unsafe.SizeOf<T>() * initialData.Length, bufferUsage, GraphicsResourceUsage.Immutable),
                initialData);
        }

        public Texture CreateTexture(in TextureDescription description)
        {
            return CreateTextureImpl(description);
        }

        public Shader CreateShader(byte[] vertex, byte[] pixel)
        {
            return CreateShaderImpl(vertex, pixel);
        }

        public Framebuffer CreateFramebuffer(FramebufferAttachment[] colorAttachments, FramebufferAttachment? depthStencilAttachment)
        {
            if (colorAttachments.Length == 0
                && depthStencilAttachment == null)
            {
                throw new GraphicsException("Cannot create Framebuffer with no color attachments nor depth stencil attachment");
            }

            return CreateFramebufferImpl(colorAttachments, depthStencilAttachment);
        }

        internal void TrackResource(GraphicsResource resource)
        {
            lock (_resourceSyncRoot)
            {
                _resources.Add(resource);
            }
        }

        internal void UntrackResource(GraphicsResource resource)
        {
            lock (_resourceSyncRoot)
            {
                _resources.Remove(resource);
            }
        }

        private void DestroyAllResources()
        {
            if (_resources.Count > 0)
            {
                lock (_resourceSyncRoot)
                {
                    _resources.Sort((x, y) => x.ResourceType.CompareTo(y.ResourceType));
                    var copyResourceData = _resources.ToArray();
                    for (var i = 0; i < copyResourceData.Length; i++)
                    {
                        var gpuResource = copyResourceData[i];
                        gpuResource.Dispose();
                    }
                }
            }
        }

        protected abstract void Destroy();
        protected abstract void FrameCore();

        protected abstract void WaitIdleCore();

        protected abstract GraphicsBuffer CreateBufferImpl(in BufferDescriptor descriptor, IntPtr initialData);
        protected abstract Texture CreateTextureImpl(in TextureDescription description);
        protected abstract Shader CreateShaderImpl(byte[] vertex, byte[] pixel);

        protected abstract Framebuffer CreateFramebufferImpl(FramebufferAttachment[] colorAttachments, FramebufferAttachment? depthStencilAttachment);

        protected abstract SwapChain CreateSwapChainImpl(in SwapChainDescriptor descriptor);
    }
}
