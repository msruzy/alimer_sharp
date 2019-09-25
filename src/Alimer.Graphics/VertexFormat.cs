// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Alimer.Graphics
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
        /// Two unsigned 8-bit values.
        /// </summary>
        UChar2,

        /// <summary>
        /// Four unsigned 8-bit values.
        /// </summary>
        UChar4,

        /// <summary>
        /// Two signed 8-bit values.
        /// </summary>
        Char2,

        /// <summary>
        /// Four signed 8-bit values.
        /// </summary>
        Char4,

        /// <summary>
        /// Two unsigned normalized 8-bit values.
        /// </summary>
        UChar2Norm,

        /// <summary>
        /// Four unsigned normalized 8-bit values.
        /// </summary>
        UChar4Norm,

        /// <summary>
        /// Two signed normalized 8-bit values.
        /// </summary>
        Char2Norm,

        /// <summary>
        /// Four signed normalized 8-bit values.
        /// </summary>
        Char4Norm,

        /// <summary>
        /// Two unsigned 16-bit values.
        /// </summary>
        UShort2,

        /// <summary>
        /// Four unsigned 16-bit values.
        /// </summary>
        UShort4,

        /// <summary>
        /// Two signed 16-bit values.
        /// </summary>
        Short2,

        /// <summary>
        /// Four signed 16-bit values.
        /// </summary>
        Short4,

        /// <summary>
        /// Two unsigned normalized 16-bit values.
        /// </summary>
        UShort2Norm,

        /// <summary>
        /// Four unsigned normalized 16-bit values.
        /// </summary>
        UShort4Norm,

        /// <summary>
        /// Two signed normalized 16-bit values.
        /// </summary>
        Short2Norm,

        /// <summary>
        /// Four signed normalized 16-bit values.
        /// </summary>
        Short4Norm,

        /// <summary>
        /// One half-precision floating-point value..
        /// </summary>
        Half,

        /// <summary>
        /// Two half-precision floating-point values.
        /// </summary>
        Half2,

        /// <summary>
        /// Four half-precision floating-point values.
        /// </summary>
        Half4,

        /// <summary>
        /// One single-precision floating-point value.
        /// </summary>
        Float,

        /// <summary>
        /// Two single-precision floating-point values.
        /// </summary>
        Float2,

        /// <summary>
        /// Three single-precision floating-point values.
        /// </summary>
        Float3,

        /// <summary>
        /// Four single-precision floating-point values.
        /// </summary>
        Float4,

        /// <summary>
        /// One unsigned 32-bit value.
        /// </summary>
        UInt,

        /// <summary>
        /// Two unsigned 32-bit values.
        /// </summary>
        UInt2,

        /// <summary>
        /// Three unsigned 32-bit values.
        /// </summary>
        UInt3,

        /// <summary>
        /// Four unsigned 32-bit values.
        /// </summary>
        UInt4,

        /// <summary>
        /// One signed 32-bit value.
        /// </summary>
        Int,

        /// <summary>
        /// Two signed 32-bit values.
        /// </summary>
        Int2,

        /// <summary>
        /// Three signed 32-bit values.
        /// </summary>
        Int3,

        /// <summary>
        /// Four signed 32-bit values.
        /// </summary>
        Int4,

        /// <summary>
        /// One packed 32-bit value with four normalized unsigned integer values, arranged as 10 bits, 10 bits, 10 bits, and 2 bits.
        /// </summary>
        UInt1010102Norm
    }
}
