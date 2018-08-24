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
        /// Gets the default <see cref="CommandContext"/>.
        /// </summary>
        public abstract CommandContext DefaultContext { get; }

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

        public GraphicsBuffer CreateBuffer(BufferUsage usage, int sizeInBytes, IntPtr data)
        {
            Guard.IsTrue(usage != BufferUsage.Unknown, nameof(usage), $"BufferUsage cannot be {nameof(BufferUsage.Unknown)}");
            Guard.MustBeGreaterThan(sizeInBytes, 0, nameof(sizeInBytes));

            return CreateBufferCore(usage, sizeInBytes, data);
        }

        public GraphicsBuffer CreateBuffer(BufferUsage usage, int sizeInBytes) => CreateBuffer(usage, sizeInBytes, IntPtr.Zero);

        public GraphicsBuffer CreateBuffer<T>(BufferUsage usage, T[] data) where T : struct
        {
            Guard.NotNull(data, nameof(data));
            Guard.MustBeGreaterThan(data.Length, 0, nameof(data));

            var sizeInBytes = Unsafe.SizeOf<T>() * data.Length;
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var buffer = CreateBuffer(usage, sizeInBytes, handle.AddrOfPinnedObject());
            handle.Free();
            return buffer;
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

        protected abstract GraphicsBuffer CreateBufferCore(BufferUsage usage, int sizeInBytes, IntPtr data);
        protected abstract Texture CreateTextureCore(in TextureDescription description);
    }
}
