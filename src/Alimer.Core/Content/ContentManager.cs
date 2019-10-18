// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Alimer.Content
{
    /// <summary>
	/// Interface for runtime content loading.
	/// </summary>
    public class ContentManager : IContentManager
    {
        /// <inheritdoc/>
        public IServiceProvider Services { get; }

        public ContentManager(IServiceProvider services)
        {
            Services = services;
        }
    }
}
