// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12CommandBuffer : CommandBuffer
    {
        private CommandList _commandList;

        public CommandList CommandList => _commandList;

        public D3D12CommandBuffer(D3D12GraphicsDevice device)
            : base(device)
        {
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
        }

        protected override void BeginRenderPassCore(RenderPassDescriptor descriptor)
        {
        }

        protected override void EndRenderPassCore()
        {
        }

        protected override void CommitCore()
        {
            //((D3D12CommandQueue)Queue).Commit(this);
        }
    }
}
