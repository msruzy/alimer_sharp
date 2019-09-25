// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Vortice.Direct3D11;

namespace Alimer.Graphics.Direct3D11
{
    internal class TextureViewD3D11 : IDisposable
    {
        public readonly DeviceD3D11 Device;
        public readonly TextureD3D11 Resource;
        public readonly ID3D11RenderTargetView RenderTargetView;
        public readonly ID3D11DepthStencilView DepthStencilView;

        public TextureViewD3D11(DeviceD3D11 device, TextureD3D11 resource, TextureViewDescriptor descriptor)
        {
            Device = device;
            Resource = resource;

            if (resource.Usage.HasFlag(TextureUsage.RenderTarget))
            {
                // TODO: Use TextureViewDescriptor
                if (!PixelFormatUtil.IsDepthStencilFormat(resource.Format))
                {
                    RenderTargetView = device.D3D11Device.CreateRenderTargetView(resource.Resource);
                }
                else
                {
                    DepthStencilView = device.D3D11Device.CreateDepthStencilView(resource.Resource);
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
