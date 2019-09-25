// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.Direct3D12;

namespace Alimer.Graphics.Direct3D12
{
    internal struct DescriptorHandle
    {
        public readonly ID3D12DescriptorHeap Heap;
        public readonly int SizeIncrement;
        public readonly CpuDescriptorHandle CpuHandle;
        public readonly GpuDescriptorHandle GpuHandle;

        public DescriptorHandle(ID3D12DescriptorHeap heap, int sizeIncrement, CpuDescriptorHandle cpuHandle)
        {
            Heap = heap;
            SizeIncrement = sizeIncrement;
            CpuHandle = cpuHandle;
            GpuHandle = default;
        }

        public DescriptorHandle(ID3D12DescriptorHeap heap, int sizeIncrement, CpuDescriptorHandle cpuHandle, GpuDescriptorHandle gpuHandle)
        {
            Heap = heap;
            SizeIncrement = sizeIncrement;
            CpuHandle = cpuHandle;
            GpuHandle = gpuHandle;
        }

        public CpuDescriptorHandle GetCpuHandle(int index)
        {
            return CpuHandle + (SizeIncrement * index);
        }

        //public GpuDescriptorHandle GetGpuHandle(int index)
        //{
        //    return GpuHandle + (SizeIncrement * index);
        //}

        public bool IsNull() => CpuHandle.Ptr == 0;
        public bool IsShaderVisible() => GpuHandle.Ptr != 0 && GpuHandle.Ptr != 0;
    }
}
