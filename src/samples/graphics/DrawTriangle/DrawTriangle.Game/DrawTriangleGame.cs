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
        private Shader _vertexShader;
        private Shader _pixelShader;
        private PipelineState _renderPipelineState;

        public DrawTriangleGame()
        {
            GraphicsBackend = GraphicsBackend.Direct3D11;
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

            const string shaderSource = @"struct PSInput {
                float4 position : SV_POSITION;
                float4 color : COLOR;
            };

            PSInput VSMain(float4 position : POSITION, float4 color : COLOR) {
                PSInput result;

                result.position = position;
                result.color = color;

                return result;
            }

            float4 PSMain(PSInput input) : SV_TARGET {
                return input.color;
            }

            [numthreads(1, 1, 1)]
            void CSMain(uint3 DTid : SV_DispatchThreadID ) {
            }";

            _vertexShader = GraphicsDevice.CreateShader(ShaderStages.Vertex, ShaderCompiler.Compile(shaderSource, ShaderStages.Vertex, ShaderLanguage.DXC));
            _pixelShader = GraphicsDevice.CreateShader(ShaderStages.Pixel, ShaderCompiler.Compile(shaderSource, ShaderStages.Pixel, ShaderLanguage.DXC));

            _renderPipelineState = GraphicsDevice.CreateRenderPipelineState(new RenderPipelineDescriptor
            {
                VertexShader = _vertexShader,
                PixelShader = _pixelShader
            });

            MainView.ClearColor = new Color4(0.0f, 0.2f, 0.4f);
        }

        protected override void Draw(GameTime time)
        {
            base.Draw(time);

            // Record commands to default context.
            var commandBuffer = GraphicsDevice.GetCommandQueue().GetCommandBuffer();
            commandBuffer.BeginRenderPass(MainView.CurrentRenderPassDescriptor);
            commandBuffer.SetPipelineState(_renderPipelineState);
            commandBuffer.SetVertexBuffer(_vertexBuffer, 0, 0);
            commandBuffer.Draw(3, 1, 0, 0);
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
