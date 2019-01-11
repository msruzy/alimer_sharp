// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class TextureD3D12 : Texture
    {
        public readonly SharpDX.DXGI.Format DXGIFormat;
        public readonly Resource Resource;

        public TextureD3D12(DeviceD3D12 device, in TextureDescription description, Resource nativeTexture)
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
