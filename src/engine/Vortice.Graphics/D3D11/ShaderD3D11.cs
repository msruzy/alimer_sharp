// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDirect3D11;

namespace Vortice.Graphics.D3D11
{
    internal class ShaderD3D11 : Shader
    {
        public readonly ID3D11VertexShader VertexShader;
        public readonly ID3D11PixelShader PixelShader;

        public ShaderD3D11(DeviceD3D11 device, byte[] vertex, byte[] pixel)
            : base(device, isCompute: false)
        {
            VertexShader = device.Device.CreateVertexShader(vertex);
            PixelShader = device.Device.CreatePixelShader(pixel);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            VertexShader?.Dispose();
            PixelShader?.Dispose();
        }
    }
}
