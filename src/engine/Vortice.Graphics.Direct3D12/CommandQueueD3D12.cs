// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DirectX.Direct3D12;

namespace Vortice.Graphics.Direct3D12
{
    internal class CommandQueueD3D12 : CommandQueue
    {
        public CommandListType CommandListType { get; }
        public ID3D12CommandQueue D3D12CommandQueue { get; }

        public CommandQueueD3D12(D3D12GraphicsDevice device, CommandQueueType queueType)
            : base(device, queueType)
        {
            switch (queueType)
            {
                case CommandQueueType.Graphics:
                    CommandListType = CommandListType.Direct;
                    break;

                case CommandQueueType.Compute:
                    CommandListType = CommandListType.Compute;
                    break;

                case CommandQueueType.Copy:
                    CommandListType = CommandListType.Copy;
                    break;

                default:
                    CommandListType = CommandListType.Bundle;
                    break;
            }

            D3D12CommandQueue = device.D3D12Device.CreateCommandQueue(CommandListType);
            D3D12CommandQueue.SetName($"{CommandListType} Command Queue");
        }

        public void Destroy()
        {
            D3D12CommandQueue.Dispose();
        }

        protected override CommandBuffer CreateCommandBuffer()
        {
            return new CommandBufferD3D12(this, CommandListType);
        }

        protected override void SubmitImpl(CommandBuffer commandBuffer)
        {
            throw new System.NotImplementedException();
        }
    }
}
