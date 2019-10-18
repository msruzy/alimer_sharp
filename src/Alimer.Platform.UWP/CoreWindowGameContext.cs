// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
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

        /// <inheritdoc/>
        public override GameWindow GameWindow { get; }

        public CoreWindowGameContext(CoreWindow? control = null)
        {
            Control = control ?? CoreWindow.GetForCurrentThread();
            GameWindow = new CoreWindowGameWindow(Control);
        }

        public override bool Run(Action loadAction, Action tickAction)
        {
            loadAction();

            while (!GameWindow.IsExiting)
            {
                Control.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);
                tickAction();
            }

            return false;
        }
    }
}
