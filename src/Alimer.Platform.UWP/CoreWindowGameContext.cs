// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Core;

namespace Alimer
{
    /// <summary>
    /// Defines a context for <see cref="Game"/> that handles logic using Windows Forms.
    /// </summary>
    public class CoreWindowGameContext : GameContext
    {
        public CoreWindow Control { get; protected set; }

        public CoreWindowGameContext(CoreWindow? control = null)
        {
            Control = control ?? CoreWindow.GetForCurrentThread();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddSingleton<GameWindow>(new CoreWindowGameWindow(Control));
        }
    }
}
