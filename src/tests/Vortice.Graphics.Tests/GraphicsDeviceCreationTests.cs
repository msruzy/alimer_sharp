// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Xunit;

namespace Alimer.Graphics.Tests
{
    public abstract class GraphicsDeviceCreationTests : GraphicsDeviceTestBase
    {
        protected GraphicsDeviceCreationTests(GraphicsBackend backend, bool validation = false)
            : base(backend, validation)
        {
        }

        [Fact]
        public void FactoryHasDefaultAdapter()
        {
            Assert.NotEqual(GraphicsBackend.Invalid, _graphicsDevice.Backend);
            Assert.NotEqual(0, _graphicsDevice.Info.VendorId);
        }
    }

    public class D3D11GpuFactoryTests : GraphicsDeviceCreationTests
    {
        public D3D11GpuFactoryTests() : base(GraphicsBackend.Direct3D11) { }
    }

    public class D3D12GpuFactoryTests : GraphicsDeviceCreationTests
    {
        public D3D12GpuFactoryTests() : base(GraphicsBackend.Direct3D12) { }
    }

    //public class VulkanGpuFactoryTests : GraphicsDeviceCreationTests
    //{
    //    public VulkanGpuFactoryTests() : base(GraphicsBackend.Vulkan) { }
    //}
}
