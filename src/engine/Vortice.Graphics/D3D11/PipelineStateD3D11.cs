// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DirectX.Direct3D11;

namespace Vortice.Graphics.D3D11
{
    internal class PipelineStateD3D11 : PipelineState
    {
        public readonly ID3D11VertexShader VertexShader;
        public readonly ID3D11GeometryShader GeometryShader;
        public readonly ID3D11HullShader HullShader;
        public readonly ID3D11DomainShader DomainShader;
        public readonly ID3D11PixelShader PixelShader;
        public readonly ID3D11InputLayout InputLayout;
        public readonly ID3D11RasterizerState RasterizerState;
        public readonly ID3D11DepthStencilState DepthStencilState;
        public readonly ID3D11BlendState BlendState;
        public readonly Vortice.DirectX.Direct3D.PrimitiveTopology PrimitiveTopology;

        public readonly ID3D11ComputeShader ComputeShader;

        public PipelineStateD3D11(DeviceD3D11 device, in RenderPipelineDescriptor descriptor)
            : base(device, descriptor)
        {
            VertexShader = (ID3D11VertexShader)((ShaderD3D11)descriptor.VertexShader).D3D11Shader;
            PixelShader = (ID3D11PixelShader)((ShaderD3D11)descriptor.FragmentShader).D3D11Shader;

            var vsByteCode = ((ShaderD3D11)descriptor.VertexShader).Bytecode;

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
                        D3DConvert.ConvertVertexFormat(vertexAttribute.Format),
                        vertexAttribute.Offset != 0 ? vertexAttribute.Offset : currentOffset,
                        slot,
                        inputRate == VertexInputRate.Vertex ? InputClassification.PerVertexData : InputClassification.PerInstanceData,
                        inputRate == VertexInputRate.Instance ? 1 : 0);

                    currentOffset += VertexFormatUtil.GetSizeInBytes(vertexAttribute.Format);
                }
            }

            InputLayout = device.D3D11Device.CreateInputLayout(inputElements, vsByteCode);

            RasterizerState = device.D3D11Device.CreateRasterizerState(RasterizerDescription.CullCounterClockwise);
            DepthStencilState = device.D3D11Device.CreateDepthStencilState(DepthStencilDescription.Default);
            BlendState = device.D3D11Device.CreateBlendState(BlendDescription.Opaque);
            PrimitiveTopology = D3DConvert.Convert(descriptor.PrimitiveTopology, 1);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            InputLayout.Dispose();
            RasterizerState.Dispose();
            DepthStencilState.Dispose();
            BlendState.Dispose();
        }
    }
}
