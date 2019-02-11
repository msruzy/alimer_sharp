// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Defines format of single vertex attribute.
    /// </summary>
    public enum VertexFormat
    {
        /// <summary>
        /// An invalid vertex format.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// One unsigned 8-bit value.
        /// </summary>
        UChar,

        /// <summary>
        /// Two unsigned 8-bit values.
        /// </summary>
        UChar2,

        /// <summary>
        /// Three unsigned 8-bit values.
        /// </summary>
        UChar3,

        /// <summary>
        /// Four unsigned 8-bit values.
        /// </summary>
        UChar4,
    }
}
