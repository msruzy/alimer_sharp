// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Vortice
{
    public static partial class Platform
    {
        static Platform()
        {
            PlatformFamily = PlatformFamily.Desktop;
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                    PlatformType = PlatformType.macOS;
                    break;

                case PlatformID.Unix:
                    PlatformType = PlatformType.Linux;
                    break;

                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    PlatformType = PlatformType.Windows;
                    break;
            }

            //var assemblyFileVersionAttribute = (AssemblyFileVersionAttribute)(typeof(object).GetTypeInfo().Assembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute)));
            //Debug.Assert(assemblyFileVersionAttribute != null);
            //FrameworkDescription = $"{Environment.Version} {assemblyFileVersionAttribute.Version}";
            FrameworkDescription = Environment.Version.ToString();
            OSDescription = Environment.OSVersion.VersionString;
        }
    }
}
