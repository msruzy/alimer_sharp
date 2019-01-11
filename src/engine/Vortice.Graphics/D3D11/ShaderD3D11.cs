// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D11;

namespace Vortice.Graphics.D3D11
{
    internal class ShaderD3D11 : Shader
    {
        public readonly VertexShader VertexShader;
        public readonly PixelShader PixelShader;

        public ShaderD3D11(DeviceD3D11 device, byte[] vertex, byte[] pixel)
            : base(device, isCompute: false)
        {
            VertexShader = new VertexShader(device.D3DDevice, vertex);
            PixelShader = new PixelShader(device.D3DDevice, pixel);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            VertexShader?.Dispose();
            PixelShader?.Dispose();
        }
    }
}
