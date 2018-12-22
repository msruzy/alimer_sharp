// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12Framebuffer : GPUFramebuffer
    {
        public readonly D3D12GraphicsDevice Device;
        private readonly DescriptorHandle _rtvHandle = default;
        private readonly DescriptorHandle _dsvHandle = default;

        public D3D12Framebuffer(D3D12GraphicsDevice device, FramebufferAttachment[] colorAttachments)
        {
            Device = device;

            if (colorAttachments.Length > 0)
            {
                _rtvHandle = device.AllocateDescriptor(DescriptorHeapType.RenderTargetView, colorAttachments.Length);
                for (var i = 0; i < colorAttachments.Length; i++)
                {
                    var d3dTexture = ((TextureD3D12)colorAttachments[i].Texture).Resource;

                    var renderTargetViewDesc = new RenderTargetViewDescription
                    {
                        Dimension = RenderTargetViewDimension.Texture2D,
                        Format = ((TextureD3D12)colorAttachments[i].Texture).DXGIFormat
                    };

                    var rtvHandle = _rtvHandle.GetCpuHandle(i);
                    device.D3DDevice.CreateRenderTargetView(d3dTexture, renderTargetViewDesc, rtvHandle);
                }
            }
        }

        public override void Destroy()
        {
        }
    }
}
