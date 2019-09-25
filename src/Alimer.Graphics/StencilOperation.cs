// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
{
    /// <summary>
    /// Identifies the stencil operations that can be performed during depth-stencil testing.
    /// </summary>
    public enum StencilOperation
    {
        /// <summary>
        /// Keep the current stencil value.
        /// </summary>
        Keep,

        /// <summary>
        /// Set the stencil data to 0.
        /// </summary>
        Zero,

        /// <summary>
        /// Replace the stencil value with the stencil reference value, which is set by the <see cref="CommandBuffer.SetStencilReference(int)"/>
        /// </summary>
        Replace,

        /// <summary>
        /// Increment the stencil value by 1, and clamp the result.
        /// </summary>
        IncrementClamp,

        /// <summary>
        /// Decrement the stencil value by 1, and clamp the result.
        /// </summary>
        DecrementClamp,

        /// <summary>
        /// Invert the stencil data.
        /// </summary>
        Invert,

        /// <summary>
        /// Increment the stencil value by 1, and wrap the result if necessary.
        /// </summary>
        IncrementWrap,

        /// <summary>
        /// Decrement the stencil value by 1, and wrap the result if necessary.
        /// </summary>
        DecrementWrap
    }
}
