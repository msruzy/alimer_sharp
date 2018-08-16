// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics
{
    public sealed class GraphicsDevice : IDisposable
    {
        private readonly IGraphicsDevice _backend;

        public PresentationParameters PresentationParameters { get; }

        public GraphicsDevice(PresentationParameters presentationParameters)
        {
            Guard.NotNull(presentationParameters, nameof(presentationParameters));

            PresentationParameters = presentationParameters;
            _backend = new D3D11.D3D11GraphicsDevice(presentationParameters, validation: false);
        }

        public void Dispose()
        {
            _backend.Destroy();
        }

        public void Present()
        {
            _backend.Present();
        }
    }
}
