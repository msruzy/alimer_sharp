// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.DirectX12
{
    internal class DirectX12Texture : Texture
    {
        public readonly SharpDX.DXGI.Format DXGIFormat;
        public readonly Resource Resource;

        public DirectX12Texture(DirectX12GraphicsDevice device, in TextureDescription description, Resource nativeTexture)
            : base(device, description)
        {
            DXGIFormat = DirectX12Convert.Convert(description.Format);
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
    }
}
