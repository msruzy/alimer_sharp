// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Numerics;
using Vortice;
using Vortice.Assets.Graphics;
using Vortice.Graphics;
using Vortice.Mathematics;

namespace DrawTriangle
{
    public sealed class DrawTriangleGame : Game
    {
        private GraphicsBuffer _vertexBuffer;
        private Shader _shader;

        protected override void LoadContent()
        {
            base.LoadContent();

            // new AudioEngine();

            var vertices = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(0.0f, 0.5f, 0.0f), new Color4(1.0f, 0.0f, 0.0f)),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.0f), new Color4(0.0f, 1.0f, 0.0f)),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.0f), new Color4(0.0f, 0.0f, 1.0f)),
            };
            _vertexBuffer = GraphicsDevice.CreateBuffer(BufferUsage.Vertex, vertices);

            const string shaderSource = @"struct PSInput
{
                float4 position : SV_POSITION;
                float4 color : COLOR;
            };

            PSInput VSMain(float4 position : POSITION, float4 color : COLOR)
{
                PSInput result;

                result.position = position;
                result.color = color;

                return result;
            }

            float4 PSMain(PSInput input) : SV_TARGET
{
                return input.color;
            }

[numthreads(1, 1, 1)]
        void CSMain(uint3 DTid : SV_DispatchThreadID )
        {
        }
";

            var vertex = ShaderCompiler.Compile(shaderSource, ShaderStages.Vertex, ShaderLanguage.DXC);
            var pixel = ShaderCompiler.Compile(shaderSource, ShaderStages.Pixel, ShaderLanguage.DXC);

            //_shader = GraphicsDevice.CreateShader(vertex, pixel);
        }

        protected override void Draw(GameTime time)
        {
            base.Draw(time);

            // Record commands to default context.
            var commandBuffer = GraphicsDevice.ImmediateCommandBuffer;
            var clearColor = new Color4(0.0f, 0.2f, 0.4f);
            commandBuffer.BeginRenderPass(MainView.CurrentRenderPassDescriptor);
            commandBuffer.EndRenderPass();
            commandBuffer.Commit();
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
