// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using DXGI = SharpDX.DXGI;

namespace Vortice.Graphics
{
    internal abstract class DXGISwapchain : Swapchain
    {
        private readonly int _syncInterval = 1;
        private readonly PresentFlags _presentFlags;

        protected readonly SwapChain _swapChain;
        protected int _currentFrameIndex;
        protected readonly int _frameCount;

        protected unsafe DXGISwapchain(
            GraphicsDevice device,
            PresentationParameters presentationParameters,
            ComObject deviceOrCommandQueue,
            int bufferCount,
            int frameCount) : base(device)
        {
            _frameCount = frameCount;
            var width = Math.Max(presentationParameters.BackBufferWidth, 1);
            var height = Math.Max(presentationParameters.BackBufferHeight, 1);

            switch (presentationParameters.DeviceWindowHandle)
            {
                case IntPtr hwnd:
                    {
                        using (var dxgiDevice = deviceOrCommandQueue.QueryInterface<SharpDX.DXGI.Device>())
                        {
                            using (var dxgiFactory = dxgiDevice.Adapter.GetParent<Factory2>())
                            {
                                // Check tearing support.
                                RawBool allowTearing = false;
                                using (var factory5 = dxgiFactory.QueryInterfaceOrNull<DXGI.Factory5>())
                                {
                                    factory5.CheckFeatureSupport(DXGI.Feature.PresentAllowTearing,
                                        new IntPtr(&allowTearing), sizeof(RawBool)
                                        );

                                    // Recommended to always use tearing if supported when using a sync interval of 0.
                                    _syncInterval = 0;
                                    _presentFlags |= DXGI.PresentFlags.AllowTearing;
                                }

                                var swapchainDesc = new SharpDX.DXGI.SwapChainDescription1()
                                {
                                    Width = width,
                                    Height = height,
                                    Format = Format.B8G8R8A8_UNorm,
                                    Stereo = false,
                                    SampleDescription = new DXGI.SampleDescription(1, 0),
                                    Usage = DXGI.Usage.RenderTargetOutput,
                                    BufferCount = bufferCount,
                                    Scaling = Scaling.Stretch,
                                    SwapEffect = allowTearing ? SwapEffect.FlipDiscard : DXGI.SwapEffect.Discard,
                                    AlphaMode = AlphaMode.Ignore,
                                    Flags = allowTearing ? SwapChainFlags.AllowTearing : DXGI.SwapChainFlags.None,
                                };

                                var fullscreenDescription = new DXGI.SwapChainFullScreenDescription
                                {
                                    Windowed = true
                                };

                                _swapChain = new DXGI.SwapChain1(dxgiFactory,
                                    deviceOrCommandQueue,
                                    hwnd,
                                    ref swapchainDesc,
                                    fullscreenDescription);
                                dxgiFactory.MakeWindowAssociation(hwnd, DXGI.WindowAssociationFlags.IgnoreAll);
                            }
                        }
                    }
                    break;

                    //case CoreWindow coreWindowHandle:
                    //    {
                    //        var coreWindow = coreWindowHandle.CoreWindow;

                    //        var swapchainDesc = new DXGI.SwapChainDescription1()
                    //        {
                    //            Width = width,
                    //            Height = height,
                    //            Format = DXGI.Format.B8G8R8A8_UNorm,
                    //            Stereo = false,
                    //            SampleDescription = new DXGI.SampleDescription(1, 0),
                    //            Usage = DXGI.Usage.RenderTargetOutput,
                    //            BufferCount = FrameCount,
                    //            Scaling = DXGI.Scaling.AspectRatioStretch,
                    //            SwapEffect = DXGI.SwapEffect.FlipDiscard,
                    //            AlphaMode = DXGI.AlphaMode.Ignore,
                    //            Flags = DXGI.SwapChainFlags.None,
                    //        };

                    //        using (var comCoreWindow = new ComObject(coreWindow))
                    //        {
                    //            _swapChain = new DXGI.SwapChain1(
                    //                factory,
                    //                device.D3DDevice,
                    //                comCoreWindow,
                    //                ref swapchainDesc);
                    //        }
                    //    }
                    //    break;
            }
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            _swapChain.Dispose();
        }

        protected override int GetBackbufferIndex() => _currentFrameIndex;

        public void Present()
        {
            //var parameters = new SharpDX.DXGI.PresentParameters();

            try
            {
                _swapChain.Present(_syncInterval, _presentFlags);
                _currentFrameIndex = (_currentFrameIndex + 1) % _frameCount;
            }
            catch (SharpDXException)
            {
            }
        }
    }
}
