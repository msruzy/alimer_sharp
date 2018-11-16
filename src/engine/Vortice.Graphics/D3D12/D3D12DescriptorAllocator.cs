// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal class D3D12DescriptorAllocator : IDisposable
    {
        public const int MaxCbvUavSrvHeapSize = 1000000;
        public const int MaxSamplerHeapSize = 2048;

        public readonly D3D12GraphicsDevice Device;
        public int CBVDescriptorSize => _sizeIncrements[(int)DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView];
        public int SRVDescriptorSize => _sizeIncrements[(int)DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView];
        public int UAVDescriptorSize => _sizeIncrements[(int)DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView];
        public int SamplerDescriptorSize => _sizeIncrements[(int)DescriptorHeapType.Sampler];
        public int RTVDescriptorSize => _sizeIncrements[(int)DescriptorHeapType.RenderTargetView];
        public int DSVDescriptorSize => _sizeIncrements[(int)DescriptorHeapType.DepthStencilView];

        private readonly int[] _sizeIncrements;
        private readonly KeyValuePair<DescriptorHeap, AllocationInfo>[] _cpuDescriptorHeapInfos;
        private readonly KeyValuePair<DescriptorHeap, AllocationInfo>[] _gpuDescriptorHeapInfos;

        public D3D12DescriptorAllocator(D3D12GraphicsDevice device)
        {
            Device = device;
            _sizeIncrements = new int[(int)DescriptorHeapType.NumTypes];
            _cpuDescriptorHeapInfos = new KeyValuePair<DescriptorHeap, AllocationInfo>[(int)DescriptorHeapType.NumTypes];
            _gpuDescriptorHeapInfos = new KeyValuePair<DescriptorHeap, AllocationInfo>[(int)DescriptorHeapType.NumTypes];

            for (var i = 0; i < (int)DescriptorHeapType.NumTypes; i++)
            {
                _sizeIncrements[i] = device.Device.GetDescriptorHandleIncrementSize((DescriptorHeapType)i);
            }
        }

        public void Dispose()
        {
        }

        public DescriptorHeapHandle AllocateCPUHeap(DescriptorHeapType type, int count)
        {
            return Allocate(type, count, count, ref _cpuDescriptorHeapInfos[(int)type], DescriptorHeapFlags.None);
        }

        private DescriptorHeapHandle Allocate(
            DescriptorHeapType type,
            int count,
            int allocationSize,
            ref KeyValuePair<DescriptorHeap, AllocationInfo> heapInfo,
            DescriptorHeapFlags flags)
        {
            if (count == 0)
            {
                return default;
            }

            var allocationInfo = default(AllocationInfo);
            var handle = default(DescriptorHeapHandle);

            {
                // If the current pool for this type has space, linearly allocate count bytes in the
                // pool
                allocationInfo = heapInfo.Value;
                if (allocationInfo.Remaining >= count)
                {
                    handle = new DescriptorHeapHandle(
                        heapInfo.Key,
                        _sizeIncrements[(int)type],
                        allocationInfo.Size - allocationInfo.Remaining);

                    heapInfo = new KeyValuePair<DescriptorHeap, AllocationInfo>(
                        heapInfo.Key,
                        new AllocationInfo(allocationInfo.Size, allocationInfo.Remaining - count)
                        );
                    Release(ref handle);
                    return handle;
                }
            }

            // If the pool has no more space, replace the pool with a new one of the specified size
            var heapDescriptor = new DescriptorHeapDescription()
            {
                Type = type,
                DescriptorCount = allocationSize,
                Flags = flags,
                NodeMask = 0,
            };

            var heap = Device.Device.CreateDescriptorHeap(heapDescriptor);

            allocationInfo = new AllocationInfo(allocationSize, allocationSize - count);
            heapInfo = new KeyValuePair<DescriptorHeap, AllocationInfo>(heap, allocationInfo);

            handle = new DescriptorHeapHandle(heap, _sizeIncrements[(int)type], 0);
            Release(ref handle);
            return handle;
        }

        public DescriptorHeapHandle AllocateGPUHeap(DescriptorHeapType type, int count)
        {
            Debug.Assert(
                type == DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView 
                || type == DescriptorHeapType.Sampler);

            var heapSize =
                (type == DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView ? MaxCbvUavSrvHeapSize
                                                                : MaxSamplerHeapSize);

            return Allocate(type, count, heapSize, ref _gpuDescriptorHeapInfos[(int)type], DescriptorHeapFlags.ShaderVisible);
        }

        private void Release(ref DescriptorHeapHandle handle)
        {

        }

        private readonly struct AllocationInfo
        {
            public readonly int Size;
            public readonly int Remaining;

            public AllocationInfo(int size, int remaining)
            {
                Size = size;
                Remaining = remaining;
            }
        }
    }

    public readonly struct DescriptorHeapHandle
    {
        public readonly DescriptorHeap Heap;
        private readonly int _sizeIncrement;
        private readonly int _offset;

        public DescriptorHeapHandle(DescriptorHeap heap, int sizeIncrement, int offset)
        {
            Heap = heap;
            _sizeIncrement = sizeIncrement;
            _offset = offset;
        }

        public CpuDescriptorHandle GetCPUHandle(int index)
        {
            Debug.Assert(Heap != null);
            var handle = Heap.CPUDescriptorHandleForHeapStart;
            handle.Ptr += _sizeIncrement * (index + _offset);
            return handle;
        }

        public GpuDescriptorHandle GetGPUHandle(int index)
        {
            Debug.Assert(Heap != null);
            var handle = Heap.GPUDescriptorHandleForHeapStart;
            handle.Ptr += _sizeIncrement * (index + _offset);
            return handle;
        }
    }

}
