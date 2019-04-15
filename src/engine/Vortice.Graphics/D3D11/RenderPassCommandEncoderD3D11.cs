// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDirect3D11;
using SharpDXGI;
using SharpDXGI.Direct3D;
using Vortice.Mathematics;

namespace Vortice.Graphics.D3D11
{
    internal class RenderPassCommandEncoderD3D11 : RenderPassCommandEncoder
    {
        private static readonly ID3D11RenderTargetView[] _nullRTVViews = new ID3D11RenderTargetView[BlendDescription.SimultaneousRenderTargetCount];
        public readonly ID3D11DeviceContext D3D11Context;
        private readonly ID3D11RenderTargetView[] _rtvViews = new ID3D11RenderTargetView[BlendDescription.SimultaneousRenderTargetCount];
        private Color4 _blendColor;
        private ID3D11InputLayout _boundInputLayout;
        private ID3D11VertexShader _boundVertexShader;
        private ID3D11GeometryShader _boundGeometryShader;
        private ID3D11HullShader _boundHullShader;
        private ID3D11DomainShader _boundDomainShader;
        private ID3D11PixelShader _boundPixelShader;
        private ID3D11DepthStencilState _boundDepthStencilState;

        public RenderPassCommandEncoderD3D11(CommandBufferD3D11 commandBuffer)
            : base(commandBuffer)
        {
            D3D11Context = commandBuffer.D3D11Context;
        }

        public void Begin(in RenderPassDescriptor descriptor)
        {
            _commandBuffer.IsEncodingPass = true;

            // Setup color attachments.
            int renderTargetCount = 0;
            var width = int.MaxValue;
            var height = int.MaxValue;

            for (var i = 0; i < descriptor.ColorAttachments.Length; ++i)
            {
                ref var attachment = ref descriptor.ColorAttachments[i];

                var texture = (TextureD3D11)attachment.Texture;
                var textureView = texture.GetView(attachment.Level, 1, attachment.Slice);
                _rtvViews[i] = textureView.RenderTargetView;

                switch (attachment.LoadAction)
                {
                    case LoadAction.Clear:
                        D3D11Context.ClearRenderTargetView(_rtvViews[i], attachment.ClearColor);
                        break;

                    default:
                        break;
                }

                width = Math.Min(width, texture.GetLevelWidth(attachment.Level));
                height = Math.Min(height, texture.GetLevelHeight(attachment.Level));

                renderTargetCount++;
            }

            ID3D11DepthStencilView depthStencilView = null;
            if (descriptor.DepthStencilAttachment.HasValue
                && descriptor.DepthStencilAttachment.Value.Texture != null)
            {
                var attachment = descriptor.DepthStencilAttachment.Value;

                var texture = (TextureD3D11)attachment.Texture;
                width = Math.Min(width, texture.GetLevelWidth(attachment.Level));
                height = Math.Min(height, texture.GetLevelHeight(attachment.Level));

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
                    D3D11Context.ClearDepthStencilView(
                        depthStencilView,
                        depthStencilClearFlags,
                        attachment.ClearDepth,
                        attachment.ClearStencil
                        );
                }
            }

            // Set up render targets
            D3D11Context.OMSetRenderTargets(renderTargetCount, _rtvViews, depthStencilView);

            // Apply viewport and scissor for render target.
            SetViewport(new Viewport(width, height));
            SetScissorRect(new Rect(width, height));
            _blendColor = default;
        }

        protected override void EndPassImpl()
        {
            D3D11Context.OMSetRenderTargets(8, _nullRTVViews, null);
        }

        protected override void SetPipelineStateImpl(RenderPipelineState pipelineState)
        {
            var pipelineStateD3D11 = (RenderPipelineStateD3D11)pipelineState;
            var inputLayout = pipelineStateD3D11.InputLayout;
            if (_boundInputLayout != inputLayout)
            {
                _boundInputLayout = inputLayout;
                D3D11Context.IASetInputLayout(_boundInputLayout);
            }

            var vertexShader = pipelineStateD3D11.VertexShader;
            if (_boundVertexShader != vertexShader)
            {
                _boundVertexShader = vertexShader;
                D3D11Context.VSSetShader(vertexShader);
            }

            var pixelShader = pipelineStateD3D11.PixelShader;
            if (_boundPixelShader != pixelShader)
            {
                _boundPixelShader = pixelShader;
                D3D11Context.PSSetShader(pixelShader);
            }

            var depthStencilState = pipelineStateD3D11.DepthStencilState;
            if (_boundDepthStencilState != depthStencilState)
            {
                _boundDepthStencilState = depthStencilState;
                D3D11Context.OMSetDepthStencilState(depthStencilState);
            }

            var rasterizerState = pipelineStateD3D11.RasterizerState;
            D3D11Context.RSSetState(rasterizerState);

            var blendState = pipelineStateD3D11.BlendState;
            D3D11Context.OMSetBlendState(blendState);
        }

        /// <inheritdoc/>
        public override void SetBlendColor(ref Color4 blendColor)
        {
            _blendColor = blendColor;
        }

        /// <inheritdoc/>
        public override void SetViewport(ref Viewport viewport)
        {
            D3D11Context.RSSetViewport(viewport);
        }

        public override void SetScissorRect(ref Rect scissorRect)
        {
            D3D11Context.RSSetScissorRect(scissorRect);
        }

        protected override void SetVertexBufferImpl(int slot, GraphicsBuffer buffer)
        {
            var view = new VertexBufferView(((BufferD3D11)buffer).Resource, 28);
            D3D11Context.IASetVertexBuffers(0, view);
        }

        protected override void DrawImpl(int vertexCount, int instanceCount, int firstVertex, int firstInstance)
        {
            PrepareDraw();
            D3D11Context.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);
            if (instanceCount <= 1)
            {
                D3D11Context.Draw(vertexCount, firstVertex);
            }
            else
            {
                D3D11Context.DrawInstanced(vertexCount, instanceCount, firstVertex, firstInstance);
            }
        }

        private void PrepareDraw()
        {
        }
    }
}
