// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics.Tests
{
    public abstract class GraphicsDeviceTestBase : IDisposable
    {
        protected readonly GraphicsDevice _graphicsDevice;

        protected GraphicsDeviceTestBase(GraphicsBackend backend = GraphicsBackend.Default, bool createDevice = true, bool enableValidation = false)
        {
            if (!GraphicsDevice.IsSupported(backend))
            {
                throw new GraphicsException($"Backend {backend} is not supported");
            }

            //if (createDevice)
            //{
            //    _graphicsDevice = _factory.CreateGraphicsDevice(SelectedAdapter());
            //}
        }

        public virtual void Dispose()
        {
            _graphicsDevice?.Dispose();
        }

        protected virtual GraphicsAdapter SelectedAdapter() => _factory.DefaultAdapter;
    }
}
