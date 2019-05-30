// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Describes a depth-stencil state in <see cref="RenderPipelineDescriptor"/>.
    /// </summary>
    public readonly struct DepthStencilDescriptor
    {
        /// <summary>
        /// A bool value that indicates whether depth values can be written to the depth attachment.
        /// </summary>
        public readonly bool IsDepthWriteEnabled;

        /// <summary>
        /// The <see cref="CompareFunction"/> that is performed between a fragment’s depth value and the depth value in the attachment, which determines whether to discard the fragment.
        /// </summary>
        public readonly CompareFunction DepthCompareFunction;

        /// <summary>
        /// The stencil descriptor for front-facing primitives.
        /// </summary>
        public readonly DepthStencilOperationDescriptor FrontFaceStencil;

        /// <summary>
        /// The stencil descriptor for back-facing primitives.
        /// </summary>
        public readonly DepthStencilOperationDescriptor BackFaceStencil;

        /// <summary>
        /// A bitmask that determines from which bits that stencil comparison tests can read.
        /// </summary>
        public readonly byte StencilReadMask;

        /// <summary>
        /// A bitmask that determines to which bits that stencil operations can write.
        /// </summary>
        public readonly byte StencilWriteMask;

        public const int DefaultStencilReadMask = byte.MaxValue;
        public const int DefaultStencilWriteMask = byte.MaxValue;

        /// <summary>
        /// A built-in description with default settings for using a depth stencil buffer.
        /// </summary>
        public static readonly DepthStencilDescriptor Default = new DepthStencilDescriptor(false, CompareFunction.Always);

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilDescriptor"/> struct.
        /// </summary>
        /// <param name="isDepthWriteEnabled">A bool value that indicates whether depth values can be written to the depth attachment.</param>
        /// <param name="depthCompareFunction">The <see cref="CompareFunction"/> that is performed between a fragment’s depth value and the depth value in the attachment, which determines whether to discard the fragment.</param>
        /// <param name="stencilReadMask">A bitmask that determines from which bits that stencil comparison tests can read.</param>
        /// <param name="stencilWriteMask">A bitmask that determines to which bits that stencil operations can write.</param>
        public DepthStencilDescriptor(
            bool isDepthWriteEnabled,
            CompareFunction depthCompareFunction,
            byte stencilReadMask = DefaultStencilReadMask,
            byte stencilWriteMask = DefaultStencilWriteMask)
        {
            IsDepthWriteEnabled = isDepthWriteEnabled;
            DepthCompareFunction = depthCompareFunction;
            FrontFaceStencil = DepthStencilOperationDescriptor.Default;
            BackFaceStencil = DepthStencilOperationDescriptor.Default;
            StencilReadMask = stencilReadMask;
            StencilWriteMask = stencilWriteMask;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilDescriptor"/> struct.
        /// </summary>
        /// <param name="isDepthWriteEnabled">A bool value that indicates whether depth values can be written to the depth attachment.</param>
        /// <param name="depthCompareFunction">The <see cref="CompareFunction"/> that is performed between a fragment’s depth value and the depth value in the attachment, which determines whether to discard the fragment.</param>
        /// <param name="frontFaceStencilFailOperation"></param>
        /// <param name="frontFaceStencilDepthFailOperation"></param>
        /// <param name="frontFaceStencilPassOperation"></param>
        /// <param name="frontFaceStencilCompareFunction"></param>
        /// <param name="backFaceStencilFailOperation"></param>
        /// <param name="backFaceStencilDepthFailOperation"></param>
        /// <param name="backFaceStencilPassOperation"></param>
        /// <param name="backFaceStencilCompareFunction"></param>
        /// <param name="stencilReadMask">A bitmask that determines from which bits that stencil comparison tests can read.</param>
        /// <param name="stencilWriteMask">A bitmask that determines to which bits that stencil operations can write.</param>
        public DepthStencilDescriptor(
            bool isDepthWriteEnabled,
            CompareFunction depthCompareFunction,
            StencilOperation frontFaceStencilFailOperation,
            StencilOperation frontFaceStencilDepthFailOperation,
            StencilOperation frontFaceStencilPassOperation,
            CompareFunction frontFaceStencilCompareFunction,
            StencilOperation backFaceStencilFailOperation,
            StencilOperation backFaceStencilDepthFailOperation,
            StencilOperation backFaceStencilPassOperation,
            CompareFunction backFaceStencilCompareFunction,
            byte stencilReadMask = DefaultStencilReadMask,
            byte stencilWriteMask = DefaultStencilWriteMask)
        {
            IsDepthWriteEnabled = isDepthWriteEnabled;
            DepthCompareFunction = depthCompareFunction;

            // Front
            FrontFaceStencil = new DepthStencilOperationDescriptor(
                frontFaceStencilFailOperation, frontFaceStencilDepthFailOperation, frontFaceStencilPassOperation, frontFaceStencilCompareFunction);

            // Back
            BackFaceStencil = new DepthStencilOperationDescriptor(
                backFaceStencilFailOperation, backFaceStencilDepthFailOperation, backFaceStencilPassOperation, backFaceStencilCompareFunction);

            StencilReadMask = stencilReadMask;
            StencilWriteMask = stencilWriteMask;
        }
    }
}
