// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines the blend operation in <see cref="RenderPipelineColorAttachmentDescriptor"/>.
    /// </summary>
    public enum BlendOperation
    {
        Add,
        Subtract,
        ReverseSubtract,
        Min,
        Max
    }
}
