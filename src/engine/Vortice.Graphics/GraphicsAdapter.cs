// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines a physical graphics adapter.
    /// </summary>
    public class GraphicsAdapter
    {
        public uint DeviceId { get; protected set; }

        public string DeviceName { get; protected set; }

        protected GraphicsAdapter()
        {
        }
    }
}
