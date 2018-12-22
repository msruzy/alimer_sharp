// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using static Vortice.Graphics.D3D11.Utils;

namespace Vortice.Graphics.D3D11
{
    internal class BufferD3D11 : GPUBuffer
    {
        public readonly Buffer Resource;

        public BufferD3D11(GPUDeviceD3D11 device, in BufferDescriptor descriptor, IntPtr initialData)
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
        public override void Destroy()
        {
            Resource.Dispose();
        }

        /// <summary>
        /// Implicit casting operator to <see cref="SharpDX.Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="buffer">The <see cref="BufferD3D11"/> to convert from.</param>
        public static implicit operator Buffer(BufferD3D11 buffer) => buffer.Resource;
    }
}
