// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SharpDX.Direct3D11;

namespace Vortice.Graphics.D3D11
{
    internal static class D3D11Utils
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
