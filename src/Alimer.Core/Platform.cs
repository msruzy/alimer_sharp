// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer
{
    /// <summary>
    /// Class describing the current running platform.
    /// </summary>
    public static partial class Platform
    {
        public static IPlatform Current { get; set; } = new StandardPlatform();

        /// <summary>
        /// Gets the running platform type.
        /// </summary>
        public static PlatformType PlatformType => Current.PlatformType;

        /// <summary>
        /// Gets the running platform family.
        /// </summary>
        public static PlatformFamily PlatformFamily => Current.PlatformFamily;

        /// <summary>
		/// Gets the running framework description.
		/// </summary>
		public static string FrameworkDescription => Current.FrameworkDescription;

        /// <summary>
		/// Get the operating system description.
		/// </summary>
		public static string OSDescription => Current.OSDescription;

        /// <summary>
		/// Returns the default directory name where the current application runs.
		/// </summary>
		public static string DefaultAppDirectory => Current.DefaultAppDirectory;
    }
}
