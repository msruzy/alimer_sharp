// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice
{
    /// <summary>
    /// Base class for initializing platform-specific services for an <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="TAppBuilder">The type of the AppBuilder class itself.</typeparam>
    public abstract class AppBuilderBase<TAppBuilder> where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
    {
        public delegate void AppMainDelegate(Application app, string[] args);

        /// <summary>
        /// Begin configuring an <see cref="Application"/>.
        /// </summary>
        /// <typeparam name="TApp">The subclass of <see cref="Application"/> to configure.</typeparam>
        /// <returns>An <typeparamref name="TAppBuilder"/> instance.</returns>
        public static TAppBuilder Configure<TApp>() where TApp : Application, new()
        {
            return Configure(new TApp());
        }

        /// <summary>
        /// Begin configuring an <see cref="Application"/>.
        /// </summary>
        /// <returns>An <typeparamref name="TAppBuilder"/> instance.</returns>
        public static TAppBuilder Configure(Application application)
        {
            Application.Current = application;

            return new TAppBuilder();
        }

        public void Start(AppMainDelegate main, string[] args)
        {
            Setup();
            main(Application.Current, args);
        }

        /// <summary>
        /// Sets up the platform-speciic services for the <see cref="Application"/>.
        /// </summary>
        private void Setup()
        {
            if (Application.Current == null)
            {
                throw new InvalidOperationException("No Application has been configured.");
            }
        }
    }
}
