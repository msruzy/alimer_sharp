// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX.Direct3D11;

namespace Vortice.Graphics.D3D11
{
    internal static class Utils
    {
        public static CpuAccessFlags Convert(GraphicsResourceUsage usage)
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

        public static TextureUsage Convert(BindFlags flags)
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

        public static BindFlags Convert(BufferUsage usage)
        {
            if ((usage & BufferUsage.Uniform) != 0)
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

        public static BindFlags Convert(TextureUsage usage, PixelFormat format)
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

        public static TextureDescription Convert(Texture2DDescription description)
        {
            return new TextureDescription(
                TextureType.Texture2D,
                description.Width,
                description.Height,
                1,
                description.MipLevels,
                description.ArraySize,
                D3DConvert.Convert(description.Format),
                Convert(description.BindFlags),
                (SampleCount)description.SampleDescription.Count
                );
        }
    }
}
