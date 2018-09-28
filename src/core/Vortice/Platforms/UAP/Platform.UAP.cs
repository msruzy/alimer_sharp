// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using Windows.System.Profile;

namespace Vortice
{
    public static partial class Platform
    {
        static Platform()
        {
            PlatformType = PlatformType.UWP;
            var versionInfo = AnalyticsInfo.VersionInfo;
            switch (versionInfo.DeviceFamily)
            {
                case "Windows.Desktop":
                    //PlatformType = PlatformType.WindowsUniversal;
                    PlatformFamily = PlatformFamily.Desktop;
                    break;
                case "Windows.Mobile":
                    //PlatformType = PlatformType.WindowsMobile;
                    PlatformFamily = PlatformFamily.Mobile;
                    break;
                case "Windows.Xbox":
                    //PlatformType = PlatformType.WindowsXbox;
                    PlatformFamily = PlatformFamily.Console;
                    break;

                case "Windows.Holographic":
                    //PlatformType = PlatformType.WindowsHolographic;
                    PlatformFamily = PlatformFamily.Console;
                    break;

                case "Windows.Team":
                    // SurfaceHub
                    //PlatformType = PlatformType.WindowsTeam;
                    break;

                case "Windows.Universal":
                    break;

                default:
                    PlatformType = PlatformType.Unknown;
                    PlatformFamily = PlatformFamily.Unknown;
                    break;
            }

            FrameworkDescription = RuntimeInformation.FrameworkDescription;
            OSDescription = RuntimeInformation.OSDescription;
        }
    }
}
