// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDirect3D11;

namespace Vortice.Graphics.D3D11
{
    internal class CommandBufferD3D11 : CommandBuffer
    {
        public readonly ID3D11DeviceContext D3D11Context;
        private readonly ID3D11DeviceContext1 _context1;
        private readonly ID3DUserDefinedAnnotation _annotation;
        private readonly bool _isImmediate;
        private readonly bool _needWorkaround;
        private ID3D11CommandList _commandList;

        private readonly RenderPassCommandEncoderD3D11 _renderPassEncoder;
        private readonly ComputePassCommandEncoderD3D11 _computePassEncoder;

        public ID3D11CommandList CommandList => _commandList;

        public CommandBufferD3D11(CommandQueueD3D11 commandQueue, ID3D11DeviceContext context)
            : base(commandQueue)
        {
            D3D11Context = context;
            _context1 = context.QueryInterfaceOrNull<ID3D11DeviceContext1>();
            _annotation = context.QueryInterfaceOrNull<ID3DUserDefinedAnnotation>();
            _isImmediate = context.GetContextType() == DeviceContextType.Immediate;

            if (!_isImmediate)
            {
                // The runtime emulates command lists.
                _needWorkaround = !((DeviceD3D11)commandQueue.Device).SupportsCommandLists;
            }

            _renderPassEncoder = new RenderPassCommandEncoderD3D11(this);
            _computePassEncoder = new ComputePassCommandEncoderD3D11(this);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Reset();

            _annotation?.Dispose();
            _context1?.Dispose();
            D3D11Context.Dispose();
        }

        public void Reset()
        {
            _commandList?.Dispose();
            _commandList = null;
            D3D11Context.ClearState();
        }

        protected override RenderPassCommandEncoder BeginRenderPassCore(in RenderPassDescriptor descriptor)
        {
            _renderPassEncoder.Begin(descriptor);
            return _renderPassEncoder;
        }

        protected override ComputePassCommandEncoder BeginComputePassCore()
        {
            _computePassEncoder.Begin();
            return _computePassEncoder;
        }
    }
}
