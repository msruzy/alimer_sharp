// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vortice.Windows
{
    public static class PlatformDetection
    {
        public static readonly int WindowsVersion = GetWindowsVersion();
        public static readonly int WindowsMinorVersion = GetWindowsMinorVersion();
        public static readonly int WindowsBuildNumber = GetWindowsBuildNumber();

        public static bool IsWindows7OrGreater()
        {
            return WindowsVersion == 6 && WindowsMinorVersion == 1;
        }

        public static bool IsWindows8Point1OrGreater()
        {
            return WindowsVersion >= 6 || WindowsMinorVersion > 1;
        }

        public static bool IsWindows10Version1607OrGreater() =>
           GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 14393;

        public static bool IsWindows10Version1703OrGreater() =>
            GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 15063;

        public static bool IsWindows10Version1709OrGreater() =>
            GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 16299;
        public static bool IsWindows10Version1803OrGreater() =>
            GetWindowsVersion() == 10 && GetWindowsMinorVersion() == 0 && GetWindowsBuildNumber() >= 17134;

        [DllImport("ntdll.dll", ExactSpelling = true)]
        private static extern int RtlGetVersion(ref RTL_OSVERSIONINFOEX lpVersionInformation);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private unsafe struct RTL_OSVERSIONINFOEX
        {
            internal uint dwOSVersionInfoSize;
            internal uint dwMajorVersion;
            internal uint dwMinorVersion;
            internal uint dwBuildNumber;
            internal uint dwPlatformId;
            internal fixed char szCSDVersion[128];
        }

        private static unsafe int GetWindowsVersion()
        {
            var osvi = new RTL_OSVERSIONINFOEX
            {
                dwOSVersionInfoSize = (uint)Unsafe.SizeOf<RTL_OSVERSIONINFOEX>()
            };
            Debug.Assert(RtlGetVersion(ref osvi) == 0);
            return (int)osvi.dwMajorVersion;
        }

        private static unsafe int GetWindowsMinorVersion()
        {
            var osvi = new RTL_OSVERSIONINFOEX
            {
                dwOSVersionInfoSize = (uint)Unsafe.SizeOf<RTL_OSVERSIONINFOEX>()
            };
            Debug.Assert(RtlGetVersion(ref osvi) == 0);
            return (int)osvi.dwMinorVersion;
        }

        private static unsafe int GetWindowsBuildNumber()
        {
            var osvi = new RTL_OSVERSIONINFOEX
            {
                dwOSVersionInfoSize = (uint)Unsafe.SizeOf<RTL_OSVERSIONINFOEX>()
            };
            Debug.Assert(RtlGetVersion(ref osvi) == 0);
            return (int)osvi.dwBuildNumber;
        }
    }
}
