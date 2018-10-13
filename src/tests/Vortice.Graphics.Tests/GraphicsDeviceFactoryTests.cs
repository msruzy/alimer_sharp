// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Xunit;

namespace Vortice.Graphics.Tests
{
    public abstract class GraphicsDeviceFactoryTests : GraphicsDeviceTestBase
    {
        public GraphicsDeviceFactoryTests(GraphicsBackend backend)
            : base(backend, createDevice: false)
        {

        }

        [Fact]
        public void FactoryHasDefaultAdapter()
        {
            Assert.NotNull(_factory.DefaultAdapter);
            Assert.Equal(_factory.DefaultAdapter, _factory.Adapters[0]);
            Assert.True(_factory.Adapters.Count > 0);
        }
    }

#if TEST_D3D11
    public class DirectX11GpuFactoryTests : GraphicsDeviceFactoryTests
    {
        public DirectX11GpuFactoryTests() : base(GraphicsBackend.Direct3D11) { }
    }
#endif

#if TEST_D3D12
    public class DirectX12GpuFactoryTests : GraphicsDeviceFactoryTests
    {
        public DirectX12GpuFactoryTests() : base(GraphicsBackend.Direct3D12) { }
    }
#endif

#if TEST_VULKAN
    public class VulkanGpuFactoryTests : GraphicsDeviceFactoryTests
    {
        public VulkanGpuFactoryTests() : base(GraphicsBackend.Vulkan) { }
    }
#endif
}
