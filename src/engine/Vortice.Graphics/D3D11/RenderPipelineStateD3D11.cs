// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDirect3D11;

namespace Vortice.Graphics.D3D11
{
    internal class RenderPipelineStateD3D11 : RenderPipelineState
    {
        public readonly ID3D11VertexShader VertexShader;
        public readonly ID3D11PixelShader PixelShader;
        public readonly ID3D11InputLayout InputLayout;
        public readonly ID3D11RasterizerState RasterizerState;
        public readonly ID3D11DepthStencilState DepthStencilState;

        public RenderPipelineStateD3D11(DeviceD3D11 device, in RenderPipelineDescriptor descriptor)
            : base(device)
        {
            VertexShader = (ID3D11VertexShader)((ShaderD3D11)descriptor.VertexShader).D3D11Shader;
            PixelShader = (ID3D11PixelShader)((ShaderD3D11)descriptor.PixelShader).D3D11Shader;

            var vsByteCode = ((ShaderD3D11)descriptor.VertexShader).Bytecode;
            var inputElements = new InputElementDescription[2];
            inputElements[0] = new InputElementDescription("POSITION", 0, SharpDXGI.Format.R32G32B32_Float, 0, 0);
            inputElements[1] = new InputElementDescription("COLOR", 0, SharpDXGI.Format.R32G32B32A32_Float, 12, 0);
            InputLayout = device.D3D11Device.CreateInputLayout(inputElements, vsByteCode);

            RasterizerState = device.D3D11Device.CreateRasterizerState(new RasterizerDescription
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid,
                ScissorEnable = false
            });

            var defaultDepthStencilOp = new DepthStencilopDescription
            {
                StencilFailOp = StencilOp.Keep,
                StencilDepthFailOp = StencilOp.Keep,
                StencilPassOp = StencilOp.Keep,
                StencilFunc = ComparisonFunc.Always
            };

            DepthStencilState = device.D3D11Device.CreateDepthStencilState(new DepthStencilDescription
            {
                DepthEnable = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthFunc = ComparisonFunc.Less,
                StencilEnable = false,
                StencilReadMask = DepthStencilDescription.DefaultStencilReadMask,
                StencilWriteMask = DepthStencilDescription.DefaultStencilWriteMask,
                FrontFace = defaultDepthStencilOp,
                BackFace = defaultDepthStencilOp
            });
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            InputLayout.Dispose();
            RasterizerState.Dispose();
            DepthStencilState.Dispose();
        }
    }
}
