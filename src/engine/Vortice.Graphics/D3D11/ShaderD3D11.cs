// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDirect3D11;

namespace Vortice.Graphics.D3D11
{
    internal class ShaderD3D11 : Shader
    {
        public readonly ID3D11DeviceChild D3D11Shader;
        public readonly byte[] Bytecode;

        public ShaderD3D11(DeviceD3D11 device, ShaderStages stage, byte[] byteCode)
            : base(device, stage)
        {
            switch (stage)
            {
                case ShaderStages.Vertex:
                    Bytecode = (byte[])byteCode.Clone();
                    D3D11Shader = device.D3D11Device.CreateVertexShader(byteCode);
                    break;

                case ShaderStages.Pixel:
                    D3D11Shader = device.D3D11Device.CreatePixelShader(byteCode);
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
