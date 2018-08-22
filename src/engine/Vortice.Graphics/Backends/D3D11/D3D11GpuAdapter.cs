// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using DXGI = SharpDX.DXGI;

namespace Vortice.Graphics.D3D11
{
    internal class D3D11GpuAdapter : GraphicsAdapter
    {
        public readonly DXGI.Adapter1 Adapter;

        public D3D11GpuAdapter(DXGI.Adapter1 adapter)
        {
            Adapter = adapter;

            var desc = adapter.Description1;
            DeviceId = (uint)desc.DeviceId;
            DeviceName = desc.Description;
        }
    }
}
