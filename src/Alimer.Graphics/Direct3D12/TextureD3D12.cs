// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.Direct3D12;
using Vortice.DXGI;

namespace Alimer.Graphics.Direct3D12
{
    internal class TextureD3D12 : Texture
    {
        public readonly Format DXGIFormat;
        public readonly ID3D12Resource Resource;

        public TextureD3D12(
            D3D12GraphicsDevice device,
            in TextureDescriptor descriptor,
            ID3D12Resource nativeTexture)
            : base(device, descriptor)
        {
            DXGIFormat = descriptor.Format.ToDirectX();
            if (nativeTexture == null)
            {
                ResourceFlags resourceFlags = ResourceFlags.None;
                if ((descriptor.Usage & TextureUsage.ShaderWrite) != 0)
                {
                    resourceFlags |= ResourceFlags.AllowUnorderedAccess;
                }

                // A multisampled resource must have either D3D12_RESOURCE_FLAG_ALLOW_RENDER_TARGET or
                // D3D12_RESOURCE_FLAG_ALLOW_DEPTH_STENCIL set in D3D12_RESOURCE_DESC::Flags.
                if ((descriptor.Usage & TextureUsage.RenderTarget) != 0
                    || descriptor.Samples > 0
                    || !PixelFormatUtil.IsCompressed(descriptor.Format))
                {
                    if (PixelFormatUtil.IsDepthStencilFormat(descriptor.Format))
                    {
                        if ((descriptor.Usage & TextureUsage.ShaderRead) == 0)
                        {
                            resourceFlags |= ResourceFlags.DenyShaderResource;
                        }

                        resourceFlags |= ResourceFlags.AllowDepthStencil;
                    }
                    else
                    {
                        resourceFlags |= ResourceFlags.AllowRenderTarget;
                    }
                }
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

        protected override TextureView CreateView()
        {
            throw new System.NotImplementedException();
        }
    }
}
