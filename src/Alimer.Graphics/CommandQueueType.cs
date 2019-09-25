// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
{
    /// <summary>
    /// Defines the type of <see cref="CommandQueue"/>.
    /// </summary>
    public enum CommandQueueType
    {
        /// <summary>
        /// <see cref="CommandQueue"/> that can be used for draw, dispatch, or copy commands.
        /// </summary>
        Graphics,
        /// <summary>
        /// <see cref="CommandQueue"/> that can be used for dispatch or copy commands.
        /// </summary>
        Compute,
        /// <summary>
        /// <see cref="CommandQueue"/> that can be used for copy commands.
        /// </summary>
        Copy
    }
}
