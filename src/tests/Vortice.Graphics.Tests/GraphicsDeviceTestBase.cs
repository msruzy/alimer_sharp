// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics.Tests
{
    public abstract class GraphicsDeviceTestBase : IDisposable
    {
        protected readonly GraphicsDevice _graphicsDevice;

        protected GraphicsDeviceTestBase(
            GraphicsBackend backend = GraphicsBackend.Default,
            bool validation = false)
        {
            if (!GraphicsDevice.IsSupported(backend))
            {
                throw new GraphicsException($"Backend {backend} is not supported");
            }

            _graphicsDevice = GraphicsDevice.Create(backend, validation);
        }

        public virtual void Dispose()
        {
            _graphicsDevice?.Dispose();
        }
    }
}
