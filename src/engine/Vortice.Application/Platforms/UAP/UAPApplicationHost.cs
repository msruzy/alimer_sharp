// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Windows.ApplicationModel.Core;

namespace Vortice
{
    internal class UAPApplicationHost : ApplicationHost
    {
        private readonly UAPView _mainView;

        public UAPApplicationHost(Application application)
            : base(application)
        {
            _mainView = new UAPView("Vortice");
        }

        public override View MainView => _mainView;

        public override bool IsAsyncLoop => true;

        public override void Run()
        {
            CoreApplication.Run(_mainView);
        }

        public override void Exit()
        {
            CoreApplication.Exit();
        }
    }

    public abstract partial class ApplicationHost
    {
        public static ApplicationHost Create(Application application)
        {
            return new UAPApplicationHost(application);
        }
    }
}
