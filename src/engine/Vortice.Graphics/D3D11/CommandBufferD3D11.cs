// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using Vortice.DirectX.Direct3D11;
using Vortice.Mathematics;

namespace Vortice.Graphics.D3D11
{
    internal class CommandBufferD3D11 : CommandBuffer
    {
        private static readonly ID3D11RenderTargetView[] _nullRTVViews = new ID3D11RenderTargetView[BlendDescription.SimultaneousRenderTargetCount];

        public readonly ID3D11DeviceContext D3D11Context;
        private readonly ID3D11DeviceContext1 _context1;
        private readonly ID3DUserDefinedAnnotation _annotation;
        private readonly bool _isImmediate;
        private readonly bool _needWorkaround;
        private ID3D11CommandList _commandList;

        // Bound states
        private readonly ID3D11RenderTargetView[] _rtvViews = new ID3D11RenderTargetView[BlendDescription.SimultaneousRenderTargetCount];
        private VertexBufferView[] _vertexBufferBindings = new VertexBufferView[32];

        private ID3D11BlendState _boundBlendState;
        private Color4 _boundBlendColor;
        private ID3D11DepthStencilState _boundDepthStencilState;
        private int _boundStencilReference;
        private ID3D11RasterizerState _boundRasterizerState;
        private Vortice.DirectX.Direct3D.PrimitiveTopology _boundPrimitiveTopology;
        private ID3D11InputLayout _boundInputLayout;
        private ID3D11VertexShader _boundVertexShader;
        private ID3D11GeometryShader _boundGeometryShader;
        private ID3D11HullShader _boundHullShader;
        private ID3D11DomainShader _boundDomainShader;
        private ID3D11PixelShader _boundPixelShader;

        public ID3D11CommandList CommandList => _commandList;

        public CommandBufferD3D11(CommandQueueD3D11 commandQueue, ID3D11DeviceContext context)
            : base(commandQueue)
        {
            D3D11Context = context;
            _context1 = context.QueryInterfaceOrNull<ID3D11DeviceContext1>();
            _annotation = context.QueryInterfaceOrNull<ID3DUserDefinedAnnotation>();
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
            D3D11Context.Dispose();
        }

        public void Reset()
        {
            _commandList?.Dispose();
            _commandList = null;
            D3D11Context.ClearState();
        }

        protected override void SetPipelineStateImpl(PipelineState pipelineState)
        {
            var pipelineStateD3D11 = (PipelineStateD3D11)pipelineState;

            if (pipelineState.IsCompute)
            {
                var computeShader = pipelineStateD3D11.ComputeShader;
                D3D11Context.CSSetShader(computeShader);
            }
            else
            {
                var blendState = pipelineStateD3D11.BlendState;
                if (_boundBlendState != blendState)
                {
                    _boundBlendState = blendState;
                    D3D11Context.OMSetBlendState(blendState, _boundBlendColor);
                }

                var depthStencilState = pipelineStateD3D11.DepthStencilState;
                if (_boundDepthStencilState != depthStencilState)
                {
                    _boundDepthStencilState = depthStencilState;
                    D3D11Context.OMSetDepthStencilState(depthStencilState, _boundStencilReference);
                }

                var rasterizerState = pipelineStateD3D11.RasterizerState;
                if (_boundRasterizerState != rasterizerState)
                {
                    _boundRasterizerState = rasterizerState;
                    D3D11Context.RSSetState(rasterizerState);
                }

                var primitiveTopology = pipelineStateD3D11.PrimitiveTopology;
                if (_boundPrimitiveTopology != primitiveTopology)
                {
                    _boundPrimitiveTopology = primitiveTopology;
                    D3D11Context.IASetPrimitiveTopology(primitiveTopology);
                }

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

                var geometryShader = pipelineStateD3D11.GeometryShader;
                if (_boundGeometryShader != geometryShader)
                {
                    _boundGeometryShader = geometryShader;
                    D3D11Context.GSSetShader(geometryShader);
                }

                var hullShader = pipelineStateD3D11.HullShader;
                if (_boundHullShader != hullShader)
                {
                    _boundHullShader = hullShader;
                    D3D11Context.HSSetShader(hullShader);
                }

                var domainShader = pipelineStateD3D11.DomainShader;
                if (_boundDomainShader != domainShader)
                {
                    _boundDomainShader = domainShader;
                    D3D11Context.DSSetShader(domainShader);
                }

                var pixelShader = pipelineStateD3D11.PixelShader;
                if (_boundPixelShader != pixelShader)
                {
                    _boundPixelShader = pixelShader;
                    D3D11Context.PSSetShader(pixelShader);
                }
            }
        }

        protected override void BeginRenderPassImpl(in RenderPassDescriptor descriptor)
        {
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
            SetScissorRect(new Rectangle(0, 0, width, height));
            _boundBlendColor = default;
        }

        protected override void EndRenderPassImpl()
        {
            D3D11Context.OMSetRenderTargets(8, _nullRTVViews, null);
        }

        /// <inheritdoc/>
        protected override void SetVertexBufferImpl(GraphicsBuffer buffer, int offset, int index)
        {
            var view = new VertexBufferView(((BufferD3D11)buffer).Resource, 28, offset);
            D3D11Context.IASetVertexBuffers(index, view);
        }

        /// <inheritdoc/>
        public override void SetBlendColor(ref Color4 blendColor)
        {
            _boundBlendColor = blendColor;
        }

        /// <inheritdoc/>
        public override void SetStencilReference(int reference)
        {
            _boundStencilReference = reference;
        }

        /// <inheritdoc/>
        public override void SetViewport(ref Viewport viewport)
        {
            D3D11Context.RSSetViewport(viewport);
        }

        public override void SetScissorRect(ref Rectangle scissorRect)
        {
            D3D11Context.RSSetScissorRect(scissorRect);
        }

        /// <inheritdoc/>
        protected override void DrawImpl(int vertexCount, int instanceCount, int firstVertex, int firstInstance)
        {
            if (instanceCount <= 1)
            {
                D3D11Context.Draw(vertexCount, firstVertex);
            }
            else
            {
                D3D11Context.DrawInstanced(vertexCount, instanceCount, firstVertex, firstInstance);
            }
        }

        /// <inheritdoc/>
        protected override void DispatchCore(int groupCountX, int groupCountY, int groupCountZ)
        {
            D3D11Context.Dispatch(groupCountX, groupCountY, groupCountZ);
        }

        protected override void SetConstantBufferImpl(ShaderStages stages, int index, GraphicsBuffer buffer)
        {
            var d3d11Buffer = ((BufferD3D11)buffer).Resource;
            if ((stages & ShaderStages.Vertex) != ShaderStages.None)
            {
                D3D11Context.VSSetConstantBuffer(index, d3d11Buffer);
            }

            if ((stages & ShaderStages.Pixel) != ShaderStages.None)
            {
                D3D11Context.PSSetConstantBuffer(index, d3d11Buffer);
            }
        }

        public void ResetState()
        {
            _boundBlendState = null;
            _boundBlendColor = default;
            _boundDepthStencilState = null;
            _boundStencilReference = 0;
            _boundRasterizerState = null;
            _boundPrimitiveTopology = Vortice.DirectX.Direct3D.PrimitiveTopology.Undefined;
            _boundInputLayout = null;
            _boundVertexShader = null;
            _boundGeometryShader = null;
            _boundHullShader = null;
            _boundDomainShader = null;
            _boundPixelShader = null;
        }
    }
}
