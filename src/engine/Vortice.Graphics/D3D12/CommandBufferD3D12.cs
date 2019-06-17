// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Drawing;
using Vortice.DirectX.Direct3D12;
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
            _type = type;
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            _commandAllocator.Dispose();
            CommandList.Dispose();
        }

        public override void SetViewport(ref Viewport viewport)
        {
            CommandList.RSSetViewport(viewport);
        }

        public override void SetScissorRect(ref Rectangle scissorRect)
        {
            CommandList.RSSetScissorRect(scissorRect);
        }

        public override void SetStencilReference(int reference)
        {
            CommandList.OMSetStencilRef(reference);
        }

        public override void SetBlendColor(ref Color4 blendColor)
        {
            CommandList.OMSetBlendFactor(blendColor);
        }

        protected override void DrawImpl(int vertexCount, int instanceCount, int firstVertex, int firstInstance)
        {
            throw new System.NotImplementedException();
        }

        //protected override void CommitCore()
        //{
        //    CommandList.Close();
        //    ((DeviceD3D12)Device).GraphicsQueue.ExecuteCommandList(CommandList);

        //    // 
        //    _currentFrameIndex = (_currentFrameIndex + 1) % _commandAllocators.Length;
        //    _commandAllocators[_currentFrameIndex].Reset();
        //    CommandList.Reset(_commandAllocators[_currentFrameIndex], null);
        //}

        protected override void SetPipelineStateImpl(PipelineState pipelineState)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetVertexBufferImpl(GraphicsBuffer buffer, int offset, int index)
        {
            throw new System.NotImplementedException();
        }

        protected override void BeginRenderPassImpl(in RenderPassDescriptor descriptor)
        {
            throw new System.NotImplementedException();
        }

        protected override void EndRenderPassImpl()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        protected override void DispatchCore(int groupCountX, int groupCountY, int groupCountZ)
        {
            CommandList.Dispatch(groupCountX, groupCountY, groupCountZ);
        }

        protected override void SetConstantBufferImpl(ShaderStages stages, int index, GraphicsBuffer buffer)
        {
        }
    }
}
