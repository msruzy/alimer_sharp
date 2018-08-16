// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Vortice.Graphics
{
    internal static class PlatformDetection
    {
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
            RTL_OSVERSIONINFOEX osvi = new RTL_OSVERSIONINFOEX();
            osvi.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osvi);
            Debug.Assert(RtlGetVersion(out osvi) == 0);
            return (int)osvi.dwMajorVersion;
        }

        private static int GetWindowsMinorVersion()
        {
            RTL_OSVERSIONINFOEX osvi = new RTL_OSVERSIONINFOEX();
            osvi.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osvi);
            Debug.Assert(RtlGetVersion(out osvi) == 0);
            return (int)osvi.dwMinorVersion;
        }

        private static int GetWindowsBuildNumber()
        {
            RTL_OSVERSIONINFOEX osvi = new RTL_OSVERSIONINFOEX();
            osvi.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osvi);
            Debug.Assert(RtlGetVersion(out osvi) == 0);
            return (int)osvi.dwBuildNumber;
        }
    }
}
