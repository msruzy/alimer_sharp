// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Describes the front-facing or back-facing stencil operations of <see cref="DepthStencilDescriptor"/>.
    /// </summary>
    public struct DepthStencilOperationDescriptor
    {
        /// <summary>
        /// A <see cref="StencilOperation"/> value that identifies the stencil operation to perform when stencil testing fails.
        /// </summary>
        public StencilOperation StencilFailOperation;

        /// <summary>
        /// A <see cref="StencilOperation"/> value that identifies the stencil operation to perform when stencil testing passes and depth testing fails.
        /// </summary>
        public StencilOperation StencilDepthFailOperation;

        /// <summary>
        /// A <see cref="StencilOperation"/> value that identifies the stencil operation to perform when stencil testing and depth testing both pass.
        /// </summary>
        public StencilOperation StencilPassOperation;

        /// <summary>
        /// A <see cref="CompareFunction"/> value that identifies the function that compares stencil data against existing stencil data.
        /// </summary>
        public CompareFunction StencilCompareFunction;

        /// <summary>
        /// A built-in description with default values.
        /// </summary>
        public static readonly DepthStencilOperationDescriptor Default = new DepthStencilOperationDescriptor(
            StencilOperation.Keep, StencilOperation.Keep, StencilOperation.Keep, CompareFunction.Always);

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilOperationDescriptor"/> struct.
        /// </summary>
        /// <param name="stencilFailOperation">A <see cref="StencilOperation"/> value that identifies the stencil operation to perform when stencil testing fails.</param>
        /// <param name="stencilDepthFailOperation">A <see cref="StencilOperation"/> value that identifies the stencil operation to perform when stencil testing passes and depth testing fails.</param>
        /// <param name="stencilPassOperation">A <see cref="StencilOperation"/> value that identifies the stencil operation to perform when stencil testing and depth testing both pass.</param>
        /// <param name="stencilCompareFunction">A <see cref="CompareFunction"/> value that identifies the function that compares stencil data against existing stencil data.</param>
        public DepthStencilOperationDescriptor(
            StencilOperation stencilFailOperation,
            StencilOperation stencilDepthFailOperation,
            StencilOperation stencilPassOperation,
            CompareFunction stencilCompareFunction)
        {
            StencilFailOperation = stencilFailOperation;
            StencilDepthFailOperation = stencilDepthFailOperation;
            StencilPassOperation = stencilPassOperation;
            StencilCompareFunction = stencilCompareFunction;
        }
    }
}
