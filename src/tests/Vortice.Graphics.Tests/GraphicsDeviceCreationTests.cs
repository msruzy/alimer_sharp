// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Xunit;

namespace Vortice.Graphics.Tests
{
    public abstract class GraphicsDeviceCreationTests : GraphicsDeviceTestBase
    {
        protected GraphicsDeviceCreationTests(
            GraphicsBackend backend = GraphicsBackend.Default,
            bool validation = false,
            PresentationParameters presentationParameters = null)
            : base(backend, validation, presentationParameters)
        {
        }

        [Fact]
        public void FactoryHasDefaultAdapter()
        {
            Assert.NotNull(_graphicsDevice.Features);
            Assert.NotEqual(0, _graphicsDevice.Features.VendorId);
        }
    }

#if TEST_D3D11
    public class D3D11GpuFactoryTests : GraphicsDeviceCreationTests
    {
        public D3D11GpuFactoryTests() : base(GraphicsBackend.Direct3D11) { }
    }
#endif

#if TEST_D3D12
    public class D312GpuFactoryTests : GraphicsDeviceCreationTests
    {
        public D312GpuFactoryTests() : base(GraphicsBackend.Direct3D12) { }
    }
#endif

#if TEST_VULKAN
    public class VulkanGpuFactoryTests : GraphicsDeviceFactoryTests
    {
        public VulkanGpuFactoryTests() : base(GraphicsBackend.Vulkan) { }
    }
#endif
}
