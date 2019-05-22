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
            PixelShader = (ID3D11PixelShader)((ShaderD3D11)descriptor.PixelShader).D3D11Shader;

            var vsByteCode = ((ShaderD3D11)descriptor.VertexShader).Bytecode;
            var inputElements = new InputElementDescription[2];
            inputElements[0] = new InputElementDescription("POSITION", 0, Vortice.DirectX.DXGI.Format.R32G32B32_Float, 0, 0);
            inputElements[1] = new InputElementDescription("COLOR", 0, Vortice.DirectX.DXGI.Format.R32G32B32A32_Float, 12, 0);
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
