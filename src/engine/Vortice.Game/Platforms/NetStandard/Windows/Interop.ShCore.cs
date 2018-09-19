// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using static Vortice.Windows.Kernel32;

namespace Vortice.Windows
{
    

    internal static class ShCore
    {
		public const string Name = "shcore";

		

		private static readonly IntPtr ShCoreDll = LoadLibrary(Name);

		public static bool IsShCoreAvailable => ShCoreDll != IntPtr.Zero;

		[DllImport(Name)]
		public static extern long SetProcessDpiAwareness(ProcessDpiAwareness value);

		[DllImport(Name)]
		public static extern long GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

		[DllImport(Name)]
		public static extern void GetScaleFactorForMonitor(IntPtr hMon, out uint pScale);
	}
}
