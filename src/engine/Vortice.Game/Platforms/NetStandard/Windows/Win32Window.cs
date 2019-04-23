// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDXGI;
using Vortice.Graphics;
using Vortice.Mathematics;
using static Vortice.Windows.User32;

namespace Vortice.Windows
{
    internal class Win32Window : View
    {
        private const int CW_USEDEFAULT = unchecked((int)0x80000000);
        private const float ContentScale = 1.0f;

        private readonly WindowsApplicationHost _host;
        private IntPtr _hwnd;

        public IntPtr HWnd => _hwnd;

        /// <inheritdoc/>
        public override bool IsMinimized => IsIconic(_hwnd);

        /// <inheritdoc/>
        public override RectF Bounds
        {
            get

            {
                GetClientRect(_hwnd, out var rect);
                return RectF.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
        }

        public Win32Window(WindowsApplicationHost host, string title, int width, int height)
            : base(title)
        {
            _title = title;
            _host = host;
            //const bool fullscreen = false;
            var resizable = true;

            var x = 0;
            var y = 0;
            WindowStyles style = 0;
            WindowExStyles styleEx = 0;

            // Setup the screen settings depending on whether it is running in full screen or in windowed mode.
            //if (fullscreen)
            //{
            //style = User32.WindowStyles.WS_POPUP | User32.WindowStyles.WS_VISIBLE;
            //styleEx = User32.WindowStyles.WS_EX_APPWINDOW;

            //width = screenWidth;
            //height = screenHeight;
            //}
            //else
            {
                if (width > 0 && height > 0)
                {
                    var screenWidth = GetSystemMetrics(SystemMetrics.SM_CXSCREEN);
                    var screenHeight = GetSystemMetrics(SystemMetrics.SM_CYSCREEN);

                    // Place the window in the middle of the screen.WS_EX_APPWINDOW
                    x = (screenWidth - width) / 2;
                    y = (screenHeight - height) / 2;
                }

                if (resizable)
                {
                    style = WindowStyles.WS_OVERLAPPEDWINDOW;
                }
                else
                {
                    style = WindowStyles.WS_POPUP | WindowStyles.WS_BORDER | WindowStyles.WS_CAPTION | WindowStyles.WS_SYSMENU;
                }

                styleEx = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;
            }
            style |= WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;

            int windowWidth;
            int windowHeight;

            if (width > 0 && height > 0)
            {
                var rect = new InteropRect(0, 0, (int)(width * ContentScale), (int)(height * ContentScale));

                // Adjust according to window styles
                AdjustWindowRectEx(
                    ref rect,
                    style,
                    false,
                    styleEx);

                windowWidth = rect.Right - rect.Left;
                windowHeight = rect.Bottom - rect.Top;
            }
            else
            {
                x = y = windowWidth = windowHeight = CW_USEDEFAULT;
            }

            _hwnd = CreateWindowEx(
                (int)styleEx,
                WindowsApplicationHost.WndClassName,
                title,
                (int)style,
                x,
                y,
                windowWidth,
                windowHeight,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);

            if (_hwnd == IntPtr.Zero)
            {
                //Log.Error("[Win32] - Failed to create window");
                return;
            }

            _host.RegisterWindow(this);

            ShowWindow(_hwnd, ShowWindowCommand.Normal);

            // Rase and set handle.
            OnHandleCreated(SwapChainHandle.CreateWin32(_hwnd, host.HInstance));
        }

        internal void HandleDestroy()
        {
            Destroy();
        }

        protected override void Destroy()
        {
            if (_hwnd != IntPtr.Zero)
            {
                var destroyHandle = _hwnd;
                _hwnd = IntPtr.Zero;

                //Log.Debug($"[WIN32] - Destroying window: {destroyHandle}");
                DestroyWindow(destroyHandle);
            }
        }

        protected override void SetTitle(string newTitle)
        {
            SetWindowText(_hwnd, newTitle);
        }
    }
}
