// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics.Tests
{
    public abstract class GraphicsDeviceTestBase : IDisposable
    {
        protected readonly GraphicsDeviceFactory _factory;
        protected readonly GraphicsDevice _graphicsDevice;

        protected GraphicsDeviceTestBase(GraphicsBackend backend = GraphicsBackend.Default, bool createDevice = true, bool enableValidation = false)
        {
            //if (GraphicsDeviceFactory.IsSupported(backend))
            //{
            //    _factory = new GraphicsDeviceFactory(backend, enableValidation);
            //}
            //else
            {
                throw new GraphicsException();
            }

            //if (createDevice)
            //{
            //    _graphicsDevice = _factory.CreateGraphicsDevice(SelectedAdapter());
            //}
        }

        public virtual void Dispose()
        {
            _graphicsDevice?.Dispose();
            _factory.Dispose();
        }

        protected virtual GraphicsAdapter SelectedAdapter() => _factory.DefaultAdapter;
    }
}
