// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    public interface IGraphicsDevice
    {
        void Destroy();

        void Present();
    }
}
