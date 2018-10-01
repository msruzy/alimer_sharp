// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Graphics.Vulkan
{
    internal class VulkanGraphicsDeviceFactory : GraphicsDeviceFactory
    {
        public VulkanGraphicsDeviceFactory(bool validation)
            : base(GraphicsBackend.Vulkan, validation)
        {

        }

        protected override void Destroy()
        {
        }

        protected override GraphicsDevice CreateGraphicsDeviceImpl(GraphicsAdapter adapter, PresentationParameters presentationParameters)
        {
            return null;
        }
    }
}
