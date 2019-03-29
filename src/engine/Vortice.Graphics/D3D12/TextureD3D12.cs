// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDirect3D12;
using SharpDXGI;

namespace Vortice.Graphics.D3D12
{
    internal class TextureD3D12 : Texture
    {
        public readonly Format DXGIFormat;
        public readonly ID3D12Resource Resource;

        public TextureD3D12(DeviceD3D12 device, in TextureDescription description, ID3D12Resource nativeTexture)
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
    }
}
