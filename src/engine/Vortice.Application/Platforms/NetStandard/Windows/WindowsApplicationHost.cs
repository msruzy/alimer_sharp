// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//using Vortice.Diagnostics;
using static Vortice.Windows.Kernel32;
using static Vortice.Windows.User32;
using static Vortice.Windows.ShCore;

namespace Vortice.Windows
{
    internal class WindowsApplicationHost : ApplicationHost
    {
        public static readonly string WndClassName = "AlimerWindow";

        public readonly IntPtr HInstance = GetModuleHandle(null);

        private bool _exitRequested;
        private bool _paused;
        private readonly WindowProc _wndProc;

        private readonly Dictionary<IntPtr, Win32Window> _windows = new Dictionary<IntPtr, Win32Window>();

        public override View MainView { get; }

        public WindowsApplicationHost(Application application)
            : base(application)
        {
            // TODO: Add options for EnableHighResolution
            const bool EnableHighResolution = true;
            if (EnableHighResolution)
            {
                if (IsShCoreAvailable)
                {
                    if (SetProcessDpiAwareness(ProcessDpiAwareness.PerMonitorDpiAware) == 0)
                    {
                        //Log.Debug("Enabled per monitor DPI awareness.");
                    }
                    else
                    {
                        //Log.Error("Failed to set process DPI awareness");
                    }
                }
                else
                {
                    if (!SetProcessDPIAware())
                    {
                        //Log.Error("Failed to set process DPI awareness");
                    }
                    else
                    {
                        //Log.Debug("User32 process DPI awareness enabled.");
                    }
                }
            }

            //EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
            //    (IntPtr monitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr data) =>
            //    {
            //        var monitorInfo = new MonitorInfo
            //        {
            //            Size = (uint)Marshal.SizeOf<MonitorInfo>()
            //        };

            //        if (!GetMonitorInfo(monitor, ref monitorInfo))
            //            throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());

            //        Rectangle bounds = monitorInfo.MonitorRect;
            //        RECT workingArea = monitorInfo.WorkRect;

            //        return true;
            //    }, IntPtr.Zero);

            // Register main class.
            _wndProc = ProcessWindowMessage;
            var wndClassEx = new WindowClassEx
            {
                Size = (uint)Marshal.SizeOf<WindowClassEx>(),
                Styles = WindowClassStyles.CS_HREDRAW | WindowClassStyles.CS_VREDRAW | WindowClassStyles.CS_OWNDC,
                WindowProc = _wndProc,
                InstanceHandle = HInstance,
                CursorHandle = LoadCursor(IntPtr.Zero, SystemCursor.IDC_ARROW),
                BackgroundBrushHandle = IntPtr.Zero,
                IconHandle = IntPtr.Zero,
                ClassName = WndClassName,
            };

            var atom = RegisterClassEx(ref wndClassEx);

            if (atom == 0)
            {
                throw new InvalidOperationException(
                    $"Failed to register window class. Error: {Marshal.GetLastWin32Error()}"
                    );
            }

            MainView = new Win32Window(this, "Vortice", 800, 600);
        }

        public override void Run()
        {
            while (!_exitRequested)
            {
                if (!_paused)
                {
                    const uint PM_REMOVE = 1;
                    if (PeekMessage(out var msg, IntPtr.Zero, 0, 0, PM_REMOVE))
                    {
                        TranslateMessage(ref msg);
                        DispatchMessage(ref msg);

                        if (msg.Value == (uint)WindowMessage.Quit)
                        {
                            _exitRequested = true;
                            break;
                        }
                    }

                    OnIdle();
                }
                else
                {
                    var ret = GetMessage(out var msg, IntPtr.Zero, 0, 0);
                    if (ret == 0)
                    {
                        _exitRequested = true;
                        break;
                    }
                    else if (ret == -1)
                    {
                        //Log.Error("[Win32] - Failed to get message");
                        _exitRequested = true;
                        break;
                    }
                    else
                    {
                        TranslateMessage(ref msg);
                        DispatchMessage(ref msg);
                    }
                }
            }
        }

        public override void Exit()
        {
            _exitRequested = true;
        }

        internal void RegisterWindow(Win32Window window)
        {
            _windows.Add(window.Handle, window);
        }

        internal bool RemoveWindow(IntPtr windowId) => _windows.Remove(windowId);

        private IntPtr ProcessWindowMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == (uint)WindowMessage.ActivateApp)
            {
                _paused = IntPtrToInt32(wParam) == 0;
                if (IntPtrToInt32(wParam) != 0)
                {
                    OnActivated();
                }
                else
                {
                    OnDeactivated();
                }

                return DefWindowProc(hWnd, msg, wParam, lParam);
            }

            const int HTCLIENT = 0x1;

