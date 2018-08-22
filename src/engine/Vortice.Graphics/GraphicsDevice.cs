// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    public abstract class GraphicsDevice : IDisposable
    {
        public GraphicsAdapter Adapter { get; }

        public PresentationParameters PresentationParameters { get; }

        protected GraphicsDevice(GraphicsAdapter adapter, PresentationParameters presentationParameters)
        {
            Guard.NotNull(adapter, nameof(adapter));
            Guard.NotNull(presentationParameters, nameof(presentationParameters));

            Adapter = adapter;
            PresentationParameters = presentationParameters;
        }

        public void Dispose()
        {
            Destroy();
        }

        public void Present()
        {
            PresentCore();
        }

        protected abstract void Destroy();
        protected abstract void PresentCore();
    }
}
