// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX.Direct3D11;

namespace Vortice.Graphics.D3D11
{
    internal class CommandBufferD3D11 : CommandBuffer
    {
        private readonly Device _nativeDevice;
        private readonly DeviceContext _context;
        private readonly DeviceContext1 _context1;
        private readonly UserDefinedAnnotation _annotation;
        private readonly bool _isImmediate;
        private readonly bool _needWorkaround;
        private CommandList _commandList;
        public readonly RenderTargetView[] RenderTargetViews = new RenderTargetView[8];

        public CommandList CommandList => _commandList;

        public CommandBufferD3D11(DeviceD3D11 device, DeviceContext context)
            : base(device)
        {
            _nativeDevice = device.D3DDevice;
            _context = context;
            _context1 = _context.QueryInterfaceOrNull<DeviceContext1>();
            _annotation = _context.QueryInterfaceOrNull<UserDefinedAnnotation>();
            _isImmediate = context.TypeInfo == DeviceContextType.Immediate;

            if (!_isImmediate)
            {
                // The runtime emulates command lists.
                _needWorkaround = !device.SupportsCommandLists;
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
                RenderTargetViews[i] = textureView.RenderTargetView;

                switch (attachment.LoadAction)
                {
                    case LoadAction.Clear:
                        _context.ClearRenderTargetView(
                            RenderTargetViews[i],
                            D3DConvert.Convert(attachment.ClearColor)
                            );
                        break;

                    default:
                        break;
                }

                renderTargetCount++;
            }

            DepthStencilView depthStencilView = null;
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
            _context.OutputMerger.SetTargets(depthStencilView, renderTargetCount, RenderTargetViews);
        }

        protected override void EndRenderPassCore()
        {
            _context.OutputMerger.ResetTargets();
        }

        protected override void CommitCore()
        {
            if (_isImmediate)
            {
                _context.Flush();
            }
            else
            {
                _commandList = _context.FinishCommandList(false);
            }
        }
    }
}
