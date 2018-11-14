// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;

namespace Vortice.Graphics.D3D11
{
    //internal class D3D11CommandQueue : CommandQueue
    //{
    //    private readonly Device _nativeDevice;
    //    private readonly DeviceContext _immediateContext;
    //    private readonly object _contextLock = new object();
    //    private readonly List<D3D11CommandBuffer> _completedBuffers = new List<D3D11CommandBuffer>();
    //    private readonly List<D3D11CommandBuffer> _pendingCommandBuffers = new List<D3D11CommandBuffer>();

    //    public D3D11CommandQueue(D3D11GraphicsDevice device)
    //        : base(device)
    //    {
    //        _nativeDevice = device.D3DDevice;
    //        _immediateContext = device.D3DContext1;
    //    }

    //    public override CommandBuffer CreateCommandBuffer()
    //    {
    //        D3D11CommandBuffer commandBuffer;
    //        if (_completedBuffers.Count > 0)
    //        {
    //            commandBuffer = _completedBuffers[0];
    //            _completedBuffers.RemoveAt(0);
    //        }
    //        else
    //        {
    //            commandBuffer = new D3D11CommandBuffer(this, _nativeDevice);
    //        }

    //        return commandBuffer;
    //    }

    //    public void Commit(D3D11CommandBuffer commandBuffer)
    //    {
    //        _pendingCommandBuffers.Add(commandBuffer);
    //    }

    //    public void Tick()
    //    {
    //        if (_pendingCommandBuffers.Count == 0)
    //            return;

    //        _pendingCommandBuffers.Sort((x, y) => x.ExecutionOrder.CompareTo(y.ExecutionOrder));

    //        lock (_contextLock)
    //        {
    //            foreach (var commandBuffer in _pendingCommandBuffers)
    //            {
    //                _immediateContext.ExecuteCommandList(commandBuffer.CommandList, false);
    //                _completedBuffers.Add(commandBuffer);
    //                commandBuffer.Reset();
    //            }
    //        }

    //        _pendingCommandBuffers.Clear();
    //    }
    //}
}
