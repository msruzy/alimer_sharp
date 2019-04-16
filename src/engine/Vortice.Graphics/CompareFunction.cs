// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics
{
    /// <summary>
    /// Specifies comparison options that should be performed on a depth texture.
    /// </summary>
    public enum CompareFunction
    {
        /// <summary>
        /// A new value never passes the comparison test.
        /// </summary>
        Never,

        /// <summary>
        /// A new value passes the comparison test if it is less than the existing value.
        /// </summary>
        Less,

        /// <summary>
        /// A new value passes the comparison test if it is equal to the existing value.
        /// </summary>
        Equal,

        /// <summary>
        /// A new value passes the comparison test if it is less than or equal to the existing value.
        /// </summary>
        LessEqual,

        /// <summary>
        /// A new value passes the comparison test if it is greater than the existing value.
        /// </summary>
        Greater,

        /// <summary>
        /// A new value passes the comparison test if it is not equal to the existing value.
        /// </summary>
        NotEqual,

        /// <summary>
        /// A new value passes the comparison test if it is greater than or equal to the existing value.
        /// </summary>
        GreaterEqual,

        /// <summary>
        /// A new value always passes the comparison test.
        /// </summary>
        Always
    }
}
