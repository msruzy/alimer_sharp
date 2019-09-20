// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Handle for creating <see cref="SwapChain"/>.
    /// </summary>
    public abstract class SwapChainHandle
    {
        protected SwapChainHandle() { }

        /// <summary>
        /// Creates a new <see cref="SwapChainHandle"/> from Win32 window.
        /// </summary>
        /// <param name="hInstance">The Win32 instance handle.</param>
        /// <param name="hWnd">The Win32 window handle.</param>
        /// <returns>A new <see cref="SwapChainHandle"/> instance.</returns>
        public static SwapChainHandle CreateWin32(IntPtr hInstance, IntPtr hWnd) => new Win32SwapChainHandle(hInstance, hWnd);

        /// <summary>
        /// Creates a new <see cref="SwapChainHandle"/> from UWP CoreWindow.
        /// </summary>
        /// <param name="coreWindow">The UWP CoreWindow instance.</param>
        /// <returns>A new <see cref="SwapChainHandle"/> instance.</returns>
        public static SwapChainHandle CreateUWPCoreWindow(object coreWindow) => new UWPCoreWindowSwapChainHandle(coreWindow);
    }

    public sealed class Win32SwapChainHandle : SwapChainHandle
    {
        public IntPtr HInstance { get; }
        public IntPtr HWnd { get; }

        internal Win32SwapChainHandle(IntPtr hInstance, IntPtr hWnd)
        {
            Guard.IsTrue(hInstance != IntPtr.Zero, nameof(hInstance), "Invalid hInstance handle");
            Guard.IsTrue(hWnd != IntPtr.Zero, nameof(hWnd), "Invalid hWnd handle");

            HInstance = hInstance;
            HWnd = hWnd;
        }
    }

    public sealed class UWPCoreWindowSwapChainHandle : SwapChainHandle
    {
        public object CoreWindow { get; }

        public UWPCoreWindowSwapChainHandle(object coreWindow)
        {
            Guard.NotNull(coreWindow, nameof(coreWindow));

            CoreWindow = coreWindow;
        }
    }
}
