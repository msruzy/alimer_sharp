// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX.Direct3D11;

namespace Vortice.Graphics.D3D11
{
    internal class D3D11CommandBuffer : CommandBuffer
    {
        private readonly Device _nativeDevice;
        private readonly DeviceContext _context;
        private readonly bool _needWorkaround;
        private CommandList _commandList;
        public readonly RenderTargetView[] RenderTargetViews = new RenderTargetView[8];
        public DepthStencilView DepthStencilView;

        public CommandList CommandList => _commandList;

        public D3D11CommandBuffer(D3D11CommandQueue queue, Device nativeDevice)
            : base(queue)
        {
            _nativeDevice = nativeDevice;
            _context = new DeviceContext(nativeDevice);

            // The runtime emulates command lists.
            _needWorkaround = !((D3D11GraphicsDevice)queue.Device).SupportsCommandLists;
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Reset();
            _context.Dispose();
        }

        public void Reset()
        {
            _commandList?.Dispose();
            _commandList = null;
            _context.ClearState();
        }

        protected override void BeginRenderPassCore(RenderPassDescriptor descriptor)
        {
            // Setup color attachments.
            int renderTargetCount = 0;

            for (var i = 0; i < descriptor.ColorAttachments.Length; ++i)
            {
                ref var colorAttachment = ref descriptor.ColorAttachments[i];

                var textureView = (D3D11TextureView)colorAttachment.Attachment;
                RenderTargetViews[i] = textureView.RenderTargetView;

                switch (colorAttachment.LoadAction)
                {
                    case LoadAction.Clear:
                        _context.ClearRenderTargetView(
                            RenderTargetViews[i],
                            D3DConvert.Convert(colorAttachment.ClearColor)
                            );
                        break;

                    default:
                        break;
                }

                renderTargetCount++;
            }

            if (descriptor.DepthStencilAttachment != null)
            {
                var depthStencilAttachment = descriptor.DepthStencilAttachment.Value;

                var d3dTexture = (D3D11Texture)depthStencilAttachment.Texture;
                //DepthStencilView = d3dTexture.GetDepthStencilView(depthStencilAttachment.MipLevel, depthStencilAttachment.Slice);

                switch (depthStencilAttachment.LoadAction)
                {
                    case LoadAction.Clear:
                        _context.ClearDepthStencilView(
                            DepthStencilView,
                            DepthStencilClearFlags.Depth,
                            depthStencilAttachment.ClearDepth,
                            depthStencilAttachment.ClearStencil
                            );
                        break;

                    default:
                        break;
                }
            }

            // Set up render targets
            _context.OutputMerger.SetTargets(DepthStencilView, renderTargetCount, RenderTargetViews);
        }

        protected override void EndRenderPassCore()
        {
            _context.OutputMerger.ResetTargets();
        }

        protected override void CommitCore()
        {
            _commandList = _context.FinishCommandList(false);
            ((D3D11CommandQueue)Queue).Commit(this);
        }
    }
}
