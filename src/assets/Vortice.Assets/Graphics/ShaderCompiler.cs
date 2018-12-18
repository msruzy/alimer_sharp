// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using Vortice.Graphics;

namespace Vortice.Assets.Graphics
{
    public static class ShaderCompiler
    {
        public static byte[] Compile(string source, string entryPoint, ShaderLanguage language, ShaderStages stage)
        {
            return null;
        }
    }

    public enum ShaderLanguage
    {
        HLSL,
        GLSL
    }
}
