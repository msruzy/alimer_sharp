// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Vortice;
using Vortice.Graphics;

namespace DrawTriangle
{
    public sealed class DrawTriangleGame : Game
    {
        private GraphicsBuffer _vertexBuffer;

        protected override void LoadContent()
        {
            base.LoadContent();

            var vertices = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(0.0f, 0.5f, 0.0f), new Color4(1.0f, 0.0f, 0.0f)),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.0f), new Color4(0.0f, 1.0f, 0.0f)),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.0f), new Color4(0.0f, 0.0f, 1.0f)),
            };
            _vertexBuffer = GraphicsDevice.CreateBuffer(BufferUsage.Vertex, vertices);

            // Set swap chain clear color.
            GraphicsDevice.MainSwapchain.ClearColor = new Color4(0.0f, 0.2f, 0.4f);
        }

        protected override void Draw(GameTime time)
        {
            base.Draw(time);

            var queue = GraphicsDevice.GraphicsQueue;

            // execution order is the order of creation by default
            var commandBuffer1 = queue.CreateCommandBuffer();
            commandBuffer1.ExecutionOrder = 0;
            var commandBuffer2 = queue.CreateCommandBuffer();
            commandBuffer2.ExecutionOrder = 1;

            var tasks = new Task[2];
            tasks[0] = Task.Run(() =>
            {
                var renderPass = GraphicsDevice.MainSwapchain.CurrentRenderPassDescriptor;
                renderPass.ColorAttachments[0].ClearColor = new Color4(1.0f, 0.0f, 0.0f);

                commandBuffer1.BeginRenderPass(renderPass);
                commandBuffer1.EndRenderPass();
                commandBuffer1.Commit();
            });

            tasks[1] = Task.Run(() =>
            {
                var renderPass = GraphicsDevice.MainSwapchain.CurrentRenderPassDescriptor;
                renderPass.ColorAttachments[0].ClearColor = new Color4(1.0f, 1.0f, 0.0f);

                commandBuffer2.BeginRenderPass(renderPass);
                commandBuffer2.EndRenderPass();
                commandBuffer2.Commit();
            });

            Task.WaitAll(tasks);
        }

        private readonly struct VertexPositionColor
        {
            public readonly Vector3 Position;
            public readonly Color4 Color;

            public VertexPositionColor(Vector3 position, in Color4 color)
            {
                Position = position;
                Color = color;
            }
        }
    }
}
