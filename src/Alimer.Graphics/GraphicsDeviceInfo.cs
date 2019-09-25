// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
{
    /// <summary>
    /// Describes information of <see cref="GraphicsDevice"/>.
    /// </summary>
    public sealed class GraphicsDeviceInfo
    {
        /// <summary>
        /// Gets or sets the backend type.
        /// </summary>
        public GraphicsBackend Backend { get; set; }

        /// <summary>
        /// Gets or sets the rendering API name.
        /// </summary>
        public string BackendName { get; set; }

        /// <summary>
        /// Get the hardware gpu device vendor name.
        /// </summary>
        public string VendorName
        {
            get
            {
                switch (VendorId)
                {
                    case 0x1002:
                        return "Advanced Micro Devices, Inc.";
                    case 0x10de:
                        return "NVIDIA Corporation";
                    case 0x102b:
                        return "Matrox Electronic Systems Ltd.";
                    case 0x1414:
                        return "Microsoft Corporation";
                    case 0x5333:
                        return "S3 Graphics Co., Ltd.";
                    case 0x8086:
                        return "Intel Corporation";
                    case 0x80ee:
                        return "Oracle Corporation";
                    case 0x15ad:
                        return "VMware Inc.";
                }
                return "";
            }
        }

        /// <summary>
        /// Gets or sets the hardware gpu device vendor id.
        /// </summary>
        public uint VendorId { get; set; }

        /// <summary>
        /// Gets or sets the hardware gpu device name.
        /// </summary>
        public string DeviceName { get; set; }
    }
}
