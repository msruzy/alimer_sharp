// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Vortice.DirectX.Direct3D12;

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
            _resource = device.D3D12Device.CreateCommittedResource(
                new HeapProperties(HeapType.Upload),
                HeapFlags.None,
                ResourceDescription.Buffer((ulong)descriptor.SizeInBytes),
                ResourceStates.GenericRead,
                null);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            bool forceDeferred = _creationFrame == ((DeviceD3D12)Device).CurrentCPUFrame;
            ((DeviceD3D12)Device).DeferredRelease(ref _resource, forceDeferred);
        }
    }
}
