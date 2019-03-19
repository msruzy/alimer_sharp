// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using static Vortice.Graphics.D3D11.Utils;
using SharpD3D11;

namespace Vortice.Graphics.D3D11
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
                Usage = (SharpD3D11.Usage)descriptor.Usage,
                BindFlags = Convert(descriptor.BufferUsage),
                CpuAccessFlags = Convert(descriptor.Usage),
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            if ((descriptor.BufferUsage & BufferUsage.Indirect) != 0)
            {
                description.OptionFlags |= ResourceOptionFlags.DrawIndirectArgs;
            }

            Resource = device.Device.CreateBuffer(description, initialData);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Resource.Dispose();
        }
    }
}
