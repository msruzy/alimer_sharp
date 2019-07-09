// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using Vortice.DirectX;
using Vortice.DirectX.Direct3D11;
using Vortice.DirectX.DXGI;

namespace Vortice.Graphics.D3D11
{
    internal unsafe class SwapchainD3D11 : SwapChain
    {
        public readonly TextureD3D11 BackbufferTexture;
        private readonly int _syncInterval = 1;
        private readonly PresentFlags _presentFlags;
        private readonly IDXGISwapChain _swapChain;
        private int _currentBackBuffer;

        public SwapchainD3D11(DeviceD3D11 device, in SwapChainDescriptor descriptor)
            : base(device, descriptor)
        {
            var width = Math.Max(descriptor.Width, 1);
            var height = Math.Max(descriptor.Height, 1);

            switch (descriptor.Handle)
            {
                case Win32SwapChainHandle win32Handle:
                    {
                        // Check tearing support.
                        var dxgiFactory5 = device.DXGIFactory.QueryInterfaceOrNull<IDXGIFactory5>();
                        var allowTearing = false;
                        if (dxgiFactory5 != null)
                        {
                            if (dxgiFactory5.PresentAllowTearing)
                            {
                                // Recommended to always use tearing if supported when using a sync interval of 0.
                                _syncInterval = 0;
                                _presentFlags |= PresentFlags.AllowTearing;
                                allowTearing = true;
                            }

                            dxgiFactory5.Dispose();
                        }

                        var dxgiFactory2 = device.DXGIFactory.QueryInterfaceOrNull<IDXGIFactory2>();
                        if (dxgiFactory2 != null)
                        {
                            var swapchainDesc = new SwapChainDescription1()
                            {
                                Width = width,
                                Height = height,
                                Format = BackBufferFormat,
                                Stereo = false,
                                SampleDescription = new SampleDescription(1, 0),
                                Usage = Vortice.DirectX.Usage.RenderTargetOutput,
                                BufferCount = BackBufferCount,
                                Scaling = Scaling.Stretch,
                                SwapEffect = allowTearing ? SwapEffect.FlipDiscard : SwapEffect.Discard,
                                AlphaMode = AlphaMode.Ignore,
                                Flags = allowTearing ? SwapChainFlags.AllowTearing : SwapChainFlags.None,
                            };

                            var fullscreenDescription = new SwapChainFullscreenDescription
                            {
                                Windowed = true
                            };

                            _swapChain = dxgiFactory2.CreateSwapChainForHwnd(
                                device.D3D11Device,
                                win32Handle.HWnd,
                                swapchainDesc,
                                fullscreenDescription);

                            dxgiFactory2.Dispose();
                        }
                        else
                        {
                            SwapChainDescription dxgiSCDesc = new SwapChainDescription
                            {
                                BufferCount = BackBufferCount,
                                IsWindowed = true,
                                BufferDescription = new ModeDescription(width, height, BackBufferFormat),
                                OutputWindow = win32Handle.HWnd,
                                SampleDescription = new SampleDescription(1, 0),
                                SwapEffect = SwapEffect.Discard,
                                Usage = Vortice.DirectX.Usage.Backbuffer | Vortice.DirectX.Usage.RenderTargetOutput
                            };

                            _swapChain = device.DXGIFactory.CreateSwapChain(device.D3D11Device, dxgiSCDesc);
                        }

                        device.DXGIFactory.MakeWindowAssociation(win32Handle.HWnd, WindowAssociationFlags.IgnoreAll);
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

            var backBufferTexture = _swapChain.GetBuffer<ID3D11Texture2D>(0);
            var textureDescriptor = backBufferTexture.Description.FromDirectX();
            BackbufferTexture = new TextureD3D11(device, ref textureDescriptor, backBufferTexture, BackBufferFormat);

            // Configure base.
            Configure(descriptor);
        }

        /// <inheritdoc/>
        public override int BackBufferCount => 2;

        /// <inheritdoc/>
        public override int CurrentBackBuffer => _currentBackBuffer;

        public Format BackBufferFormat { get; } = Format.B8G8R8A8_UNorm;

        /// <inheritdoc/>
        protected override void Destroy()
        {
            BackbufferTexture.Dispose();
            _swapChain.Dispose();
        }

        /// <inheritdoc/>
        protected override Texture GetBackBufferTexture(int index)
        {
            Debug.Assert(index == 0);
            return BackbufferTexture;
        }

        protected override void PresentImpl()
        {
            //var parameters = new SharpDX.DXGI.PresentParameters();

            var result = _swapChain.Present(_syncInterval, _presentFlags);
            _currentBackBuffer = (_currentBackBuffer + 1) % BackBufferCount;
        }
    }
}
