// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Vortice.Graphics.Direct3D11;
using Vortice.Graphics.Direct3D12;

namespace Vortice.Graphics.Tests
{
    public abstract class GraphicsDeviceTestBase : IDisposable
    {
        protected readonly GraphicsDeviceFactory _graphicsDeviceFactory;
        protected readonly GraphicsDevice _graphicsDevice;

        protected GraphicsDeviceTestBase(GraphicsBackend backend, bool validation = false)
        {
            switch (backend)
            {
                case GraphicsBackend.Direct3D11:
                    if (!D3D11GraphicsDeviceFactory.IsSupported())
                    {
                        throw new GraphicsException($"Backend {backend} is not supported");
                    }

                    _graphicsDeviceFactory = new D3D11GraphicsDeviceFactory(validation);
                    break;

                case GraphicsBackend.Direct3D12:
                    if (!D3D12GraphicsDeviceFactory.IsSupported())
                    {
                        throw new GraphicsException($"Backend {backend} is not supported");
                    }

                    _graphicsDeviceFactory = new D3D12GraphicsDeviceFactory(validation);
                    break;

                default:
                    throw new GraphicsException($"Backend {backend} is not supported");
            }

            _graphicsDevice = _graphicsDeviceFactory.CreateDevice(PowerPreference.Default);
        }

        public virtual void Dispose()
        {
            _graphicsDevice?.Dispose();
        }
    }
}
