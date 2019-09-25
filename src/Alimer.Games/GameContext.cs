// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;

namespace Alimer
{
    /// <summary>
    /// Defines a context for <see cref="Game"/> that handles platform logic.
    /// </summary>
    public abstract class GameContext
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
