// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDirect3D12;

namespace Vortice.Graphics.D3D12
{
    internal class BufferD3D12 : GraphicsBuffer
    {
        private ID3D12Resource _resource;
        private readonly ulong _creationFrame;

        public BufferD3D12(DeviceD3D12 device, in BufferDescriptor descriptor, IntPtr initialData)
            : base(device, descriptor)
        {
            _creationFrame = device.CurrentCPUFrame;
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            bool forceDeferred = _creationFrame == ((DeviceD3D12)Device).CurrentCPUFrame;
            ((DeviceD3D12)Device).DeferredRelease(ref _resource, forceDeferred);
        }
    }
}
