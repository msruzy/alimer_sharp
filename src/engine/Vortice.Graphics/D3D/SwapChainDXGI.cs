﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using DXGI = SharpDX.DXGI;

namespace Vortice.Graphics
{
    internal abstract class SwapChainDXGI : SwapChain
    {
        private readonly int _syncInterval = 1;
        private readonly PresentFlags _presentFlags;
        protected readonly DXGI.SwapChain _swapChain;
        protected int _currentBackBuffer;

        protected unsafe SwapChainDXGI(
            GraphicsDevice device,
            SwapChainDescriptor descriptor,
            Factory1 dxgiFactory,
            ComObject deviceOrCommandQueue,
            int bufferCount,
            int backBufferCount)
            : base(device, descriptor)
        {
            BackBufferCount = backBufferCount;
            var width = Math.Max(descriptor.Width, 1);
            var height = Math.Max(descriptor.Height, 1);

            switch (descriptor.Handle)
            {
                case Win32SwapChainHandle win32Handle:
                    {
                        // Check tearing support.
                        RawBool allowTearing = false;
                        var dxgiFactory5 = dxgiFactory.QueryInterfaceOrNull<Factory5>();
                        if (dxgiFactory5 != null)
                        {
                            dxgiFactory5.CheckFeatureSupport(Feature.PresentAllowTearing, new IntPtr(&allowTearing), sizeof(RawBool));
                            if (allowTearing)
                            {
                                // Recommended to always use tearing if supported when using a sync interval of 0.
                                _syncInterval = 0;
                                _presentFlags |= PresentFlags.AllowTearing;
                                allowTearing = true;
                            }

                            dxgiFactory5.Dispose();
                        }

                        var dxgiFactory2 = dxgiFactory.QueryInterfaceOrNull<Factory2>();
                        if (dxgiFactory2 != null)
                        {
                            var swapchainDesc = new SwapChainDescription1()
                            {
                                Width = width,
                                Height = height,
                                Format = BackBufferFormat,
                                Stereo = false,
                                SampleDescription = new DXGI.SampleDescription(1, 0),
                                Usage = DXGI.Usage.RenderTargetOutput,
                                BufferCount = bufferCount,
                                Scaling = Scaling.Stretch,
                                SwapEffect = allowTearing ? SwapEffect.FlipDiscard : DXGI.SwapEffect.Discard,
                                AlphaMode = AlphaMode.Ignore,
                                Flags = allowTearing ? SwapChainFlags.AllowTearing : DXGI.SwapChainFlags.None,
                            };

                            var fullscreenDescription = new SwapChainFullScreenDescription
                            {
                                Windowed = true
                            };

                            _swapChain = new SwapChain1(dxgiFactory2,
                                deviceOrCommandQueue,
                                win32Handle.HWnd,
                                ref swapchainDesc,
                                fullscreenDescription);
                        }
                        else
                        {
                            SwapChainDescription dxgiSCDesc = new SwapChainDescription
                            {
                                BufferCount = bufferCount,
                                IsWindowed = true,
                                ModeDescription = new ModeDescription(width, height, new Rational(60, 1), BackBufferFormat),
                                OutputHandle = win32Handle.HWnd,
                                SampleDescription = new SampleDescription(1, 0),
                                SwapEffect = SwapEffect.Discard,
                                Usage = DXGI.Usage.BackBuffer | DXGI.Usage.RenderTargetOutput
                            };

                            _swapChain = new DXGI.SwapChain(dxgiFactory, deviceOrCommandQueue, dxgiSCDesc);
                        }

                        dxgiFactory.MakeWindowAssociation(win32Handle.HWnd, WindowAssociationFlags.IgnoreAll);
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
        public override int BackBufferCount { get; }

        /// <inheritdoc/>
        public override int CurrentBackBuffer => _currentBackBuffer;

        public Format BackBufferFormat { get; } = Format.B8G8R8A8_UNorm;

        /// <inheritdoc/>
        protected override void ConfigureImpl(in SwapChainDescriptor descriptor)
        {
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            _swapChain.Dispose();
        }

        protected override void PresentImpl()
        {
            //var parameters = new SharpDX.DXGI.PresentParameters();

            try
            {
                _swapChain.Present(_syncInterval, _presentFlags);
                _currentBackBuffer = (_currentBackBuffer + 1) % BackBufferCount;
            }
            catch (SharpDXException)
            {
            }
        }
    }
}
