// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Assets
{
    /// <summary>
    /// Interface for generic asset importer
    /// </summary>
    public interface IAssetImporter
    {
        /// <summary>
        /// Import given object from file name.
        /// </summary>
        /// <param name="fileName">The path to import from.</param>
        /// <returns></returns>
        object Import(string fileName);
    }
}
