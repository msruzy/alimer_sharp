// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DirectX.Direct3D12;

namespace Vortice.Graphics.Direct3D12
{
    internal class ShaderD3D12 : Shader
    {
        public readonly Vortice.DirectX.Direct3D12.ShaderBytecode D3D12ShaderBytecode;

        public ShaderD3D12(D3D12GraphicsDevice device, ShaderBytecode bytecode)
            : base(device, bytecode)
        {
            D3D12ShaderBytecode = new Vortice.DirectX.Direct3D12.ShaderBytecode(bytecode.Data);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
        }
    }
}
