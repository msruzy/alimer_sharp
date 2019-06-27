// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Vortice.DirectX.Direct3D11;

namespace Vortice.Graphics.Direct3D11
{
    internal class BufferD3D11 : GraphicsBuffer
    {
        public readonly ID3D11Buffer Resource;

        public BufferD3D11(DeviceD3D11 device, in BufferDescriptor descriptor, IntPtr initialData)
            : base(device, descriptor)
        {
            var description = new BufferDescription()
            {
                ByteWidth = descriptor.SizeInBytes,
                Usage = (Vortice.DirectX.Direct3D11.Usage)descriptor.Usage,
                BindFlags = descriptor.BufferUsage.ToDirectX(),
                CpuAccessFlags = descriptor.Usage.ToDirectX(),
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            if ((descriptor.BufferUsage & BufferUsage.Indirect) != 0)
            {
                description.OptionFlags |= ResourceOptionFlags.DrawIndirectArgs;
            }

            Resource = device.D3D11Device.CreateBuffer(description, initialData);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Resource.Dispose();
        }
    }
}
