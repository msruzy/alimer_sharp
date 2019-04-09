// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDirect3D11;

namespace Vortice.Graphics.D3D11
{
    internal class ComputePassCommandEncoderD3D11 : ComputePassCommandEncoder
    {
        private readonly ID3D11DeviceContext _context;

        public ComputePassCommandEncoderD3D11(CommandBufferD3D11 commandBuffer)
            : base(commandBuffer)
        {
            _context = commandBuffer.D3D11Context;
        }

        public void Begin()
        {
            _commandBuffer.IsEncodingPass = true;
        }

        protected override void DispatchCore(int groupCountX, int groupCountY, int groupCountZ)
        {
            _context.Dispatch(groupCountX, groupCountY, groupCountZ);
        }

        protected override void EndPassImpl()
        {
        }
    }
}
