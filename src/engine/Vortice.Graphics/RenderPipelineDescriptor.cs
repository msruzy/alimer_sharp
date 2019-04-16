// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Describes a rendering <see cref="RenderPipelineState"/>.
    /// </summary>
    public sealed class RenderPipelineDescriptor
    {
        public const int SimultaneousRenderTargetCount = 8;

        private RenderPipelineColorAttachmentDescriptor[] _colorAttachments;

        public Shader VertexShader { get; set; }

        public Shader PixelShader { get; set; }

        public Shader DomainShader { get; set; }

        public Shader HullShader { get; set; }

        public Shader GeometryShader { get; set; }

        public PrimitiveTopology PrimitiveTopology { get; set; } = PrimitiveTopology.TriangeList;

        public RenderPipelineColorAttachmentDescriptor[] ColorAttachments
        {
            get => _colorAttachments ?? (_colorAttachments = new RenderPipelineColorAttachmentDescriptor[SimultaneousRenderTargetCount]);
            set => _colorAttachments = value;
        }

        public PixelFormat DepthStencilAttachmentFormat { get; set; }

        public RasterizerDescriptor RasterizerState { get; set; } = RasterizerDescriptor.Default;
        public DepthStencilDescriptor DepthStencilState { get; set; } = DepthStencilDescriptor.Default;

        /// <summary>
        /// The number of samples in each fragment.
        /// </summary>
        public SampleCount Samples { get; set; } = SampleCount.Count1;
    }
}
