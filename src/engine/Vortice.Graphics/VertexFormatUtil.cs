// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Utility class for <see cref="VertexFormat"/>.
    /// </summary>
    public static class VertexFormatUtil
    {
        public static int GetSizeInBytes(VertexFormat format)
        {
            switch (format)
            {
                case VertexFormat.UChar2:
                case VertexFormat.Char2:
                case VertexFormat.UChar2Norm:
                case VertexFormat.Char2Norm:
                //case VertexFormat.Half:
                    return 2;
                case VertexFormat.Float:
                case VertexFormat.UInt:
                case VertexFormat.Int:
                case VertexFormat.UChar4:
                case VertexFormat.Char4:
                case VertexFormat.UChar4Norm:
                case VertexFormat.Char4Norm:
                case VertexFormat.UShort2:
                case VertexFormat.Short2:
                case VertexFormat.UShort2Norm:
                case VertexFormat.Short2Norm:
                case VertexFormat.Half2:
                case VertexFormat.UInt1010102Norm:
                    return 4;
                case VertexFormat.Float2:
                case VertexFormat.UInt2:
                case VertexFormat.Int2:
                case VertexFormat.UShort4:
                case VertexFormat.Short4:
                case VertexFormat.UShort4Norm:
                case VertexFormat.Short4Norm:
                case VertexFormat.Half4:
                    return 8;
                case VertexFormat.Float3:
                case VertexFormat.UInt3:
                case VertexFormat.Int3:
                    return 12;
                case VertexFormat.Float4:
                case VertexFormat.UInt4:
                case VertexFormat.Int4:
                    return 16;
                default:
                    throw new GraphicsException($"Invalid {format} value");
            }
        }
    }
}
