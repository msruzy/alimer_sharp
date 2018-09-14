// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;

namespace Vortice.Graphics.DirectX11
{
    internal static class D3D11Convert
    {
        public static RawColor4 Convert(in Color4 color) => new RawColor4(color.R, color.G, color.B, color.A);

        public static Format Convert(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R8UNorm:
                    return Format.R8_UNorm;

                case PixelFormat.RG8UNorm:
                    return Format.R8G8_UNorm;

                case PixelFormat.RGBA8UNorm:
                    return Format.R8G8B8A8_UNorm;

                case PixelFormat.BGRA8UNorm:
                    return Format.B8G8R8A8_UNorm;

                case PixelFormat.Depth16UNorm:
                    return Format.D16_UNorm;

                case PixelFormat.Depth24UNormStencil8:
                    return Format.D24_UNorm_S8_UInt;

                case PixelFormat.Depth32Float:
                    return Format.D32_Float;

                case PixelFormat.Depth32FloatStencil8:
                    return Format.D32_Float_S8X24_UInt;

                case PixelFormat.BC1:
                    return Format.BC1_UNorm;

                case PixelFormat.BC1_sRGB:
                    return Format.BC1_UNorm_SRgb;

                case PixelFormat.BC2:
                    return Format.BC2_UNorm;

                case PixelFormat.BC2_sRGB:
                    return Format.BC2_UNorm_SRgb;

                case PixelFormat.BC3:
                    return Format.BC3_UNorm;

                case PixelFormat.BC3_sRGB:
                    return Format.BC3_UNorm_SRgb;

                case PixelFormat.BC4UNorm:
                    return Format.BC4_UNorm;

                case PixelFormat.BC4SNorm:
                    return Format.BC4_SNorm;

                case PixelFormat.BC5UNorm:
                    return Format.BC5_UNorm;

                case PixelFormat.BC5SNorm:
                    return Format.BC5_SNorm;

                case PixelFormat.BC6HSFloat:
                    return Format.BC6H_Sf16;

                case PixelFormat.BC6HUFloat:
                    return Format.BC6H_Uf16;

                default:
                    return Format.Unknown;
            }
        }

        public static PixelFormat Convert(Format format)
        {
            switch (format)
            {
                case Format.R8_UNorm:
                    return PixelFormat.R8UNorm;

                case Format.R8G8_UNorm:
                    return PixelFormat.RG8UNorm;

                case Format.R8G8B8A8_UNorm:
                    return PixelFormat.RGBA8UNorm;

                case Format.B8G8R8A8_UNorm:
                    return PixelFormat.BGRA8UNorm;

                case Format.D16_UNorm:
                    return PixelFormat.Depth16UNorm;

                case Format.D24_UNorm_S8_UInt:
                    return PixelFormat.Depth24UNormStencil8;

                case Format.D32_Float:
                    return PixelFormat.Depth32Float;

                case Format.D32_Float_S8X24_UInt:
                    return PixelFormat.Depth32FloatStencil8;

                case Format.BC1_UNorm:
                    return PixelFormat.BC1;

                case Format.BC1_UNorm_SRgb:
                    return PixelFormat.BC1_sRGB;

                case Format.BC2_UNorm:
                    return PixelFormat.BC2;

                case Format.BC2_UNorm_SRgb:
                    return PixelFormat.BC2_sRGB;

                case Format.BC3_UNorm:
                    return PixelFormat.BC3;

                case Format.BC3_UNorm_SRgb:
                    return PixelFormat.BC3_sRGB;

                case Format.BC4_UNorm:
                    return PixelFormat.BC4UNorm;

                case Format.BC4_SNorm:
                    return PixelFormat.BC4SNorm;

                case Format.BC5_UNorm:
                    return PixelFormat.BC5UNorm;

                case Format.BC5_SNorm:
                    return PixelFormat.BC5SNorm;

                case Format.BC6H_Sf16:
                    return PixelFormat.BC6HSFloat;

                case Format.BC6H_Uf16:
                    return PixelFormat.BC6HUFloat;

                case Format.BC7_UNorm:
                    return PixelFormat.Unknown;

                default:
                    return PixelFormat.Unknown;
            }
        }

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
            var usage = TextureUsage.Unknown;

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

        #region Platform Detection
        public static int WindowsVersion => GetWindowsVersion();
        public static bool IsWindows7 => GetWindowsVersion() == 6 && GetWindowsMinorVersion() == 1;
        public static bool IsWindows8x => GetWindowsVersion() == 6 && (GetWindowsMinorVersion() == 2 || GetWindowsMinorVersion() == 3);

        public static bool IsWindows10x => GetWindowsVersion() == 10;

        public static bool IsWindows10Version1607OrGreater =>
            GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 14393;
        public static bool IsWindows10Version1703OrGreater =>
            GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 15063;
        public static bool IsWindows10Version1709OrGreater =>
            GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 16299;
        public static bool IsWindows10Version1803OrGreater =>
            GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 17134;

        [DllImport("ntdll.dll")]
        private static extern int RtlGetVersion(out RTL_OSVERSIONINFOEX lpVersionInformation);

        [StructLayout(LayoutKind.Sequential)]
        private struct RTL_OSVERSIONINFOEX
        {
            internal uint dwOSVersionInfoSize;
            internal uint dwMajorVersion;
            internal uint dwMinorVersion;
            internal uint dwBuildNumber;
            internal uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string szCSDVersion;
        }

        private static int GetWindowsVersion()
        {
            var osvi = new RTL_OSVERSIONINFOEX
            {
                dwOSVersionInfoSize = (uint)Unsafe.SizeOf<RTL_OSVERSIONINFOEX>()
            };
            Debug.Assert(RtlGetVersion(out osvi) == 0);
            return (int)osvi.dwMajorVersion;
        }

        private static int GetWindowsMinorVersion()
        {
            var osvi = new RTL_OSVERSIONINFOEX
            {
                dwOSVersionInfoSize = (uint)Unsafe.SizeOf<RTL_OSVERSIONINFOEX>()
            };
            Debug.Assert(RtlGetVersion(out osvi) == 0);
            return (int)osvi.dwMinorVersion;
        }

        private static int GetWindowsBuildNumber()
        {
            var osvi = new RTL_OSVERSIONINFOEX
            {
                dwOSVersionInfoSize = (uint)Unsafe.SizeOf<RTL_OSVERSIONINFOEX>()
            };
            Debug.Assert(RtlGetVersion(out osvi) == 0);
            return (int)osvi.dwBuildNumber;
        }
        #endregion
    }
}
