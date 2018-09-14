// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D12;

namespace Vortice.Graphics.DirectX12
{
    internal class DirectX12CommandBuffer : CommandBuffer
    {
        public DirectX12CommandBuffer(DirectX12GraphicsDevice device)
            : base(device)
        {
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
        }

        protected override void BeginRenderPassCore(in RenderPassDescription renderPassDescription)
        {
        }

        protected override void EndRenderPassCore()
        {
        }
    }
}
