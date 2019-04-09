// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDirect3D12;
using Vortice.Mathematics;

namespace Vortice.Graphics.D3D12
{
    internal class CommandBufferD3D12 : CommandBuffer
    {
        private readonly int _frameCount;
        private readonly CommandListType _type;
        private int _currentFrameIndex;

        private readonly ID3D12CommandAllocator[] _commandAllocators;

        public ID3D12GraphicsCommandList CommandList { get; }

        public CommandBufferD3D12(DeviceD3D12 device, int frameCount, CommandListType type)
            : base(null)
        {
            _frameCount = frameCount;
            _type = type;

            _commandAllocators = new ID3D12CommandAllocator[frameCount];
            for (var i = 0; i < frameCount; ++i)
            {
                _commandAllocators[i] = device.D3D12Device.CreateCommandAllocator(type);
            }

            CommandList = device.D3D12Device.CreateCommandList(type, _commandAllocators[_currentFrameIndex], null);
            CommandList.Close();
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            for (var i = 0; i < _frameCount; ++i)
            {
                _commandAllocators[i].Dispose();
            }

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
