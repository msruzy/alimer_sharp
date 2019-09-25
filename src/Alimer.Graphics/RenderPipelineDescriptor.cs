// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
{
    /// <summary>
    /// Describes a rendering <see cref="PipelineState"/>.
    /// </summary>
    public sealed class RenderPipelineDescriptor
    {
        public const int MaxVertexBufferBindings = 4;
        public const int MaxColorAttachments = 8;

        private VertexLayoutDescriptor[] _vertexLayouts;
        private RenderPipelineColorAttachmentDescriptor[] _colorAttachments;

        public Shader VertexShader { get; set; }

        public Shader TessellationControl { get; set; }

        public Shader TessellationEvaluation { get; set; }

        public Shader GeometryShader { get; set; }
        public Shader FragmentShader { get; set; }

        public VertexLayoutDescriptor[] VertexLayouts
        {
            get => _vertexLayouts ?? (_vertexLayouts = new VertexLayoutDescriptor[MaxVertexBufferBindings]);
            set => _vertexLayouts = value;
        }

        public PrimitiveTopology PrimitiveTopology { get; set; } = PrimitiveTopology.TriangeList;

        public RenderPipelineColorAttachmentDescriptor[] ColorAttachments
        {
            get => _colorAttachments ?? (_colorAttachments = new RenderPipelineColorAttachmentDescriptor[MaxColorAttachments]);
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
