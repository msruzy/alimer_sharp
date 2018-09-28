// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    public struct TextureViewDescriptor
    {
        public PixelFormat Format { get; set; }
    }

    /// <summary>
    /// Defines a <see cref="TextureView"/> class.
    /// </summary>
    public abstract class TextureView
    {
        protected TextureView(Texture texture, in TextureViewDescriptor descriptor)
        {
        }

        protected internal abstract void Destroy();
    }
}
