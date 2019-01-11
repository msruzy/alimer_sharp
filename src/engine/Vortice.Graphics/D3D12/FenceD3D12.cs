// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class FenceD3D12 : IDisposable
    {
        public readonly DeviceD3D12 Device;
        private Fence _fence;
        private readonly AutoResetEvent _fenceEvent;

        public FenceD3D12(DeviceD3D12 device, long initialValue)
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

        public void Signal(CommandQueue queue, long fenceValue)
        {
            queue.Signal(_fence, fenceValue);
        }

        public void Wait(long fenceValue)
        {
            if (_fence.CompletedValue < fenceValue)
            {
                _fence.SetEventOnCompletion(fenceValue, _fenceEvent.SafeWaitHandle.DangerousGetHandle());
                _fenceEvent.WaitOne();
            }
        }

        public bool IsSignaled(long fenceValue)
        {
            return _fence.CompletedValue >= fenceValue;
        }

        public void Clear(long fenceValue)
        {
            _fence.Signal(fenceValue);
        }
    }
}
