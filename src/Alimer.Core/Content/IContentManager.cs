// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Alimer.Content
{
    /// <summary>
	/// Interface for runtime content loading.
	/// </summary>
    public interface IContentManager
    {
        /// <summary>
        /// Gets the service provider associated with the ContentManager.
        /// </summary>
        IServiceProvider Services { get; }
    }
}
