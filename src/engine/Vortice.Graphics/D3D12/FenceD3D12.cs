// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using SharpDirect3D12;

namespace Vortice.Graphics.D3D12
{
    internal class FenceD3D12 : IDisposable
    {
        public readonly DeviceD3D12 Device;
        private ID3D12Fence _fence;
        private readonly AutoResetEvent _fenceEvent;

        public FenceD3D12(DeviceD3D12 device, ulong initialValue)
        {
            Device = device;
            _fence = device.D3DDevice.CreateFence(initialValue, FenceFlags.None);
            _fenceEvent = new AutoResetEvent(false);
        }

        public void Dispose()
        {
            Device.DeferredRelease(ref _fence);
            _fenceEvent.Dispose();
        }

        public void Signal(ID3D12CommandQueue queue, ulong fenceValue)
        {
            queue.Signal(_fence, fenceValue);
        }

        public void Wait(ulong fenceValue)
        {
            if (_fence.CompletedValue < fenceValue)
            {
                _fence.SetEventOnCompletion(fenceValue, _fenceEvent);
                _fenceEvent.WaitOne();
            }
        }

        public bool IsSignaled(ulong fenceValue)
        {
            return _fence.CompletedValue >= fenceValue;
        }

        public void Clear(ulong fenceValue)
        {
            _fence.Signal(fenceValue);
        }
    }
}
