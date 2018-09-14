// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX.Direct3D11;

namespace Vortice.Graphics.DirectX11
{
    internal sealed class DirectX11CommandBuffer : CommandBuffer
    {
        private readonly DeviceContext1 _context;
        private readonly bool _immediate;
        private readonly bool _needWorkaround;
        public readonly RenderTargetView[] RenderTargetViews = new RenderTargetView[8];
        public DepthStencilView DepthStencilView = null;

        public DirectX11CommandBuffer(DirectX11GraphicsDevice device, DeviceContext1 context)
            : base(device)
        {
            Guard.NotNull(context, nameof(context));

            _immediate = context.TypeInfo == DeviceContextType.Immediate;
            _context = context;

            if (context.TypeInfo == DeviceContextType.Deferred 
                && device.Device.CheckThreadingSupport(out var supportsConcurrentResources, out var supportsCommandLists).Success)
            {
                // The runtime emulates command lists.
                _needWorkaround = !supportsCommandLists;
            }

            Reset();
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            _context.Dispose();
        }

        protected override void BeginRenderPassCore(in RenderPassDescription renderPassDescription)
        {
            // Setup color attachments.
            int renderTargetCount = 0;

            for (var i = 0; i < renderPassDescription.ColorAttachments.Length; ++i)
            {
                ref RenderPassColorAttachment colorAttachment = ref renderPassDescription.ColorAttachments[i];

                var d3dTexture = (DirectX11Texture)colorAttachment.Texture;
                RenderTargetViews[i] = d3dTexture.GetRenderTargetView(colorAttachment.MipLevel, colorAttachment.Slice);

                switch (colorAttachment.LoadAction)
                {
                    case LoadAction.Clear:
                        _context.ClearRenderTargetView(
                            RenderTargetViews[i],
                            DirectX11Utils.Convert(colorAttachment.ClearColor)
                            );
                        break;

                    default:
                        break;
                }

                renderTargetCount++;
            }

            if (renderPassDescription.DepthStencilAttachment != null)
            {
                var depthStencilAttachment = renderPassDescription.DepthStencilAttachment.Value;

                var d3dTexture = (DirectX11Texture)depthStencilAttachment.Texture;
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

        private void Reset()
        {

        }
    }
}
