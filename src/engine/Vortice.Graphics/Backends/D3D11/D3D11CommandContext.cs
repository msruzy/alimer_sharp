// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX.Direct3D11;
using Vortice.Graphics.D3D;

namespace Vortice.Graphics.D3D11
{
    internal class D3D11CommandContext : CommandContext
    {
        private readonly SharpDX.Direct3D11.DeviceContext1 _context;
        public readonly RenderTargetView[] RenderTargetViews;
        public readonly DepthStencilView DepthStencilView;

        public D3D11CommandContext(D3D11GraphicsDevice device)
            : base(device)
        {
            _context = device.ImmediateContext;
            RenderTargetViews = new RenderTargetView[8];
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

                var d3dTexture = (D3D11Texture)colorAttachment.Texture;
                RenderTargetViews[i] = d3dTexture.GetRenderTargetView(colorAttachment.MipLevel, colorAttachment.Slice);

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

            if (renderPassDescription.DepthStencilAttachment != null)
            {
                var depthStencilAttachment = renderPassDescription.DepthStencilAttachment.Value;

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
    }
}
