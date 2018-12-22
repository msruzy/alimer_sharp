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
        private FramebufferD3D11 _currentFramebuffer;

        public CommandList CommandList => _commandList;

        public CommandBufferD3D11(GPUDeviceD3D11 device, DeviceContext context)
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
            _currentFramebuffer = null;

            _commandList?.Dispose();
            _commandList = null;
            _context.ClearState();
        }

        internal override void BeginRenderPassCore(GPUFramebuffer framebuffer, in RenderPassBeginDescriptor descriptor)
        {
            // Setup color attachments.
            _currentFramebuffer = (FramebufferD3D11)framebuffer;
            _context.OutputMerger.SetTargets(_currentFramebuffer.DepthStencilView, _currentFramebuffer.RenderTargetViews);

            int renderTargetCount = 0;

            /*for (var i = 0; i < descriptor.ColorAttachments.Length; ++i)
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
            _context.OutputMerger.SetTargets(DepthStencilView, renderTargetCount, RenderTargetViews);*/
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
