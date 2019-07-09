// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DirectX.Direct3D11;

namespace Vortice.Graphics.D3D11
{
    internal class CommandQueueD3D11 : CommandQueue
    {
        private readonly ID3D11DeviceContext _context;
        private readonly bool _isImmediate;

        public CommandQueueD3D11(DeviceD3D11 device, ID3D11DeviceContext context)
            : base(device, CommandQueueType.Graphics)
        {
            _context = context;
            _isImmediate = context.GetContextType() == DeviceContextType.Immediate;
        }

        public CommandQueueD3D11(DeviceD3D11 device, CommandQueueType queueType)
            : base(device, queueType)
        {
            _context = device.D3D11Device.CreateDeferredContext();
        }

        public void Destroy()
        {
            _context.Dispose();
        }

        protected override CommandBuffer CreateCommandBuffer()
        {
            return new CommandBufferD3D11(this, _context);
        }

        protected override void SubmitImpl(CommandBuffer commandBuffer)
        {
            if (_isImmediate)
            {
            }
            else
            {
               // _commandList = _context.FinishCommandList(false);
            }

            _availableCommandBuffers.Enqueue(commandBuffer);
        }
    }
}
