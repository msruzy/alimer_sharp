// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Describes a rendering <see cref="Pipeline"/>.
    /// </summary>
    public struct RenderPipelineDescriptor
    {
        public Shader VertexShader { get; set; }

        public Shader PixelShader { get; set; }

        public Shader DomainShader { get; set; }

        public Shader HullShader { get; set; }

        public Shader GeometryShader { get; set; }

        public RenderPipelineColorAttachmentDescriptor[] ColorAttachments
        {
            get => _RTVFormats ?? (_RTVFormats = new Format[BlendDescription.SimultaneousRenderTargetCount]);
            set => _RTVFormats = value;
        }

        public PixelFormat DepthStencilFormat { get; set; }
    }
}
