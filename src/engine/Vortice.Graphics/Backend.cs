// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Vortice.Graphics
{
    public interface IGraphicsDeviceFactory
    {
        bool Validation { get; }

        List<GraphicsAdapter> Adapters { get; }

        void Destroy();

        GraphicsDevice CreateGraphicsDevice(GraphicsAdapter adapter, PresentationParameters presentationParameters);
    }
}
