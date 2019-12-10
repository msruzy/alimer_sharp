// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
{
    /// <summary>
    /// Describes <see cref="GraphicsDevice"/> features.
    /// </summary>
    public sealed class GraphicsDeviceFeatures
    {
        public bool IndependentBlend { get; set; }
        public bool ComputeShader { get; set; }
        public bool GeometryShader { get; set; }
        public bool TessellationShader { get; set; }
        public bool SampleRateShading { get; set; }
        public bool DualSrcBlend { get; set; }
        public bool LogicOp { get; set; }
        public bool MultiViewport { get; set; }
        public bool IndexUInt32 { get; set; }
        public bool DrawIndirect { get; set; }
        public bool AlphaToOne { get; set; }
        public bool FillModeNonSolid { get; set; }
        public bool SamplerAnisotropy { get; set; }
        public bool TextureCompressionBC { get; set; }
        public bool TextureCompressionPVRTC { get; set; }
        public bool TextureCompressionETC2 { get; set; }
        public bool TextureCompressionATC { get; set; }
        public bool TextureCompressionASTC { get; set; }
        public bool PipelineStatisticsQuery { get; set; }
        /// Specifies whether 1D textures are supported.
        public bool Texture1D { get; set; }
        /// Specifies whether 3D textures are supported.
        public bool Texture3D { get; set; }
        /// Specifies whether 2D array textures are supported.
        public bool Texture2DArray { get; set; }
        /// Specifies whether cube array textures are supported.
        public bool TextureCubeArray { get; set; }
        /// Specifies whether raytracing is supported.
        public bool Raytracing { get; set; }
    }
}
