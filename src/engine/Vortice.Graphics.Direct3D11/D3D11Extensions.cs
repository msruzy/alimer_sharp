// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DXGI;
using Vortice.Direct3D11;
using System.Collections;
using System.Collections.Generic;

namespace Vortice.Graphics.Direct3D11
{
    public static class D3D11Extensions
    {
        #region ToDirectX Methods
        public static CpuAccessFlags ToDirectX(this GraphicsResourceUsage usage)
        {
            switch (usage)
            {
                case GraphicsResourceUsage.Dynamic:
                    return CpuAccessFlags.Write;
                case GraphicsResourceUsage.Staging:
                    return CpuAccessFlags.Read | CpuAccessFlags.Write;
                default:
                    return CpuAccessFlags.None;
            }
        }

        public static BindFlags ToDirectX(this BufferUsage usage)
        {
            if ((usage & BufferUsage.Constant) != 0)
            {
                // Exclusive usage.
                return BindFlags.ConstantBuffer;
            }

            var bindFlags = BindFlags.None;
            if ((usage & BufferUsage.Vertex) != 0)
            {
                bindFlags |= BindFlags.VertexBuffer;
            }

            if ((usage & BufferUsage.Index) != 0)
            {
                bindFlags |= BindFlags.IndexBuffer;
            }

            if ((usage & BufferUsage.Storage) != 0)
            {
                bindFlags |= BindFlags.UnorderedAccess;
            }

            return bindFlags;
        }

        public static BindFlags ToDirectX(this TextureUsage usage, PixelFormat format)
        {
            var bindFlags = BindFlags.None;
            if ((usage & TextureUsage.ShaderRead) != 0)
            {
                bindFlags |= BindFlags.ShaderResource;
            }

            if ((usage & TextureUsage.ShaderWrite) != 0)
            {
                bindFlags |= BindFlags.UnorderedAccess;
            }

            if ((usage & TextureUsage.RenderTarget) != 0)
            {
                if (!PixelFormatUtil.IsDepthStencilFormat(format))
                {
                    bindFlags |= BindFlags.RenderTarget;
                }
                else
                {
                    bindFlags |= BindFlags.DepthStencil;
                }
            }

            return bindFlags;
        }
        #endregion ToDirectX Methods

        #region FromDirectX Methods
        public static TextureType FromDirectX(this ResourceDimension dimension)
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

        public static TextureUsage FromDirectX(this BindFlags flags)
        {
            var usage = TextureUsage.None;

            if ((flags & BindFlags.ShaderResource) != 0)
            {
                usage |= TextureUsage.ShaderRead;
            }

            if ((flags & BindFlags.RenderTarget) != 0
                || (flags & BindFlags.DepthStencil) != 0)
            {
                usage |= TextureUsage.RenderTarget;
            }

            if ((flags & BindFlags.UnorderedAccess) != 0)
            {
                usage |= TextureUsage.ShaderWrite;
            }

            return usage;
        }

        public static TextureDescriptor FromDirectX(this Texture2DDescription description)
        {
            return new TextureDescriptor(
                TextureType.Texture2D,
                description.Width,
                description.Height,
                1,
                description.MipLevels,
                description.ArraySize,
                description.Format.FromDirectXPixelFormat(),
                description.BindFlags.FromDirectX(),
                (SampleCount)description.SampleDescription.Count
                );
        }
        #endregion FromDirectX Methods
    }
}
