// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12CommandBuffer : CommandBuffer
    {
        private readonly int _frameCount;
        private readonly CommandListType _type;
        private int _currentFrameIndex;

        private readonly CommandAllocator[] _commandAllocators;
        private readonly GraphicsCommandList _commandList;

        public GraphicsCommandList CommandList => _commandList;

        public D3D12CommandBuffer(D3D12GraphicsDevice device, int frameCount, CommandListType type)
            : base(device)
        {
            _frameCount = frameCount;
            _type = type;

            _commandAllocators = new CommandAllocator[frameCount];
            for (var i = 0; i < frameCount; ++i)
            {
                _commandAllocators[i] = device.D3DDevice.CreateCommandAllocator(type);
            }

            _commandList = device.D3DDevice.CreateCommandList(type, _commandAllocators[_currentFrameIndex], null);
            _commandList.Close();
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            for (var i = 0; i < _frameCount; ++i)
            {
                _commandAllocators[i].Dispose();
            }

            _commandList.Dispose();
        }

        protected override void BeginRenderPassCore(RenderPassDescriptor descriptor)
        {
        }

        protected override void EndRenderPassCore()
        {
        }

        protected override void CommitCore()
        {
            _commandList.Close();
            ((D3D12GraphicsDevice)Device).GraphicsQueue.ExecuteCommandList(_commandList);

            // 
            _currentFrameIndex = (_currentFrameIndex + 1) % _commandAllocators.Length;
            _commandAllocators[_currentFrameIndex].Reset();
            _commandList.Reset(_commandAllocators[_currentFrameIndex], null);
        }
    }
}
