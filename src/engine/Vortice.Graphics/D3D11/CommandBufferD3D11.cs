// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDirect3D11;
using Vortice.Mathematics;

namespace Vortice.Graphics.D3D11
{
    internal class CommandBufferD3D11 : CommandBuffer
    {
        private static readonly ID3D11RenderTargetView[] _nullRTVViews = new ID3D11RenderTargetView[BlendDescription.SimultaneousRenderTargetCount];
        private readonly ID3D11DeviceContext _context;
        private readonly ID3D11DeviceContext1 _context1;
        private readonly ID3DUserDefinedAnnotation _annotation;
        private readonly bool _isImmediate;
        private readonly bool _needWorkaround;
        private ID3D11CommandList _commandList;
        private readonly ID3D11RenderTargetView[] _rtvViews = new ID3D11RenderTargetView[BlendDescription.SimultaneousRenderTargetCount];

        public ID3D11CommandList CommandList => _commandList;

        public CommandBufferD3D11(CommandQueueD3D11 commandQueue, ID3D11DeviceContext context)
            : base(commandQueue)
        {
            _context = context;
            _context1 = _context.QueryInterfaceOrNull<ID3D11DeviceContext1>();
            _annotation = _context.QueryInterfaceOrNull<ID3DUserDefinedAnnotation>();
            _isImmediate = context.GetContextType() == DeviceContextType.Immediate;

            if (!_isImmediate)
            {
                // The runtime emulates command lists.
                _needWorkaround = !((DeviceD3D11)commandQueue.Device).SupportsCommandLists;
            }
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Reset();

            _annotation?.Dispose();
            _context1?.Dispose();
            _context.Dispose();
        }

        public void Reset()
        {
            _commandList?.Dispose();
            _commandList = null;
            _context.ClearState();
        }

        internal override void BeginRenderPassCore(in RenderPassDescriptor descriptor)
        {
            // Setup color attachments.
            int renderTargetCount = 0;

            for (var i = 0; i < descriptor.ColorAttachments.Length; ++i)
            {
                ref var attachment = ref descriptor.ColorAttachments[i];

                var texture = (TextureD3D11)attachment.Texture;
                var textureView = texture.GetView(attachment.Level, 1, attachment.Slice);
                _rtvViews[i] = textureView.RenderTargetView;

                switch (attachment.LoadAction)
                {
                    case LoadAction.Clear:
                        _context.ClearRenderTargetView(_rtvViews[i], attachment.ClearColor);
                        break;

                    default:
                        break;
                }

                renderTargetCount++;
            }

            ID3D11DepthStencilView depthStencilView = null;
            if (descriptor.DepthStencilAttachment.HasValue
                && descriptor.DepthStencilAttachment.Value.Texture != null)
            {
                var attachment = descriptor.DepthStencilAttachment.Value;

                var texture = (TextureD3D11)attachment.Texture;
                var textureView = texture.GetView(attachment.Level, 1, attachment.Slice);
                depthStencilView = textureView.DepthStencilView;

                DepthStencilClearFlags depthStencilClearFlags = 0;
                if (attachment.DepthLoadAction == LoadAction.Clear)
                {
                    depthStencilClearFlags |= DepthStencilClearFlags.Depth;
                }

                if (attachment.StencilLoadAction == LoadAction.Clear)
                {
                    depthStencilClearFlags |= DepthStencilClearFlags.Stencil;
                }

                if (depthStencilClearFlags != 0)
                {
                    _context.ClearDepthStencilView(
                        depthStencilView,
                        depthStencilClearFlags,
                        attachment.ClearDepth,
                        attachment.ClearStencil
                        );
                }
            }

            // Set up render targets
            _context.OMSetRenderTargets(renderTargetCount, _rtvViews, depthStencilView);
        }

        protected override void EndRenderPassCore()
        {
            _context.OMSetRenderTargets(8, _nullRTVViews, null);
        }

        protected override void SetViewportImpl(Viewport viewport)
        {
            _context.RSSetViewport(viewport);
        }

        protected override void SetViewportsImpl(Viewport[] viewports, int count)
        {
            _context.RSSetViewports(count, viewports);
        }

        protected override void SetScissorRectImpl(RectI scissorRect)
        {
            _context.RSSetScissorRect(scissorRect);
        }

        protected override void SetScissorRectsImpl(RectI[] scissorRects, int count)
        {
            _context.RSSetScissorRects(count, scissorRects);
        }
    }
}
