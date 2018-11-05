// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    /// <summary>
    /// Describes features supported by given instance of <see cref="GraphicsDevice"/>.
    /// </summary>
    public sealed class GraphicsDeviceFeatures
    {
        public int VendorId { get; internal set; }

        public GpuVendor Vendor
        {
            get
            {
                switch (VendorId)
                {
                    case 0x13B5:
                        return GpuVendor.Arm;
                    case 0x10DE:
                        return GpuVendor.Nvidia;
                    case 0x1002:
                    case 0x1022:
                        return GpuVendor.Amd;
                    case 0x8086:
                        return GpuVendor.Intel;
                    default:
                        return GpuVendor.Unknown;
                }
            }
        }

        public int DeviceId { get; internal set; }

        public string DeviceName { get; internal set; }

        /// <summary>
        /// Multithreading capability.
        /// </summary>
        public bool Multithreading { get; internal set; }
    }
}
