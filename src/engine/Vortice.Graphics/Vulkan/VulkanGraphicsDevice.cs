// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice.Graphics.Vulkan
{
    internal class VulkanGraphicsDevice : GraphicsDevice
    {
        private static bool? _isSupported;

        public VulkanGraphicsDevice(bool validation)
            : base(GraphicsBackend.Vulkan)
        {

        }

        protected override void Destroy()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if given Vulkan backend is supported.
        /// </summary>
        /// <returns>True if supported, false otherwise.</returns>
        public static bool IsSupported()
        {
            if (_isSupported.HasValue)
                return _isSupported.Value;

            if (Platform.PlatformType != PlatformType.Windows
                && Platform.PlatformType != PlatformType.Linux
                && Platform.PlatformType != PlatformType.Android)
            {
                _isSupported = false;
                return false;
            }

            try
            {
            }
            catch (Exception)
            {
                _isSupported = false;
                return false;
            }

            _isSupported = true;
            return true;
        }

        public override CommandBuffer ImmediateContext => throw new NotImplementedException();
        public override SwapChain MainSwapchain => throw new NotImplementedException();

        protected override GraphicsBuffer CreateBufferCore(in BufferDescriptor descriptor, IntPtr initialData)
        {
            throw new NotImplementedException();
        }

        protected override Texture CreateTextureCore(in TextureDescription description)
        {
            throw new NotImplementedException();
        }

       
        protected override void FrameCore()
        {
            throw new NotImplementedException();
        }

        protected override void WaitIdleCore()
        {
            throw new NotImplementedException();
        }

        protected override Shader CreateShaderCore(byte[] vertex, byte[] pixel)
        {
            throw new NotImplementedException();
        }

        internal override GPUFramebuffer CreateFramebuffer(FramebufferAttachment[] colorAttachments)
        {
            throw new NotImplementedException();
        }

        internal override GPUSwapChain CreateSwapChain(in SwapChainDescriptor descriptor)
        {
            throw new NotImplementedException();
        }
    }
}
