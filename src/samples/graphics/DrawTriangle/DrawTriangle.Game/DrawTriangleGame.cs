// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Vortice;
using Vortice.Assets.Graphics;
using Vortice.Graphics;
using Vortice.Graphics.Direct3D11;
using Vortice.Mathematics;

namespace DrawTriangle
{
    public sealed class DrawTriangleGame : Game
    {
        private GraphicsBuffer _vertexBuffer;
        private Shader _vertexShader;
        private Shader _fragmentShader;
        private PipelineState _renderPipelineState;
        private GraphicsBuffer _matricesConstantBuffer;

        public DrawTriangleGame()
        {
        }

        protected override GraphicsDeviceFactory CreateGraphicsFactory(bool validation)
        {
            return new D3D11GraphicsDeviceFactory(validation);
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

            unsafe
            {
                var worldViewProjection = Matrix4x4.Identity;
                _matricesConstantBuffer = GraphicsDevice.CreateBuffer(new BufferDescriptor(Unsafe.SizeOf<Matrix4x4>(), BufferUsage.Constant), (IntPtr)Unsafe.AsPointer(ref worldViewProjection));
            }

            const string shaderSource = @"struct PSInput {
                float4 position : SV_POSITION;
                float4 color : COLOR;
            };

            cbuffer Matrices : register(b0)
            {
	            float4x4	WorldViewProjectionMatrix : packoffset(c0);
            };

            PSInput VSMain(float4 position : ATTRIBUTE0, float4 color : ATTRIBUTE1) {
                PSInput result;

                //result.position = mul(position, WorldViewProjectionMatrix);
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

            _vertexShader = GraphicsDevice.CreateShader(ShaderCompiler.Compile(GraphicsDevice.Backend, shaderSource, ShaderStages.Vertex));
            _fragmentShader = GraphicsDevice.CreateShader(ShaderCompiler.Compile(GraphicsDevice.Backend, shaderSource, ShaderStages.Pixel));

            _renderPipelineState = GraphicsDevice.CreateRenderPipelineState(new RenderPipelineDescriptor
            {
                VertexShader = _vertexShader,
                FragmentShader = _fragmentShader,
                VertexLayouts = new VertexLayoutDescriptor[]
                {
                    new VertexLayoutDescriptor(
                        new VertexAttributeDescriptor(0, VertexFormat.Float3, 0),
                        new VertexAttributeDescriptor(1, VertexFormat.Float4, 12))
                }
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
            commandBuffer.SetConstantBuffer(ShaderStages.Vertex, 0, _matricesConstantBuffer);
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
