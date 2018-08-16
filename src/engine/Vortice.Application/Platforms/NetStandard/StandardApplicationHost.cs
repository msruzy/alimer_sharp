// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace Vortice
{
    internal class StandardApplicationHost : ApplicationHost
    {
        public override bool IsAsyncLoop => throw new System.NotImplementedException();

        public override View MainView => throw new System.NotImplementedException();

        //private readonly UAPView _mainView;

        public StandardApplicationHost(Application application)
            : base(application)
        {
            //_mainView = new UAPView("Alimer");
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override void Run()
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract partial class ApplicationHost
    {
        public static ApplicationHost Create(Application application)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new Windows.WindowsApplicationHost(application);
            }

            return new StandardApplicationHost(application);
        }
    }
}
