// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.DirectX.Direct3D12;

namespace Vortice.Graphics.Direct3D12
{
    internal class DescriptorAllocator
    {
        private static readonly int DescriptorsPerHeap = 256;

        public readonly D3D12GraphicsDevice Device;
        public readonly DescriptorHeapType Type;
        public readonly bool IsShaderVisible;

        private ID3D12DescriptorHeap _currentHeap;
        private CpuDescriptorHandle _currentCpuHandle;
        private GpuDescriptorHandle _currentGpuHandle;
        private int _descriptorSize;
        private int _remainingFreeHandles;

        public DescriptorAllocator(D3D12GraphicsDevice device, DescriptorHeapType type)
        {
            Device = device;
            Type = type;
            IsShaderVisible = 
                type == DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView
                || type == DescriptorHeapType.Sampler;
        }

        public DescriptorHandle Allocate(int count)
        {
            if (_currentHeap == null
                || _remainingFreeHandles < count)
            {
                _currentHeap = Device.RequestNewHeap(Type, DescriptorsPerHeap);
                _currentCpuHandle = _currentHeap.GetCPUDescriptorHandleForHeapStart();
                if (IsShaderVisible)
                {
                    _currentGpuHandle = _currentHeap.GetGPUDescriptorHandleForHeapStart();
                }

                _remainingFreeHandles = DescriptorsPerHeap;

                if (_descriptorSize == 0)
                {
                    _descriptorSize = Device.D3D12Device.GetDescriptorHandleIncrementSize(Type);
                }
            }

            var cpuHandle = _currentCpuHandle;
            _currentCpuHandle.Ptr += count * _descriptorSize;
            _remainingFreeHandles -= count;

            if (IsShaderVisible)
            {
                var gpuHandle = _currentGpuHandle;
                _currentGpuHandle.Ptr += (ulong)(count * _descriptorSize);
                return new DescriptorHandle(_currentHeap, _descriptorSize, cpuHandle, gpuHandle);
            }

            return new DescriptorHandle(_currentHeap, _descriptorSize, cpuHandle);
        }
    }
}
