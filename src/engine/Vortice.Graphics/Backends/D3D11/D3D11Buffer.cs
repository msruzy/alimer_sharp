// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX.Direct3D11;

namespace Vortice.Graphics.D3D11
{
    internal class D3D11Buffer : GraphicsBuffer
    {
        public readonly SharpDX.Direct3D11.Buffer Resource;

        public D3D11Buffer(D3D11GraphicsDevice device, BufferUsage usage, int sizeInBytes, IntPtr data)
            : base(device, usage)
        {
            var isDynamic = false;
            var description = new BufferDescription()
            {
                BindFlags = D3D11Convert.Convert(usage),
                CpuAccessFlags = isDynamic ? CpuAccessFlags.Write : CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = sizeInBytes,
                Usage = ResourceUsage.Immutable,
                StructureByteStride = 0
            };

            if ((usage & BufferUsage.Indirect) != 0)
            {
                description.OptionFlags |= ResourceOptionFlags.DrawIndirectArguments;
            }

            Resource = new SharpDX.Direct3D11.Buffer(device.Device, data, description);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Resource.Dispose();
        }
    }
}
