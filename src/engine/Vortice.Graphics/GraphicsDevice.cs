// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vortice.Graphics
{
    public abstract class GraphicsDevice : DisposableBase
    {
        private readonly object _resourceSyncRoot = new object();
        private readonly List<GraphicsResource> _resources = new List<GraphicsResource>();

        /// <summary>
        /// Gets the <see cref="GraphicsAdapter"/> used for device creation.
        /// </summary>
        public GraphicsAdapter Adapter { get; }

        public PresentationParameters PresentationParameters { get; }

        /// <summary>
        /// Gets the backbuffer <see cref="Texture"/>.
        /// </summary>
        public abstract Texture BackbufferTexture { get; }

        /// <summary>
        /// Gets the immediate <see cref="CommandBuffer"/>
        /// </summary>
        public abstract CommandBuffer ImmediateCommandBuffer { get; }

        protected GraphicsDevice(GraphicsAdapter adapter, PresentationParameters presentationParameters)
        {
            Guard.NotNull(adapter, nameof(adapter));
            Guard.NotNull(presentationParameters, nameof(presentationParameters));

            Adapter = adapter;
            PresentationParameters = presentationParameters;
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

        public void Present()
        {
            PresentCore();
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

            return CreateBufferCore(descriptor, initialData);
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
            Guard.IsTrue(description.TextureType != TextureType.Unknown, nameof(description), $"TextureType cannot be {nameof(TextureType.Unknown)}");
            Guard.MustBeGreaterThanOrEqualTo(description.Width, 1, nameof(description.Width));
            Guard.MustBeGreaterThanOrEqualTo(description.Height, 1, nameof(description.Height));
            Guard.MustBeGreaterThanOrEqualTo(description.Depth, 1, nameof(description.Depth));

            return CreateTextureCore(description);
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
        protected abstract void PresentCore();

        protected abstract GraphicsBuffer CreateBufferCore(in BufferDescriptor descriptor, IntPtr initialData);
        protected abstract Texture CreateTextureCore(in TextureDescription description);
    }
}
