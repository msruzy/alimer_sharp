// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Vortice.Direct3D11;

namespace Alimer.Graphics.Direct3D11
{
    internal class TextureViewD3D11 : TextureView, IDisposable
    {
        public readonly DeviceD3D11 Device;
        public readonly ID3D11RenderTargetView RenderTargetView;
        public readonly ID3D11DepthStencilView DepthStencilView;

        public TextureViewD3D11(DeviceD3D11 device, TextureD3D11 texture, TextureViewDescriptor descriptor)
            : base(texture, descriptor)
        {
            Device = device;

            if ((texture.Usage & TextureUsage.RenderTarget) != TextureUsage.None)
            {
                // TODO: Use TextureViewDescriptor
                if (!PixelFormatUtil.IsDepthStencilFormat(descriptor.Format))
                {
                    RenderTargetView = device.D3D11Device.CreateRenderTargetView(texture);
                }
                else
                {
                    DepthStencilView = device.D3D11Device.CreateDepthStencilView(texture);
                }
            }
        }

        public void Dispose()
        {
            RenderTargetView?.Dispose();
            DepthStencilView?.Dispose();
        }
    }
}
