// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using System.Runtime.Serialization;
using Vortice;
using Vortice.Graphics;

namespace DrawTriangle
{
    public readonly struct Entity : IEquatable<Entity>
    {
        public int Id { get; }

        public Entity(int id)
        {
            Id = id;
        }

        public bool Equals(Entity other) => Id == other.Id;
    }

    public class EntityManager
    {
        private int _nextEntityId = 1;

        public Entity CreateEntity()
        {
            var entityId = _nextEntityId;
            _nextEntityId++;
            return new Entity(entityId);
        }
    }

    [DataContract(Name = nameof(Scene))]
    public sealed class Scene
    {
        private readonly EntityManager _entityManager = new EntityManager();

        public Entity CreateEntity() => _entityManager.CreateEntity();
    }

    public sealed class DrawTriangleGame : Game
    {
        private GraphicsBuffer _vertexBuffer;
        private readonly Scene _scene = new Scene();

        protected override GraphicsDeviceFactory CreateGraphicsDeviceFactory()
        {
#if DEBUG
            bool validation = true;
#else
            bool validation = false;
#endif
            if (DirectX12GraphicsDeviceFactory.IsSupported())
            {
                //return new DirectX12GraphicsDeviceFactory(validation);
            }

            return new DirectX11GraphicsDeviceFactory(validation);
        }

        protected override void Initialize()
        {
            _scene.CreateEntity();

            base.Initialize();
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

        protected override void Draw(GameTime time)
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
