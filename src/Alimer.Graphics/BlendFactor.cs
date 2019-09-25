// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
{
    /// <summary>
    /// Defines the blend factor in <see cref="RenderPipelineColorAttachmentDescriptor"/>.
    /// </summary>
    public enum BlendFactor
    {
        One,
        Zero,
        SourceColor,
        OneMinusSourceColor,
        SourceAlpha,
        OneMinusSourceAlpha,
        DestinationColor,
        OneMinusDestinationColor,
        DestinationAlpha,
        OneMinusDestinationAlpha,
        SourceAlphaSaturated,
        BlendColor,
        OneMinusBlendColor
    }
}
