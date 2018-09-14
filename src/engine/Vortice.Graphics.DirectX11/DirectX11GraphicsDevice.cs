// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace Vortice.Graphics.DirectX11
{
    internal unsafe class DirectX11GraphicsDevice : GraphicsDevice
    {
        private static readonly FeatureLevel[] s_featureLevels = new FeatureLevel[]
        {
            FeatureLevel.Level_11_1,
            FeatureLevel.Level_11_0,
            FeatureLevel.Level_10_1,
            FeatureLevel.Level_10_0
        };

        private readonly DirectX11GraphicsDeviceFactory _factory;
        public readonly Device1 Device;
        public readonly DeviceContext1 ImmediateContext;
        private readonly DirectX11SwapChain _swapChain;

        /// <inheritdoc/>
        public override Texture BackbufferTexture => _swapChain.BackbufferTexture;

        /// <inheritdoc/>
        public override CommandBuffer ImmediateCommandBuffer { get; }

        public DirectX11GraphicsDevice(
            DirectX11GraphicsDeviceFactory factory,
            DirectX11GpuAdapter adapter,
            PresentationParameters presentationParameters)
            : base(adapter, presentationParameters)
        {
            _factory = factory;
            var creationFlags = DeviceCreationFlags.BgraSupport/* | DeviceCreationFlags.VideoSupport*/;

            if (_factory.Validation)
            {
                creationFlags |= DeviceCreationFlags.Debug;
            }

            try
            {
                using (var device = new Device(adapter.Adapter, creationFlags, s_featureLevels))
                {
                    if (DirectX11Utils.IsWindows10x)
                    {
                        Device = device.QueryInterface<Device5>();
                    }
                    else
                    {
                        Device = device.QueryInterface<Device1>();
                    }
                }
            }
            catch (SharpDXException)
            {
                // Remove debug flag not being supported.
                creationFlags &= ~DeviceCreationFlags.Debug;

                using (var device = new Device(adapter.Adapter, creationFlags, s_featureLevels))
                {
                    if (DirectX11Utils.IsWindows10x)
                    {
                        Device = device.QueryInterface<Device5>();
                    }
                    else
                    {
                        Device = device.QueryInterface<Device1>();
                    }
                }
            }

            ImmediateContext = Device.ImmediateContext1;
            ImmediateCommandBuffer = new DirectX11CommandBuffer(this, ImmediateContext);
            _swapChain = new DirectX11SwapChain(this, presentationParameters);
        }

        protected override void Destroy()
        {
            _swapChain.Destroy();
            ImmediateContext.Dispose();
            Device.Dispose();
        }

        protected override void PresentCore()
        {
            _swapChain.Present();
        }

        protected override GraphicsBuffer CreateBufferCore(in BufferDescriptor descriptor, IntPtr initialData)
        {
            return new DirectX11Buffer(this, descriptor, initialData);
        }

        protected override Texture CreateTextureCore(in TextureDescription description)
        {
            return new DirectX11Texture(this, description, nativeTexture: null);
        }
    }
}
