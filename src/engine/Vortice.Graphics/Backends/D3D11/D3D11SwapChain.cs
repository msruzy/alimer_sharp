// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX;
using SharpDX.Mathematics.Interop;
using SharpDX.Direct3D11;
using System;
using DXGI = SharpDX.DXGI;
using Vortice.Graphics.D3D;

namespace Vortice.Graphics.D3D11
{
    internal unsafe class D3D11SwapChain
    {
        public const int FrameCount = 2;
        private readonly int _syncInterval = 1;
        private readonly DXGI.PresentFlags _presentFlags;

        private readonly DXGI.SwapChain _swapChain;
        public readonly D3D11Texture BackbufferTexture;

        public D3D11SwapChain(D3D11GraphicsDevice device, PresentationParameters presentationParameters)
        {
            var width = Math.Max(presentationParameters.BackBufferWidth, 1);
            var height = Math.Max(presentationParameters.BackBufferHeight, 1);

            using (var dxgiDevice2 = device.Device.QueryInterface<SharpDX.DXGI.Device2>())
            {
                using (var dxgiAdapter = dxgiDevice2.Adapter)
                {
                    using (var dxgiFactory2 = dxgiAdapter.GetParent<SharpDX.DXGI.Factory2>())
                    {

                        switch (presentationParameters.DeviceWindowHandle)
                        {
                            case IntPtr hwnd:
                                {
                                    // Check tearing support.
                                    RawBool allowTearing = false;
                                    if (PlatformDetection.IsWindows10x)
                                    {
                                        using (var factory5 = dxgiFactory2.QueryInterfaceOrNull<DXGI.Factory5>())
                                        {
                                            factory5.CheckFeatureSupport(DXGI.Feature.PresentAllowTearing,
                                                new IntPtr(&allowTearing), sizeof(RawBool)
                                                );

                                            // Recommended to always use tearing if supported when using a sync interval of 0.
                                            _syncInterval = 0;
                                            _presentFlags |= DXGI.PresentFlags.AllowTearing;
                                        }
                                    }

                                    var swapchainDesc = new DXGI.SwapChainDescription1()
                                    {
                                        Width = width,
                                        Height = height,
                                        Format = DXGI.Format.B8G8R8A8_UNorm,
                                        Stereo = false,
                                        SampleDescription = new DXGI.SampleDescription(1, 0),
                                        Usage = DXGI.Usage.RenderTargetOutput,
                                        BufferCount = FrameCount,
                                        Scaling = DXGI.Scaling.Stretch,
                                        SwapEffect = allowTearing ? DXGI.SwapEffect.FlipDiscard : DXGI.SwapEffect.Discard,
                                        AlphaMode = DXGI.AlphaMode.Ignore,
                                        Flags = allowTearing ? DXGI.SwapChainFlags.AllowTearing : DXGI.SwapChainFlags.None,
                                    };

                                    var fullscreenDescription = new DXGI.SwapChainFullScreenDescription
                                    {
                                        Windowed = true
                                    };

                                    _swapChain = new DXGI.SwapChain1(dxgiFactory2, device.Device, hwnd, ref swapchainDesc, fullscreenDescription);
                                    dxgiFactory2.MakeWindowAssociation(hwnd, DXGI.WindowAssociationFlags.IgnoreAll);
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
                }
            }

            var backBufferTexture = Resource.FromSwapChain<Texture2D>(_swapChain, 0);
            var d3dTextureDesc = backBufferTexture.Description;
            var textureDescription = TextureDescription.Texture2D(
                d3dTextureDesc.Width,
                d3dTextureDesc.Height,
                d3dTextureDesc.MipLevels,
                d3dTextureDesc.ArraySize,
                D3DConvert.Convert(d3dTextureDesc.Format),
                D3D11Convert.Convert(d3dTextureDesc.BindFlags),
                (SampleCount)d3dTextureDesc.SampleDescription.Count);
            BackbufferTexture = new D3D11Texture(device, textureDescription, backBufferTexture);
        }

        /// <inheritdoc/>
        public void Destroy()
        {
            BackbufferTexture.Dispose();
            _swapChain.Dispose();
        }

        public void Present()
        {
            //var parameters = new SharpDX.DXGI.PresentParameters();

            try
            {
                _swapChain.Present(_syncInterval, _presentFlags);
            }
            catch (SharpDXException)
            {
            }
        }
    }
}
