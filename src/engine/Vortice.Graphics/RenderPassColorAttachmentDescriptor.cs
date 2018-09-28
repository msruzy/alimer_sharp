// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// A color render target attachment.
    /// </summary>
    public struct RenderPassColorAttachmentDescriptor
    {
        /// <summary>
        /// Gets the <see cref="TextureView"/>.
        /// </summary>
        public TextureView Attachment;

        /// <summary>
        /// Gets or sets the <see cref="LoadAction"/>.
        /// </summary>
        public LoadAction LoadAction;

        /// <summary>
        /// Gets or sets the <see cref="StoreAction"/>.
        /// </summary>
        public StoreAction StoreAction;

        /// <summary>
        /// Gets or sets the clear <see cref="Color4"/>.
        /// </summary>
        public Color4 ClearColor;

        public RenderPassColorAttachmentDescriptor(
            TextureView attachment,
            LoadAction loadAction = LoadAction.Clear,
            StoreAction storeAction = StoreAction.Store)
        {
            Guard.NotNull(attachment, nameof(attachment));
            Attachment = attachment;
            LoadAction = loadAction;
            StoreAction = storeAction;
            ClearColor = new Color4(0.0f, 0.0f, 0.0f, 1.0f);
        }

        public RenderPassColorAttachmentDescriptor(
            TextureView attachment,
            LoadAction loadAction,
            StoreAction storeAction,
            Color4 clearColor)
        {
            Guard.NotNull(attachment, nameof(attachment));
            Attachment = attachment;
            LoadAction = loadAction;
            StoreAction = storeAction;
            ClearColor = clearColor;
        }
    }
}
