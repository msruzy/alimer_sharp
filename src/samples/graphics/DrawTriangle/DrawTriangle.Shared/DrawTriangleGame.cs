// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Numerics;
using Vortice;
using Vortice.Graphics;

namespace DrawTriangle
{
    public sealed class DrawTriangleGame : Application
    {
        private GraphicsBuffer _vertexBuffer;

        protected override GraphicsDeviceFactory CreateGraphicsDeviceFactory()
        {
#if DEBUG
            bool validation = true;
#else
            bool validation = false;
#endif
            //if (DirectX12GraphicsDeviceFactory.IsSupported())
            //{
            //return new DirectX12GraphicsDeviceFactory(validation);
            //}

            return new DirectX11GraphicsDeviceFactory(validation);
        }

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
        }

        protected override void Draw(ApplicationTime time)
        {
            base.Draw(time);

            var context = GraphicsDevice.ImmediateCommandBuffer;
            context.BeginRenderPass(new Color4(0.0f, 0.2f, 0.4f));
            context.EndRenderPass();
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
