// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.
using SharpDX.Direct3D12;

namespace Vortice.Graphics.D3D12
{
    internal struct DescriptorHandle
    {
        public const long InvalidAddress = -1;

        public readonly DescriptorHeap Heap;
        public readonly int SizeIncrement;
        public readonly CpuDescriptorHandle CpuHandle;
        public readonly GpuDescriptorHandle GpuHandle;

        public DescriptorHandle(DescriptorHeap heap, int sizeIncrement, CpuDescriptorHandle cpuHandle)
        {
            Heap = heap;
            SizeIncrement = sizeIncrement;
            CpuHandle = cpuHandle;
            GpuHandle = new GpuDescriptorHandle
            {
                Ptr = InvalidAddress
            };
        }

        public DescriptorHandle(DescriptorHeap heap, int sizeIncrement, CpuDescriptorHandle cpuHandle, GpuDescriptorHandle gpuHandle)
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

        public GpuDescriptorHandle GetGpuHandle(int index)
        {
            return GpuHandle + (SizeIncrement * index);
        }

        public bool IsNull() => CpuHandle.Ptr == 0;
        public bool IsShaderVisible() => GpuHandle.Ptr != 0 && GpuHandle.Ptr != InvalidAddress;
    }
}
