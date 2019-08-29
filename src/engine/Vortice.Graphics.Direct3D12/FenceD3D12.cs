﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Vortice.Direct3D12;

namespace Vortice.Graphics.Direct3D12
{
    internal class FenceD3D12 : IDisposable
    {
        public readonly D3D12GraphicsDevice Device;
        private ID3D12Fence _fence;
        private readonly AutoResetEvent _fenceEvent;

        public FenceD3D12(D3D12GraphicsDevice device, long initialValue)
        {
            Device = device;
            _fence = device.D3D12Device.CreateFence(initialValue, FenceFlags.None);
            _fenceEvent = new AutoResetEvent(false);
        }

        public void Dispose()
        {
            Device.DeferredRelease(ref _fence);
            _fenceEvent.Dispose();
        }

        public void Signal(ID3D12CommandQueue queue, long fenceValue)
        {
            queue.Signal(_fence, fenceValue);
        }

        public void Wait(long fenceValue)
        {
            if (_fence.CompletedValue < fenceValue)
            {
                _fence.SetEventOnCompletion(fenceValue, _fenceEvent);
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
