// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    internal abstract class GPUTexture
    {
        public TextureType TextureType { get; }
		public int Width { get; }
        public int Height { get; }
        public int Depth { get; }
		public int MipLevels { get; }
        public int ArrayLayers { get; }
		public PixelFormat Format { get; }
        public TextureUsage TextureUsage { get; }
        public SampleCount Samples { get; }

        protected GPUTexture(in TextureDescription description)
        {
            TextureType = description.TextureType;
            Width = description.Width;
            Height = description.Height;
            Depth = description.Depth;
            MipLevels = description.MipLevels;
            ArrayLayers = description.ArrayLayers;
            Format = description.Format;
            TextureUsage = description.TextureUsage;
            Samples = description.Samples;
        }

        public abstract void Destroy();
    }

    internal abstract class GPUFramebuffer
    {
        public abstract void Destroy();
    }

    internal abstract class GPUSwapChain
    {
        public abstract int BackBufferCount { get; }
        public abstract int CurrentBackBuffer { get; }
        public abstract GPUTexture GetBackBufferTexture(int index);

        public abstract void Configure(in SwapChainDescriptor descriptor);
        public abstract void Destroy();
        public abstract void Present();
    }

    internal abstract class GPUBuffer
    {
        public abstract void Destroy();
    }
}
