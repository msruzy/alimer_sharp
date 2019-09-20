// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Vortice
{
    public abstract class Application
    {
        private static Application s_current;

        /// <summary>
        /// Gets the current instance of the <see cref="Application"/> class.
        /// </summary>
        /// <value>
        /// The current instance of the <see cref="Application"/> class.
        /// </value>
        public static Application Current
        {
            get => s_current;
            internal set => s_current = value;
        }

        public IServiceProvider Services { get; }

        protected Application()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            //Context.ConfigureServices(services);

            services.AddSingleton<Application>(this);
            //services.AddSingleton<IContentManager, ContentManager>();
        }
    }
}
