// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a <see cref="Framebuffer"/> attachment.
    /// </summary>
    public struct FramebufferAttachment
    {
        public FramebufferAttachment(Texture texture)
        {
            Texture = texture;
            MipLevel = Slice = 0;
        }

        public FramebufferAttachment(Texture texture, int mipLevel)
        {
            Texture = texture;
            MipLevel = mipLevel;
            Slice = 0;
        }

        public FramebufferAttachment(Texture texture, int mipLevel, int slice)
        {
            Texture = texture;
            MipLevel = mipLevel;
            Slice = slice;
        }

        public Texture Texture;
        public int MipLevel;
        public int Slice;
    }
}
