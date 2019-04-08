// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDirect3D12;

namespace Vortice.Graphics.D3D12
{
    internal class ShaderD3D12 : Shader
    {
        public readonly ShaderBytecode D3D12ShaderBytecode;

        public ShaderD3D12(DeviceD3D12 device, ShaderStages stage, byte[] byteCode)
            : base(device, stage)
        {
            D3D12ShaderBytecode = new ShaderBytecode(byteCode);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
        }
    }
}
