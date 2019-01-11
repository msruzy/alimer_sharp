// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using static Vortice.Graphics.D3D11.Utils;

namespace Vortice.Graphics.D3D11
{
    internal class BufferD3D11 : GraphicsBuffer
    {
        public readonly Buffer Resource;

        public BufferD3D11(DeviceD3D11 device, in BufferDescriptor descriptor, IntPtr initialData)
            : base(device, descriptor)
        {
            var description = new BufferDescription()
            {
                SizeInBytes = descriptor.SizeInBytes,
                Usage = (ResourceUsage)descriptor.Usage,
                BindFlags = Convert(descriptor.BufferUsage),
                CpuAccessFlags = Convert(descriptor.Usage),
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            if ((descriptor.BufferUsage & BufferUsage.Indirect) != 0)
            {
                description.OptionFlags |= ResourceOptionFlags.DrawIndirectArguments;
            }

            Resource = new Buffer(device.D3DDevice, initialData, description);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Resource.Dispose();
        }
    }
}
