// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace Vortice.Graphics.Direct3D11
{
    internal class SamplerD3D11 : Sampler
    {
        public readonly ID3D11SamplerState NativeSamplerState;

        public SamplerD3D11(DeviceD3D11 device, in SamplerDescriptor descriptor)
           : base(device, descriptor)
        {
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            NativeSamplerState.Dispose();
        }
    }
}
