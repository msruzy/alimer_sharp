// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX;
using SharpDX.Mathematics.Interop;
using SharpDX.Direct3D12;
using System;
using DXGI = SharpDX.DXGI;

namespace Vortice.Graphics.DirectX12
{
    internal unsafe class DirectX12SwapChain
    {
        public const int FrameCount = 2;
        private readonly int _syncInterval = 1;
        private readonly DXGI.PresentFlags _presentFlags;

        private readonly SharpDX.DXGI.SwapChain3 _swapChain;
        public readonly DirectX12Texture[] _backbufferTextures;
        private int _currentBackBufferIndex;

        public DirectX12Texture BackbufferTexture => _backbufferTextures[_currentBackBufferIndex];

        public DirectX12SwapChain(
            SharpDX.DXGI.Factory2 factory,
            DirectX12GraphicsDevice device,
            PresentationParameters presentationParameters)
        {
            var width = Math.Max(presentationParameters.BackBufferWidth, 1);
            var height = Math.Max(presentationParameters.BackBufferHeight, 1);

            switch (presentationParameters.DeviceWindowHandle)
            {
                case IntPtr hwnd:
                    {
                        // Check tearing support.
                        RawBool allowTearing = false;
                        using (var factory5 = factory.QueryInterfaceOrNull<DXGI.Factory5>())
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

                        using (var newSwapChain = new DXGI.SwapChain1(factory,
                            device.DirectCommandQueue,
                            hwnd,
                            ref swapchainDesc,
                            fullscreenDescription))
                        {
                            _swapChain = newSwapChain.QueryInterface<DXGI.SwapChain3>();
                        }

                        factory.MakeWindowAssociation(hwnd, DXGI.WindowAssociationFlags.IgnoreAll);
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

            _backbufferTextures = new DirectX12Texture[FrameCount];
            for (int i = 0; i < FrameCount; i++)
            {
                var backBufferTexture = _swapChain.GetBackBuffer<Resource>(i);
                var d3dTextureDesc = backBufferTexture.Description;
                var textureDescription = TextureDescription.Texture2D(
                    (int)d3dTextureDesc.Width,
                    d3dTextureDesc.Height,
                    d3dTextureDesc.MipLevels,
                    d3dTextureDesc.DepthOrArraySize,
                    DirectX12Convert.Convert(d3dTextureDesc.Format),
                    DirectX12Convert.Convert(d3dTextureDesc.Flags),
                    (SampleCount)d3dTextureDesc.SampleDescription.Count);
                _backbufferTextures[i] = new DirectX12Texture(device, textureDescription, backBufferTexture);
            }

            _currentBackBufferIndex = _swapChain.CurrentBackBufferIndex;
        }

        /// <inheritdoc/>
        public void Destroy()
        {
            for (int i = 0; i < FrameCount; i++)
            {
                _backbufferTextures[i].Dispose();
            }

            _swapChain.Dispose();
        }

        public void Present()
        {
            //var parameters = new SharpDX.DXGI.PresentParameters();

            try
            {
                _swapChain.Present(_syncInterval, _presentFlags);
                _currentBackBufferIndex = _swapChain.CurrentBackBufferIndex;
            }
            catch (SharpDXException)
            {
            }
        }
    }
}
