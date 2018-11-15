// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12Framebuffer : IFramebuffer
    {
        public readonly D3D12GraphicsDevice Device;
        private readonly CpuDescriptorHandle[] _rtv;

        public D3D12Framebuffer(D3D12GraphicsDevice device, FramebufferAttachment[] colorAttachments)
        {
            Device = device;
            _rtv = new CpuDescriptorHandle[colorAttachments.Length];
            for (var i = 0; i < colorAttachments.Length; i++)
            {
                _rtv[i] = device.RTVDescriptorHeap.AllocatePersistent().Handles[0];

                var d3dTexture = ((D3D12Texture)colorAttachments[i].Texture).Resource;

                var renderTargetViewDesc = new RenderTargetViewDescription
                {
                     Dimension = RenderTargetViewDimension.Texture2D,
                     Format = ((D3D12Texture)colorAttachments[i].Texture).DXGIFormat
                };

                device.Device.CreateRenderTargetView(d3dTexture, renderTargetViewDesc, _rtv[i]);
            }
        }

        public void Destroy()
        {
            for (var i = 0; i < _rtv.Length; i++)
            {
                Device.RTVDescriptorHeap.FreePersistent(ref _rtv[i]);
            }
        }
    }
}
