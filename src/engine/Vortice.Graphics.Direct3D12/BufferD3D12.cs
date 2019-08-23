// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Vortice.DirectX.Direct3D12;

namespace Vortice.Graphics.Direct3D12
{
    internal class BufferD3D12 : GraphicsBuffer
    {
        private ID3D12Resource _resource;
        private readonly long _creationFrame;
        private ResourceStates _resourceState;

        public BufferD3D12(D3D12GraphicsDevice device, in BufferDescriptor descriptor, IntPtr initialData)
            : base(device, descriptor)
        {
            _creationFrame = device.CurrentCPUFrame;
            long bufferSize = descriptor.SizeInBytes;
            if (descriptor.Usage == BufferUsage.Constant)
            {
                bufferSize = Utilities.AlignUp(descriptor.SizeInBytes);
            }

            ResourceFlags resourceFlags = ResourceFlags.None;
            _resourceState = ResourceStates.Common;
            HeapType heapType = HeapType.Default;
            if (descriptor.ResourceUsage == GraphicsResourceUsage.Staging)
            {
                heapType = HeapType.Readback;
                _resourceState = ResourceStates.CopyDestination;
            }
            else if (descriptor.ResourceUsage == GraphicsResourceUsage.Dynamic)
            {
                heapType = HeapType.Upload;
                _resourceState = ResourceStates.GenericRead;
            }

            _resource = device.D3D12Device.CreateCommittedResource(
                new HeapProperties(heapType),
                HeapFlags.None,
                ResourceDescription.Buffer(bufferSize, resourceFlags, 0),
                _resourceState);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            bool forceDeferred = _creationFrame == ((D3D12GraphicsDevice)Device).CurrentCPUFrame;
            ((D3D12GraphicsDevice)Device).DeferredRelease(ref _resource, forceDeferred);
        }
    }
}
