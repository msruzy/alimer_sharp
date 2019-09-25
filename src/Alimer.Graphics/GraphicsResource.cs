﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Base class for all graphics resources.
    /// </summary>
    public abstract class GraphicsResource : DisposableBase
    {
        /// <summary>
        /// Gets the creation <see cref="GraphicsDevice"/>.
        /// </summary>
        public GraphicsDevice Device { get; }

        /// <summary>
        /// Gets the type of <see cref="GraphicsResourceType"/>.
        /// </summary>
        public GraphicsResourceType ResourceType { get; }

        /// <summary>
        /// Gets the <see cref="GraphicsResourceUsage"/>.
        /// </summary>
        public GraphicsResourceUsage ResourceUsage { get; }

        /// <summary>
        /// Create a new instance of <see cref="GraphicsResource"/> class.
        /// </summary>
        /// <param name="device">The creation <see cref="GraphicsDevice"/></param>
        /// <param name="resourceType">The <see cref="GraphicsResourceType"/></param>
        /// <param name="resourceUsage">The <see cref="GraphicsResourceUsage"/></param>
        protected GraphicsResource(
            GraphicsDevice device,
            GraphicsResourceType resourceType,
            GraphicsResourceUsage resourceUsage)
        {
            Guard.NotNull(device, nameof(device), $"{nameof(GraphicsDevice)} cannot be null");

            Device = device;
            ResourceType = resourceType;
            ResourceUsage = resourceUsage;
            device.TrackResource(this);
        }

        /// <inheritdoc/>
		protected sealed override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                Device.UntrackResource(this);
                Destroy();
            }
        }

        /// <summary>
        /// Unconditionally destroy this resource.
        /// </summary>
        protected abstract void Destroy();
    }
}
