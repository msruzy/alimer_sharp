// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DirectX.DXGI;
using Vortice.DirectX.Direct3D12;

namespace Vortice.Graphics.Direct3D12
{
    internal class PipelineStateD3D12 : PipelineState
    {
        private readonly ID3D12RootSignature _rootSignature;
        public readonly ID3D12PipelineState D3D12PipelineState;
        public readonly Vortice.DirectX.Direct3D.PrimitiveTopology PrimitiveTopology;

        public PipelineStateD3D12(GraphicsDeviceD3D12 device, in RenderPipelineDescriptor descriptor)
            : base(device, descriptor)
        {
            var sampleCount = (int)descriptor.Samples;

            var rootSignatureDesc = new VersionedRootSignatureDescription(
                new RootSignatureDescription1(RootSignatureFlags.AllowInputAssemblerInputLayout)
                );

            _rootSignature = device.D3D12Device.CreateRootSignature(0, rootSignatureDesc);

            int inputElementsCount = 0;
            for (int i = 0; i < descriptor.VertexLayouts.Length; i++)
            {
                if (descriptor.VertexLayouts[i].Attributes == null)
                    break;

                inputElementsCount += descriptor.VertexLayouts[i].Attributes.Length;
            }

            var elementIndex = 0;
            var inputElements = new InputElementDescription[inputElementsCount];
            for (int slot = 0; slot < descriptor.VertexLayouts.Length; slot++)
            {
                if (descriptor.VertexLayouts[slot].Attributes == null)
                    break;

                int currentOffset = 0;
                var inputRate = descriptor.VertexLayouts[slot].InputRate;
                foreach (var vertexAttribute in descriptor.VertexLayouts[slot].Attributes)
                {
                    inputElements[elementIndex++] = new InputElementDescription(
                        "ATTRIBUTE",
                        vertexAttribute.Location,
                        vertexAttribute.Format.ToDirectX(),
                        vertexAttribute.Offset != 0 ? vertexAttribute.Offset : currentOffset,
                        slot,
                        inputRate == VertexInputRate.Vertex ? InputClassification.PerVertexData : InputClassification.PerInstanceData,
                        inputRate == VertexInputRate.Instance ? 1 : 0);

                    currentOffset += VertexFormatUtil.GetSizeInBytes(vertexAttribute.Format);
                }
            }

            var psoDesc = new GraphicsPipelineStateDescription()
            {
                RootSignature = _rootSignature,
                VertexShader = ((ShaderD3D12)descriptor.VertexShader).D3D12ShaderBytecode,
                PixelShader = ((ShaderD3D12)descriptor.FragmentShader).D3D12ShaderBytecode,
                InputLayout = new InputLayoutDescription(inputElements),
                SampleMask = uint.MaxValue,
                PrimitiveTopologyType = D3D12Convert.Convert(descriptor.PrimitiveTopology),
                RasterizerState = RasterizerDescription.CullCounterClockwise,
                BlendState = BlendDescription.Opaque,
                DepthStencilState = DepthStencilDescription.None,
                RenderTargetFormats = new[] { Format.R8G8B8A8_UNorm },
                DepthStencilFormat = descriptor.DepthStencilAttachmentFormat.ToDirectX(),
                SampleDescription = new SampleDescription(sampleCount, 0)
            };

            D3D12PipelineState = device.D3D12Device.CreateGraphicsPipelineState(psoDesc);
            PrimitiveTopology = descriptor.PrimitiveTopology.ToDirectX(1);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            _rootSignature.Dispose();
            D3D12PipelineState.Dispose();
        }
    }
}