            if (_windows.TryGetValue(hWnd, out var window))
            {
                //Log.Debug($"Received {(WindowMessage)msg}");

                switch ((WindowMessage)msg)
                {
                    case WindowMessage.EraseBackground:
                        return new IntPtr(1);

                    case WindowMessage.SetCursor:
                        if (SignedLOWORD(lParam) == HTCLIENT)
                        {
                            //Input.UpdateCursor();
                            return new IntPtr(1);
                        }

                        break;

                    case WindowMessage.SetFocus:
                        //window._focused = true;
                        //window.RaiseFocusChanged(true);
                        return IntPtr.Zero;

                    case WindowMessage.KillFocus:
                        //window._focused = false;
                        //window.RaiseFocusChanged(false);
                        return IntPtr.Zero;

                    case WindowMessage.KeyDown:
                    case WindowMessage.SysKeyDown:
                        {
                            var lp = lParam.ToInt64();
                            var scanCode = (uint)((lp >> 16) & 0xFF);
                            VirtualKey vk = MapVirtualKey(scanCode, KeyMapType.ScToVkEx);
                            //var key = KeyMap.Map[(int)vk];
                            var c = (char)MapVirtualKey((uint)vk, KeyMapType.VkToChar);
                            var repeats = (int)(lp & 0xFFFF);
                            var repeated = ((lp >> 30) & 1) > 0;
                            //window.RaiseKeyDown(key, repeats, repeated, (int)scanCode, c);
                            //if (!repeated)
                            //	window.RaiseKeyPressed(key, (int)scanCode, c);
                            //Log.Info($"Pressed {vk}, {c}, {repeats}, {repeated}");
                            break;
                        }
                    case WindowMessage.KeyUp:
                    case WindowMessage.SysKeyUp:
                        {
                            var lp = lParam.ToInt64();
                            var scanCode = (uint)((lp >> 16) & 0xFF);
                            VirtualKey vk = MapVirtualKey(scanCode, KeyMapType.ScToVkEx);
                            //var key = KeyMap.Map[(int)vk];
                            var c = (char)MapVirtualKey((uint)vk, KeyMapType.VkToChar);
                            //window.RaiseKeyUp(key, (int)scanCode, c);
                            break;
                        }
                    case WindowMessage.Char:
                        {
                            var lp = lParam.ToInt64();
                            var scanCode = (uint)((lp >> 16) & 0xFF);
                            VirtualKey vk = MapVirtualKey(scanCode, KeyMapType.ScToVkEx);
                            if (vk != VirtualKey.Escape
                                && vk != VirtualKey.Tab
                                && vk != VirtualKey.Back)
                            {
                                //window.RaiseTextInput((char)wParam);
                            }

                            return IntPtr.Zero;
                        }

                    //case WindowMessage.NcMouseMove:
                    case WindowMessage.MouseMove:
                        {
                            //var p = MakePoint(lParam);
                            //window.RaiseMouseMoved(p);
                            break;
                        }
                    case WindowMessage.LButtonDown:
                        {
                            //var p = MakePoint(lParam);
                            //window.RaiseMouseDown(MouseButton.Left, p);
                            break;
                        }
                    case WindowMessage.LButtonUp:
                        {
                            //var p = MakePoint(lParam);
                            //window.RaiseMouseUp(MouseButton.Left, p);
                            break;
                        }
                    case WindowMessage.MButtonDown:
                        {
                            //var p = MakePoint(lParam);
                            //window.RaiseMouseDown(MouseButton.Middle, p);
                            break;
                        }
                    case WindowMessage.MButtonUp:
                        {
                            //var p = MakePoint(lParam);
                            //window.RaiseMouseUp(MouseButton.Middle, p);
                            break;
                        }
                    case WindowMessage.RButtonDown:
                        {
                            //var p = MakePoint(lParam);
                            //window.RaiseMouseDown(MouseButton.Right, p);
                            break;
                        }
                    case WindowMessage.RButtonUp:
                        {
                            //var p = MakePoint(lParam);
                            //window.RaiseMouseUp(MouseButton.Right, p);
                            break;
                        }
                    case WindowMessage.XButtonDown:
                        {
                            //var p = MakePoint(lParam);
                            //var btn = ((wParam.ToInt32() >> 16) & 1) > 0 ? MouseButton.X1 : MouseButton.X2;
                            //window.RaiseMouseDown(btn, p);
                            break;
                        }
                    case WindowMessage.XButtonUp:
                        {
                            //var p = MakePoint(lParam);
                            //var btn = ((wParam.ToInt32() >> 16) & 1) > 0 ? MouseButton.X1 : MouseButton.X2;
                            //window.RaiseMouseUp(btn, p);
                            break;
                        }
                    case WindowMessage.MouseLeave:
                        //LogDebug("Mouse left!");
                        break;
                    case WindowMessage.MouseWheel:
                        //LogDebug("Mouse wheel!");
                        break;

                    case WindowMessage.Close:
                        window.Close();
                        return IntPtr.Zero;

                    case WindowMessage.Destroy:
                        if (window.Handle != IntPtr.Zero)
                        {
                            window.HandleDestroy();
                        }

                        RemoveWindow(hWnd);
                        if (_windows.Count == 0)
                        {
                            PostQuitMessage(0);
                        }
                        break;
                }
            }

            return DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private static int SignedLOWORD(int n)
        {
            return (short)(n & 0xFFFF);
        }

        private static int SignedHIWORD(int n)
        {
            return (short)(n >> 16 & 0xFFFF);
        }

        private static int SignedLOWORD(IntPtr intPtr)
        {
            return SignedLOWORD(IntPtrToInt32(intPtr));
        }

        private static int SignedHIWORD(IntPtr intPtr)
        {
            return SignedHIWORD(IntPtrToInt32(intPtr));
        }

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return (int)intPtr.ToInt64();
        }

        private static Point MakePoint(IntPtr lparam)
        {
            var lp = lparam.ToInt32();
            var x = lp & 0xff;
            var y = (lp >> 16) & 0xff;
            return new Point(x, y);
        }
    }
}
