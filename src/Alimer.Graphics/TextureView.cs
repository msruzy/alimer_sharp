// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Alimer.Graphics
{
    /// <summary>
    /// Defines a <see cref="Texture"/> view.
    /// </summary>
    public abstract class TextureView
    {
        /// <summary>
        /// Gets the associated <see cref="Texture"/> resource.
        /// </summary>
        public Texture Texture { get; }

        protected TextureView(Texture texture, in TextureViewDescriptor descriptor)
        {
            Guard.NotNull(texture, nameof(texture));

            Texture = texture;
        }
    }
}
