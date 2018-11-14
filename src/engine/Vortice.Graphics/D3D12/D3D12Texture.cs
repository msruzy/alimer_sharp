// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12Texture : Texture
    {
        public readonly SharpDX.DXGI.Format DXGIFormat;
        public readonly Resource Resource;

        public D3D12Texture(D3D12GraphicsDevice device, in TextureDescription description, Resource nativeTexture)
            : base(device, description)
        {
            DXGIFormat = D3DConvert.Convert(description.Format);
            if (nativeTexture == null)
            {
            }
            else
            {
                Resource = nativeTexture;
            }
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            Resource.Dispose();
        }

        protected override TextureView CreateTextureViewCore(in TextureViewDescriptor descriptor)
        {
            return null;
        }
    }
}
