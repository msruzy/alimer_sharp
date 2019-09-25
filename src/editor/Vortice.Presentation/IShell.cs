// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;

namespace Vortice.Presentation
{
    public interface IShell : IScreen
    {
        WindowState WindowState { get; set; }
        double Width { get; set; }
        double Height { get; set; }

        string Title { get; set; }
        ImageSource Icon { get; set; }

        bool ShowFloatingWindowsInTaskbar { get; set; }
    }

    /// <summary>
    /// Delegate invoked at startup and before showing main <see cref="IShell"/> window.
    /// </summary>
    public delegate void StartupTask();
}
