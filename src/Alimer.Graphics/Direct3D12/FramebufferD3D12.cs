// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

#if TODO
using Vortice.DirectX.Direct3D12;
namespace Alimer.Graphics.Direct3D12
{
    internal class FramebufferD3D12 : Framebuffer
    {
        private readonly DescriptorHandle _rtvHandle;
        private readonly DescriptorHandle _dsvHandle;

        public FramebufferD3D12(DeviceD3D12 device, FramebufferAttachment[] colorAttachments, FramebufferAttachment? depthStencilAttachment)
            : base(device, colorAttachments, depthStencilAttachment)
        {
            if (colorAttachments.Length > 0)
            {
                _rtvHandle = device.AllocateDescriptor(DescriptorHeapType.RenderTargetView, colorAttachments.Length);
                for (var i = 0; i < colorAttachments.Length; i++)
                {
                    var d3dTexture = ((TextureD3D12)colorAttachments[i].Texture).Resource;

                    var renderTargetViewDesc = new RenderTargetViewDescription
                    {
                        ViewDimension = RenderTargetViewDimension.Texture2D,
                        Format = ((TextureD3D12)colorAttachments[i].Texture).DXGIFormat
                    };

                    var rtvHandle = _rtvHandle.GetCpuHandle(i);
                    device.D3DDevice.CreateRenderTargetView(d3dTexture, renderTargetViewDesc, rtvHandle);
                }
            }
        }

        protected override void Destroy()
        {
        }
    }
}

#endif
