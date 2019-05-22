// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DirectX.DXGI;
using Vortice.DirectX.Direct3D11;

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

        public static Format Convert(VertexFormat format)
        {
            switch (format)
            {
                case VertexFormat.UChar2:
                    return Format.R8G8_UInt;
                case VertexFormat.UChar4:
                    return Format.R8G8B8A8_UInt;

                case VertexFormat.Char2:
                    return Format.R8G8_SInt;
                case VertexFormat.Char4:
                    return Format.R8G8B8A8_SInt;

                case VertexFormat.UChar2Norm:
                    return Format.R8G8_UNorm;
                case VertexFormat.UChar4Norm:
                    return Format.R8G8B8A8_UNorm;

                case VertexFormat.Char2Norm:
                    return Format.R8G8_SNorm;
                case VertexFormat.Char4Norm:
                    return Format.R8G8B8A8_SNorm;

                case VertexFormat.UShort2:
                    return Format.R16G16_UInt;
                case VertexFormat.UShort4:
                    return Format.R16G16B16A16_UInt;

                case VertexFormat.Short2:
                    return Format.R16G16_SInt;
                case VertexFormat.Short4:
                    return Format.R16G16B16A16_SInt;

                case VertexFormat.UShort2Norm:
                    return Format.R16G16_UNorm;
                case VertexFormat.UShort4Norm:
                    return Format.R16G16B16A16_UNorm;

                case VertexFormat.Short2Norm:
                    return Format.R16G16_SNorm;
                case VertexFormat.Short4Norm:
                    return Format.R16G16B16A16_SNorm;

                case VertexFormat.Half2:
                    return Format.R16G16_Float;

                case VertexFormat.Half4:
                    return Format.R16G16B16A16_Float;

                case VertexFormat.Float:
                    return Format.R32_Float;
                case VertexFormat.Float2:
                    return Format.R32G32_Float;
                case VertexFormat.Float3:
                    return Format.R32G32B32_Float;
                case VertexFormat.Float4:
                    return Format.R32G32B32A32_Float;

                case VertexFormat.UInt:
                    return Format.R32_UInt;
                case VertexFormat.UInt2:
                    return Format.R32G32_UInt;
                case VertexFormat.UInt3:
                    return Format.R32G32B32_UInt;
                case VertexFormat.UInt4:
                    return Format.R32G32B32A32_UInt;

                case VertexFormat.Int:
                    return Format.R32_SInt;
                case VertexFormat.Int2:
                    return Format.R32G32_SInt;
                case VertexFormat.Int3:
                    return Format.R32G32B32_SInt;
                case VertexFormat.Int4:
                    return Format.R32G32B32A32_SInt;

                case VertexFormat.Invalid:
                default:
                    return Format.Unknown;
            }
        }
    }
}
