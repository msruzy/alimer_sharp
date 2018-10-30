// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D12;
using SharpDX.DXGI;

namespace Vortice.Graphics.D3D12
{
    internal static class D3D12Convert
    {
        public static TextureType Convert(ResourceDimension dimension)
        {
            switch (dimension)
            {
                case ResourceDimension.Texture1D:
                    return TextureType.Texture1D;

                case ResourceDimension.Texture2D:
                    return TextureType.Texture2D;

                case ResourceDimension.Texture3D:
                    return TextureType.Texture3D;

                default:
                    return TextureType.Unknown;
            }
        }

        public static TextureUsage Convert(ResourceFlags flags)
        {
            var usage = TextureUsage.Unknown;

            if ((flags & ResourceFlags.DenyShaderResource) == 0)
            {
                usage |= TextureUsage.ShaderRead;
            }

            if ((flags & ResourceFlags.AllowRenderTarget) != 0
                || (flags & ResourceFlags.AllowDepthStencil) != 0)
            {
                usage |= TextureUsage.RenderTarget;
            }

            if ((flags & ResourceFlags.AllowUnorderedAccess) != 0)
            {
                usage |= TextureUsage.ShaderWrite;
            }

            return usage;
        }

        public static ResourceFlags Convert(TextureUsage usage, PixelFormat format)
        {
            var flags = ResourceFlags.None;
            if ((usage & TextureUsage.ShaderRead) == 0)
            {
                flags |= ResourceFlags.DenyShaderResource;
            }

            if ((usage & TextureUsage.ShaderWrite) != 0)
            {
                flags |= ResourceFlags.AllowUnorderedAccess;
            }

            if ((usage & TextureUsage.RenderTarget) != 0)
            {
                if (!PixelFormatUtil.IsDepthStencilFormat(format))
                {
                    flags |= ResourceFlags.AllowRenderTarget;
                }
                else
                {
                    flags |= ResourceFlags.AllowDepthStencil;
                }
            }

            return flags;
        }
    }
}
