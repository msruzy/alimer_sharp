// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.DirectX12
{
    internal class DirectX12GraphicsDevice : GraphicsDevice
    {
        private static readonly FeatureLevel[] s_featureLevels = new FeatureLevel[]
        {
            FeatureLevel.Level_12_1,
            FeatureLevel.Level_12_0,
            FeatureLevel.Level_11_1,
            FeatureLevel.Level_11_0,
        };

        private readonly DirectX12GraphicsDeviceFactory _factory;
        public readonly Device Device;
        public readonly FeatureLevel FeatureLevel;
        public readonly int RTVDescriptorSize;
        public readonly int DSVDescriptorSize;

        private readonly DirectX12SwapChain _swapChain;

        /// <inheritdoc/>
        public override Texture BackbufferTexture => _swapChain.BackbufferTexture;

        /// <inheritdoc/>
        public override CommandBuffer ImmediateCommandBuffer { get; }

        public CommandQueue DirectCommandQueue { get; }

        public DirectX12GraphicsDevice(
            DirectX12GraphicsDeviceFactory factory,
            DirectX12GraphicsAdapter adapter,
            PresentationParameters presentationParameters)
            : base(adapter, presentationParameters)
        {
            _factory = factory;
            try
            {
                Device = new Device(adapter.Adapter, FeatureLevel.Level_11_0);
            }
            catch (SharpDXException)
            {
                // Create the Direct3D 12 with WARP adapter.
                var warpAdapter = factory.DXGIFactory.GetWarpAdapter();
                Device = new Device(warpAdapter, FeatureLevel.Level_11_0);
            }

            // Init capabilities.
            unsafe
            {
                fixed (FeatureLevel* levelsPtr = &s_featureLevels[0])
                {
                    var featureLevels = new FeatureDataFeatureLevels
                    {
                        FeatureLevelCount = 4,
                        FeatureLevelsRequestedPointer = new IntPtr(levelsPtr)
                    };

                    Debug.Assert(Device.CheckFeatureSupport(Feature.FeatureLevels, ref featureLevels));
                    FeatureLevel = featureLevels.MaxSupportedFeatureLevel;
                }

                var options = Device.D3D12Options;
            }

            var featureDataRootSignature = new FeatureDataRootSignature
            {
                HighestVersion = RootSignatureVersion.Version11
            };

            if (!Device.CheckFeatureSupport(Feature.RootSignature, ref featureDataRootSignature))
            {
                featureDataRootSignature.HighestVersion = RootSignatureVersion.Version10;
            }

            var gpuVaSupport = Device.GpuVirtualAddressSupport;

            // Create command list manager.
            //CommandListManager = new D3D12CommandListManager(this);

            // Get descriptor heaps size.
            RTVDescriptorSize = Device.GetDescriptorHandleIncrementSize(DescriptorHeapType.RenderTargetView);
            DSVDescriptorSize = Device.GetDescriptorHandleIncrementSize(DescriptorHeapType.DepthStencilView);

            // Create main swap chain.
            _swapChain = new DirectX12SwapChain(_factory.DXGIFactory, this, presentationParameters);
        }

        protected override void Destroy()
        {
            //WaitIdle();
            //CommandListManager.Dispose();
            _swapChain.Destroy();

            if (_factory.Validation)
            {
                using (var deviceDebug = Device.QueryInterface<DebugDevice>())
                {
                    deviceDebug.ReportLiveDeviceObjects(ReportingLevel.Detail);
                }
            }

            Device.Dispose();
        }

        //public override void WaitIdle()
        //{
            //CommandListManager.WaitIdle();
        //}

        protected override void PresentCore()
        {
            _swapChain.Present();
        }

        protected override GraphicsBuffer CreateBufferCore(in BufferDescriptor descriptor, IntPtr initialData)
        {
            return new DirectX12Buffer(this, descriptor, initialData);
        }

        protected override Texture CreateTextureCore(in TextureDescription description)
        {
            return new DirectX12Texture(this, description, nativeTexture: null);
        }
    }
}
