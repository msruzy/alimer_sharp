// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12Buffer : GraphicsBuffer
    {
        private Resource _resource;
        private readonly long _creationFrame;

        public D3D12Buffer(D3D12GraphicsDevice device, in BufferDescriptor descriptor, IntPtr initialData)
            : base(device, descriptor)
        {
            _creationFrame = device.CurrentCPUFrame;
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            bool forceDeferred = _creationFrame == ((D3D12GraphicsDevice)Device).CurrentCPUFrame;
            ((D3D12GraphicsDevice)Device).DeferredRelease(ref _resource, forceDeferred);
        }
    }
}
