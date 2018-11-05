// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Vortice.Graphics.D3D11
{
    internal class D3D11Buffer : GraphicsBuffer
    {
        public readonly Buffer Resource;

        public D3D11Buffer(D3D11GraphicsDevice device, in BufferDescriptor descriptor, IntPtr initialData)
            : base(device, descriptor)
        {
            var description = new BufferDescription()
            {
                SizeInBytes = descriptor.SizeInBytes,
                Usage = (ResourceUsage)descriptor.Usage,
                BindFlags = D3D11Utils.Convert(descriptor.BufferUsage),
                CpuAccessFlags = D3D11Utils.Convert(descriptor.Usage),
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
