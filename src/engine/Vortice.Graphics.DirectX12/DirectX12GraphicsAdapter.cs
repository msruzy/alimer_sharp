// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.DXGI;

namespace Vortice.Graphics.DirectX12
{
    internal class DirectX12GraphicsAdapter : GraphicsAdapter
    {
        public readonly Adapter1 Adapter;

        public DirectX12GraphicsAdapter(Adapter1 adapter)
        {
            Adapter = adapter;

            var desc = adapter.Description1;
            DeviceId = (uint)desc.DeviceId;
            DeviceName = desc.Description;
        }
    }
}
