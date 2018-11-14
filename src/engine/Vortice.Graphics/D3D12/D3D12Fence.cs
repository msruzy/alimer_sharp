// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12Fence : IDisposable
    {
        public readonly D3D12GraphicsDevice Device;
        private Fence _fence;
        private readonly AutoResetEvent _fenceEvent;

        public D3D12Fence(D3D12GraphicsDevice device, long initialValue)
        {
            Device = device;
            _fence = device.Device.CreateFence(initialValue, FenceFlags.None);
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
