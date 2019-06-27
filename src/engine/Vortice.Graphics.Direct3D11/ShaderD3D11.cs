// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DirectX.Direct3D11;

namespace Vortice.Graphics.Direct3D11
{
    internal class ShaderD3D11 : Shader
    {
        public readonly ID3D11DeviceChild D3D11Shader;
        public readonly byte[] Bytecode;

        public ShaderD3D11(DeviceD3D11 device, ShaderBytecode bytecode)
            : base(device, bytecode)
        {
            switch (bytecode.Stage)
            {
                case ShaderStages.Vertex:
                    Bytecode = (byte[])bytecode.Data.Clone();
                    D3D11Shader = device.D3D11Device.CreateVertexShader(bytecode.Data);
                    break;

                case ShaderStages.Hull:
                    D3D11Shader = device.D3D11Device.CreateHullShader(bytecode.Data);
                    break;

                case ShaderStages.Domain:
                    D3D11Shader = device.D3D11Device.CreateDomainShader(bytecode.Data);
                    break;

                case ShaderStages.Geometry:
                    D3D11Shader = device.D3D11Device.CreateGeometryShader(bytecode.Data);
                    break;

                case ShaderStages.Pixel:
                    D3D11Shader = device.D3D11Device.CreatePixelShader(bytecode.Data);
                    break;

                case ShaderStages.Compute:
                    D3D11Shader = device.D3D11Device.CreateComputeShader(bytecode.Data);
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            D3D11Shader.Dispose();
        }
    }
}
