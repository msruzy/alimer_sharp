// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using Vortice.Presentation.Modules.Shell.Views;

namespace Vortice.Presentation.Modules.Shell.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : /*Conductor<IDocument>.Collection.OneActive,*/ Screen, IShell, IPartImportsSatisfiedNotification
    {
        #region Fields
        private WindowState _windowState;
        private double _width = 1280.0;
        private double _height = 720;
        private string _title = "Vortice"; // Resources.MainWindowDefaultTitle;
        private ImageSource _icon;
        private bool _showFloatingWindowsInTaskbar;
        private ShellView _shellView;
        #endregion

        #region Properties
        public WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                _windowState = value;
                NotifyOfPropertyChange(() => WindowState);
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                NotifyOfPropertyChange(() => Width);
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                NotifyOfPropertyChange(() => Height);
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        public ImageSource Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                NotifyOfPropertyChange(() => Icon);
            }
        }

        public bool ShowFloatingWindowsInTaskbar
        {
            get { return _showFloatingWindowsInTaskbar; }
            set
            {
                _showFloatingWindowsInTaskbar = value;
                NotifyOfPropertyChange(() => ShowFloatingWindowsInTaskbar);
                _shellView?.UpdateFloatingWindows();
            }
        }
        #endregion

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            // if (_icon == null)
            //    _icon = _resourceManager.GetBitmap("Resources/Icons/Vortice-32.png");
        }

        protected override void OnViewLoaded(object view)
        {
            _shellView = (ShellView)view;
            //_commandKeyGestureService.BindKeyGestures((UIElement)view);
            base.OnViewLoaded(view);
        }
    }
}
