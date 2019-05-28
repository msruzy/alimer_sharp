using System;
using System.Windows;
using Vortice.Presentation.Modules.Shell.ViewModels;

namespace Vortice.Presentation.Modules.Shell.Views
{
	public partial class ShellView
    {
        public ShellView()
		{
			InitializeComponent();
		}

        public void UpdateFloatingWindows()
        {
            var mainWindow = Window.GetWindow(this);
            var mainWindowIcon = (mainWindow != null) ? mainWindow.Icon : null;
            var showFloatingWindowsInTaskbar = ((ShellViewModel)DataContext).ShowFloatingWindowsInTaskbar;
            foreach (var window in Manager.FloatingWindows)
            {
                window.Icon = mainWindowIcon;
                window.ShowInTaskbar = showFloatingWindowsInTaskbar;
            }
        }

        private void OnManagerLayoutUpdated(object sender, EventArgs e)
        {
            UpdateFloatingWindows();
        }
    }
}
