// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12CommandQueue : CommandQueue
    {
        public readonly SharpDX.Direct3D12.CommandQueue NativeQueue;

        public D3D12CommandQueue(D3D12GraphicsDevice device, CommandListType type)
            : base(device)
        {
            NativeQueue = device.NativeDevice.CreateCommandQueue(new CommandQueueDescription(type, CommandQueueFlags.None));
        }

        public void Destroy()
        {
            NativeQueue.Dispose();
        }

        public override CommandBuffer CreateCommandBuffer()
        {
            throw new System.NotImplementedException();
        }

        public override void Submit(params CommandBuffer[] buffers)
        {
            var commandLists = new CommandList[buffers.Length];
            for (var i = 0; i < buffers.Length; i++)
            {
                commandLists[i] = ((D3D12CommandBuffer)buffers[i]).CommandList;
            }

            NativeQueue.ExecuteCommandLists(buffers.Length, commandLists);
        }
    }
}
