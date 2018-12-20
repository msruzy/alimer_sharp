// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using SharpDX;
using SharpDX.Direct3D12;
using SharpDX.Mathematics.Interop;
using System;
using DXGI = SharpDX.DXGI;

namespace Vortice.Graphics.D3D12
{
    internal unsafe class SwapchainD3D12 : Swapchain
    {
        private readonly int _backbufferCount;
        private readonly int _syncInterval = 1;
        private readonly DXGI.PresentFlags _presentFlags;
        private readonly DXGI.SwapChain3 _swapChain;

        private int _backBufferIndex;
        private readonly TextureD3D12[] _backbufferTextures;

        public SwapchainD3D12(
            D3D12GraphicsDevice device,
            PresentationParameters presentationParameters,
            int backbufferCount)
            : base(device)
        {
            _backbufferCount = backbufferCount;

            var width = Math.Max(presentationParameters.BackBufferWidth, 1);
            var height = Math.Max(presentationParameters.BackBufferHeight, 1);

            switch (presentationParameters.DeviceWindowHandle)
            {
                case IntPtr hwnd:
                    {
                        // Check tearing support.
                        var allowTearing = false;
                        var factory5 = device.DXGIFactory.QueryInterfaceOrNull<DXGI.Factory5>();
                        if (factory5 != null)
                        {
                            if (factory5.PresentAllowTearing)
                            {
                                // Recommended to always use tearing if supported when using a sync interval of 0.
                                _syncInterval = 0;
                                _presentFlags |= DXGI.PresentFlags.AllowTearing;
                                allowTearing = true;
                            }

                            factory5.Dispose();
                        }

                        var swapchainDesc = new SharpDX.DXGI.SwapChainDescription1()
                        {
                            Width = width,
                            Height = height,
                            Format = DXGI.Format.B8G8R8A8_UNorm,
                            Stereo = false,
                            SampleDescription = new DXGI.SampleDescription(1, 0),
                            Usage = DXGI.Usage.RenderTargetOutput,
                            BufferCount = _backbufferCount,
                            Scaling = DXGI.Scaling.Stretch,
                            SwapEffect = allowTearing ? DXGI.SwapEffect.FlipDiscard : DXGI.SwapEffect.Discard,
                            AlphaMode = DXGI.AlphaMode.Ignore,
                            Flags = DXGI.SwapChainFlags.AllowModeSwitch
                        };

                        if (allowTearing)
                        {
                            swapchainDesc.Flags |= DXGI.SwapChainFlags.AllowTearing;
                        }

                        var fullscreenDescription = new DXGI.SwapChainFullScreenDescription
                        {
                            Windowed = true
                        };

                        using (var swapChain = new DXGI.SwapChain1(device.DXGIFactory,
                            device.GraphicsQueue,
                            hwnd,
                            ref swapchainDesc,
                            fullscreenDescription))
                        {
                            _swapChain = swapChain.QueryInterface<DXGI.SwapChain3>();
                        }

                        device.DXGIFactory.MakeWindowAssociation(hwnd, DXGI.WindowAssociationFlags.IgnoreAll);
                    }
                    break;
            }

            _backBufferIndex = _swapChain.CurrentBackBufferIndex;
            _backbufferTextures = new TextureD3D12[_backbufferCount];
            for (int i = 0; i < _backbufferCount; i++)
            {
                var backBufferTexture = _swapChain.GetBackBuffer<Resource>(i);
                var d3dTextureDesc = backBufferTexture.Description;
                var textureDescription = TextureDescription.Texture2D(
                    (int)d3dTextureDesc.Width,
                    d3dTextureDesc.Height,
                    d3dTextureDesc.MipLevels,
                    d3dTextureDesc.DepthOrArraySize,
                    D3DConvert.Convert(d3dTextureDesc.Format),
                    D3D12Convert.Convert(d3dTextureDesc.Flags),
                    (SampleCount)d3dTextureDesc.SampleDescription.Count);

                _backbufferTextures[i] = new TextureD3D12(device, textureDescription, backBufferTexture);
            }

            Initialize(_backbufferCount);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            for (int i = 0; i < _backbufferCount; i++)
            {
                _backbufferTextures[i].Dispose();
            }
        }

        protected override Texture GetBackbufferTexture(int index)
        {
            return _backbufferTextures[index];
        }

        protected override int GetBackbufferIndex() => _backBufferIndex;

        public void Present()
        {
            try
            {
                _swapChain.Present(_syncInterval, _presentFlags);

                // Update the frame index.
                _backBufferIndex = _swapChain.CurrentBackBufferIndex;
            }
            catch (SharpDXException)
            {
            }
        }
    }
}
