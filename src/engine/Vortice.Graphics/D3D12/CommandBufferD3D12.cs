// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDirect3D12;
using Vortice.Mathematics;

namespace Vortice.Graphics.D3D12
{
    internal class CommandBufferD3D12 : CommandBuffer
    {
        private readonly CommandListType _type;

        private readonly ID3D12CommandAllocator _commandAllocator;

        public ID3D12GraphicsCommandList CommandList { get; }

        public CommandBufferD3D12(CommandQueueD3D12 queue, CommandListType type)
            : base(queue)
        {
            var d3d12Device = ((DeviceD3D12)queue.Device).D3D12Device;

            _commandAllocator = d3d12Device.CreateCommandAllocator(type);
            CommandList = d3d12Device.CreateCommandList(type, _commandAllocator, null);
            //CommandList.Close();
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            _commandAllocator.Dispose();
            CommandList.Dispose();
        }

        protected override RenderPassCommandEncoder BeginRenderPassCore(in RenderPassDescriptor descriptor)
        {
            return null;
        }

        protected override ComputePassCommandEncoder BeginComputePassCore()
        {
            return null;
        }

        //protected override void SetViewportImpl(Viewport viewport)
        //{
        //    CommandList.RSSetViewport(viewport);
        //}

        //protected override void SetViewportsImpl(Viewport[] viewports, int count)
        //{
        //    CommandList.RSSetViewports(count, viewports);
        //}

        //protected override void SetScissorRectImpl(Rect scissorRect)
        //{
        //    CommandList.RSSetScissorRect(scissorRect);
        //}

        //protected override void SetScissorRectsImpl(Rect[] scissorRects, int count)
        //{
        //    //CommandList.RSSetScissorRects(count, scissorRects);
        //}

        //protected override void CommitCore()
        //{
        //    CommandList.Close();
        //    ((DeviceD3D12)Device).GraphicsQueue.ExecuteCommandList(CommandList);

        //    // 
        //    _currentFrameIndex = (_currentFrameIndex + 1) % _commandAllocators.Length;
        //    _commandAllocators[_currentFrameIndex].Reset();
        //    CommandList.Reset(_commandAllocators[_currentFrameIndex], null);
        //}
    }
}
