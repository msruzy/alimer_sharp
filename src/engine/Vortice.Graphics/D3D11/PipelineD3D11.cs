// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDirect3D11;

namespace Vortice.Graphics.D3D11
{
    internal class PipelineD3D11 : Pipeline
    {
        public readonly ID3D11VertexShader D3D11VertexShader;
        public readonly ID3D11PixelShader D3D11PixelShader;
        public readonly ID3D11InputLayout D3D11InputLayout;

        public PipelineD3D11(DeviceD3D11 device, in RenderPipelineDescriptor descriptor)
            : base(device)
        {
            D3D11VertexShader = (ID3D11VertexShader)((ShaderD3D11)descriptor.VertexShader).D3D11Shader;
            D3D11PixelShader = (ID3D11PixelShader)((ShaderD3D11)descriptor.PixelShader).D3D11Shader;

            var vsByteCode = ((ShaderD3D11)descriptor.VertexShader).Bytecode;
            var inputElements = new InputElementDescription[2];
            inputElements[0] = new InputElementDescription("POSITION", 0, SharpDXGI.Format.R32G32B32_Float, 0, 0);
            inputElements[1] = new InputElementDescription("COLOR", 0, SharpDXGI.Format.R32G32B32A32_Float, 12, 0);
            D3D11InputLayout = device.D3D11Device.CreateInputLayout(inputElements, vsByteCode);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            D3D11InputLayout.Dispose();
        }
    }
}
