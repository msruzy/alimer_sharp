// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12CommandQueue : CommandQueue
    {
        public readonly SharpDX.Direct3D12.CommandQueue NativeQueue;
        private readonly List<D3D12CommandBuffer> _pendingCommandBuffers = new List<D3D12CommandBuffer>();
        private readonly object _contextLock = new object();

        public D3D12CommandQueue(D3D12GraphicsDevice device, CommandListType type)
            : base(device)
        {
            NativeQueue = device.Device.CreateCommandQueue(new CommandQueueDescription(type, CommandQueueFlags.None));
        }

        public void Destroy()
        {
            NativeQueue.Dispose();
        }

        public override CommandBuffer CreateCommandBuffer()
        {
            return new D3D12CommandBuffer(this);
        }

        public void Commit(D3D12CommandBuffer commandBuffer)
        {
            _pendingCommandBuffers.Add(commandBuffer);
        }

        public void Tick()
        {
            if (_pendingCommandBuffers.Count == 0)
                return;

            _pendingCommandBuffers.Sort((x, y) => x.ExecutionOrder.CompareTo(y.ExecutionOrder));
            var executeCommandLists = new CommandList[_pendingCommandBuffers.Count];

            lock (_contextLock)
            {
                for (var i = 0; i < _pendingCommandBuffers.Count; i++)
                {
                    executeCommandLists[i] = _pendingCommandBuffers[i].CommandList;
                    //_completedBuffers.Add(commandBuffer);
                    //commandBuffer.Reset();
                }
            }

            NativeQueue.ExecuteCommandLists(executeCommandLists.Length, executeCommandLists);
            _pendingCommandBuffers.Clear();
        }
    }
}
