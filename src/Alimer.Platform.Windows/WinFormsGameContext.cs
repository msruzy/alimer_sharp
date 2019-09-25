// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Alimer
{
    /// <summary>
    /// Defines a context for <see cref="Game"/> that handles logic using Windows Forms.
    /// </summary>
    public class WinFormsGameContext : GameContext
    {
        public Control Control { get; protected set; }

        public WinFormsGameContext(Control control)
        {
            Guard.NotNull(control, nameof(control));

            Control = control;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddSingleton<GameWindow>(new WinFormsGameWindow(Control));
        }
    }
}
