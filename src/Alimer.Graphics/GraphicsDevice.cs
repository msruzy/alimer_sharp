// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Alimer.Graphics
{
    /// <summary>
    /// Defines a graphics device class.
    /// </summary>
    public abstract class GraphicsDevice : DisposableBase
    {
        private readonly object _resourceSyncRoot = new object();
        private readonly List<GraphicsResource> _resources = new List<GraphicsResource>();
        protected CommandQueue _graphicsCommandQueue;
        protected CommandQueue _computeCommandQueue;
        protected CommandQueue _copyCommandQueue;

        /// <summary>
        /// Gets the device <see cref="GraphicsBackend"/>.
        /// </summary>
        public GraphicsBackend Backend => Info.Backend;

        /// <summary>
        /// Gets value indicating gpu validation enable state.
        /// </summary>
        public bool Validation { get; protected set; }

        /// <summary>
        /// Gets the information of this device.
        /// </summary>
        public GraphicsDeviceInfo Info { get; } = new GraphicsDeviceInfo();

        /// <summary>
        /// Gets the capabilities of this device.
        /// </summary>
        public GraphicsDeviceCapabilities Capabilities { get; } = new GraphicsDeviceCapabilities();

        /// <summary>
        /// Create new instance of <see cref="GraphicsDevice"/> class.
        /// </summary>
        protected GraphicsDevice()
        {
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

        public CommandQueue GetCommandQueue(CommandQueueType type = CommandQueueType.Graphics)
        {
            switch (type)
            {
                case CommandQueueType.Graphics:
                    return _graphicsCommandQueue;
                case CommandQueueType.Compute:
                    return _computeCommandQueue;
                case CommandQueueType.Copy:
                    return _copyCommandQueue;
                default:
                    Debug.Fail("Invalid command queue type.");
                    return null;
            }
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
            Guard.IsTrue(descriptor.Usage != BufferUsage.None, nameof(descriptor.Usage), $"BufferUsage cannot be {nameof(BufferUsage.None)}");
            Guard.MustBeGreaterThan(descriptor.SizeInBytes, 0u, nameof(descriptor.SizeInBytes));

            if (descriptor.ResourceUsage == GraphicsResourceUsage.Immutable
                && initialData == IntPtr.Zero)
            {
                throw new GraphicsException("Immutable buffer needs valid initial data.");
            }

            return CreateBufferImpl(descriptor, initialData);
        }

        public GraphicsBuffer CreateBuffer(in BufferDescriptor descriptor) => CreateBuffer(descriptor, IntPtr.Zero);

        public unsafe GraphicsBuffer CreateBuffer<T>(BufferDescriptor descriptor, T[] initialData) where T : unmanaged
        {
            Guard.NotNull(initialData, nameof(initialData));
            Guard.MustBeGreaterThan(initialData.Length, 0, nameof(initialData));

            // Calculate size in bytes if not provided.
            if (descriptor.SizeInBytes == 0)
            {
                descriptor.SizeInBytes = (uint)(sizeof(T) * initialData.Length);
            }

            var span = initialData.AsSpan();
            return CreateBuffer(descriptor, (IntPtr)Unsafe.AsPointer(ref span.GetPinnableReference()));
        }

        /// <summary>
        /// Create a new immutable <see cref="GraphicsBuffer"/>.
        /// </summary>
        /// <typeparam name="T">Type of data</typeparam>
        /// <param name="bufferUsage">The <see cref="BufferUsage"/></param>
        /// <param name="initialData">Valid iniitial data</param>
        /// <returns>New instance of <see cref="GraphicsBuffer"/></returns>
        public unsafe GraphicsBuffer CreateBuffer<T>(BufferUsage bufferUsage, Span<T> initialData) where T : unmanaged
        {
            return CreateBuffer(
                new BufferDescriptor((uint)(sizeof(T) * initialData.Length), bufferUsage, GraphicsResourceUsage.Immutable),
                (IntPtr)Unsafe.AsPointer(ref initialData.GetPinnableReference()));
        }

        public Texture CreateTexture(in TextureDescriptor descriptor)
        {
            return CreateTextureCore(descriptor);
        }

        public Sampler CreateSampler(in SamplerDescriptor descriptor)
        {
            return CreateSamplerCore(descriptor);
        }

        public Shader CreateShader(in ShaderBytecode bytecode)
        {
            Guard.NotNullOrEmpty(bytecode.Data, nameof(bytecode), "Invalid bytecode data");

            return CreateShaderImpl(bytecode);
        }

        public PipelineState CreateRenderPipelineState(in RenderPipelineDescriptor descriptor)
        {
            Guard.NotNull(descriptor, nameof(descriptor), "Invalid RenderPipeline descriptor");

            return CreateRenderPipelineStateImpl(descriptor);
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

        protected abstract SwapChain CreateSwapChainImpl(in SwapChainDescriptor descriptor);
        protected abstract GraphicsBuffer CreateBufferImpl(in BufferDescriptor descriptor, IntPtr initialData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract Texture CreateTextureCore(in TextureDescriptor descriptor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract Sampler CreateSamplerCore(in SamplerDescriptor descriptor);
        protected abstract Shader CreateShaderImpl(in ShaderBytecode bytecode);
        protected abstract PipelineState CreateRenderPipelineStateImpl(in RenderPipelineDescriptor descriptor);
    }
}
