// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Windows
{
	internal enum WindowMessage : uint
	{
		Null = 0x0000,
		Create = 0x0001,
		Destroy = 0x0002,
		Move = 0x0003,
		Size = 0x0005,
		Activate = 0x0006,
		SetFocus = 0x0007,
		KillFocus = 0x0008,
		Enable = 0x000A,
		SetRedraw = 0x000B,
		SetText = 0x000C,
		GetText = 0x000D,
		GetTextLength = 0x000E,
		Paint = 0x000F,
		Close = 0x0010,
		QueryEndSession = 0x0011,
		QueryOpen = 0x0013,
		EndSession = 0x0016,
		Quit = 0x0012,
		EraseBackground = 0x0014,
		SystemColorChange = 0x0015,
		ShowWindow = 0x0018,
		WindowsIniChange = 0x001A,
		SettingChange = WindowsIniChange,
		DevModeChange = 0x001B,
		ActivateApp = 0x001C,
		FontChange = 0x001D,
		TimeChange = 0x001E,
		CancelMode = 0x001F,
		SetCursor = 0x0020,
		MouseActivate = 0x0021,
		ChildActivate = 0x0022,
		KeyDown = 0x0100,
		KeyUp = 0x0101,
		Char = 0x0102,
		SysKeyDown = 0x0104,
		SysKeyUp = 0x0105,
		MouseMove = 0x0200,
		LButtonDown = 0x0201,
		LButtonUp = 0x0202,
		MButtonDown = 0x0207,
		MButtonUp = 0x0208,
		RButtonDown = 0x0204,
		RButtonUp = 0x0205,
		MouseWheel = 0x020A,
		XButtonDown = 0x020B,
		XButtonUp = 0x020C,
		MouseLeave = 0x02A3,
		NcMouseMove = 0x00A0,
		WindowPositionChanging = 0x0046,
		WindowPositionChanged = 0x0047,
	}

	internal enum ShowWindowCommand
	{
		/// <summary>
		/// Hides the window and activates another window.
		/// </summary>
		Hide = 0,

		/// <summary>
		/// Activates and displays a window. If the window is minimized or
		/// maximized, the system restores it to its original size and position.
		/// An application should specify this flag when displaying the window
		/// for the first time.
		/// </summary>
		Normal = 1,

		/// <summary>
		/// Activates the window and displays it as a minimized window.
		/// </summary>
		ShowMinimized = 2,

		/// <summary>
		/// Maximizes the specified window.
		/// </summary>
		Maximize = 3,

		/// <summary>
		/// Activates the window and displays it as a maximized window.
		/// </summary>
		ShowMaximized = 3,

		/// <summary>
		/// Displays a window in its most recent size and position. This value
		/// is similar to <see cref="ShowWindowCommand.Normal"/>, except
		/// the window is not activated.
		/// </summary>
		ShowNoActivate = 4,

		/// <summary>
		/// Activates the window and displays it in its current size and position.
		/// </summary>
		Show = 5,

		/// <summary>
		/// Minimizes the specified window and activates the next top-level
		/// window in the Z order.
		/// </summary>
		Minimize = 6,

		/// <summary>
		/// Displays the window as a minimized window. This value is similar to
		/// <see cref="ShowMinimized"/>, except the
		/// window is not activated.
		/// </summary>
		ShowMinNoActive = 7,

		/// <summary>
		/// Displays the window in its current size and position. This value is
		/// similar to <see cref="Show"/>, except the
		/// window is not activated.
		/// </summary>
		ShowNA = 8,

		/// <summary>
		/// Activates and displays the window. If the window is minimized or
		/// maximized, the system restores it to its original size and position.
		/// An application should specify this flag when restoring a minimized window.
		/// </summary>
		Restore = 9,

		/// <summary>
		/// Sets the show state based on the SW_* value specified in the
		/// STARTUPINFO structure passed to the CreateProcess function by the
		/// program that started the application.
		/// </summary>
		ShowDefault = 10,

		/// <summary>
		///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread
		/// that owns the window is not responding. This flag should only be
		/// used when minimizing windows from a different thread.
		/// </summary>
		ForceMinimize = 11
	}

    [Flags]
    public enum SetWindowPosFlags : uint
    {
        SWP_ASYNCWINDOWPOS = 0x4000,
        SWP_DEFERERASE = 0x2000,
        SWP_DRAWFRAME = 0x0020,
        SWP_FRAMECHANGED = 0x0020,
        SWP_HIDEWINDOW = 0x0080,
        SWP_NOACTIVATE = 0x0010,
        SWP_NOCOPYBITS = 0x0100,
        SWP_NOMOVE = 0x0002,
        SWP_NOOWNERZORDER = 0x0200,
        SWP_NOREDRAW = 0x0008,
        SWP_NOREPOSITION = 0x0200,
        SWP_NOSENDCHANGING = 0x0400,
        SWP_NOSIZE = 0x0001,
        SWP_NOZORDER = 0x0004,
        SWP_SHOWWINDOW = 0x0040,

        SWP_RESIZE = SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOZORDER
    }

    internal enum KeyMapType : uint
	{
		VktoSc = 0,
		ScToVk = 1,
		VkToChar = 2,
		ScToVkEx = 3
	}

    public enum ProcessDpiAwareness
    {
        DpiUnaware,
        SystemDpiAware,
        PerMonitorDpiAware
    }

    public enum MonitorDpiType
    {
        EffectiveDpi,
        AngularDpi,
        RawDpi,
        Default = EffectiveDpi
    }
}
