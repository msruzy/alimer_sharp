// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using Vortice.Graphics;
using DotNetDxc;
using Vortice.DirectX.ShaderCompiler;
using Vortice.DirectX.ShaderCompiler.D3D12;

namespace Vortice.Assets.Graphics
{
    public static class ShaderCompiler
    {
        public static Vortice.Graphics.ShaderBytecode Compile(
            GraphicsBackend backend,
            string source,
            ShaderStages stage,
            string entryPoint = "",
            string fileName = "")
        {
            if (string.IsNullOrEmpty(entryPoint))
            {
                entryPoint = GetDefaultEntryPoint(stage);
            }

            // We use legacy compiler for D3D11.
            bool isDxil = backend != GraphicsBackend.Direct3D11;
            if (isDxil)
            {
                RuntimeHelpers.RunClassConstructor(typeof(Dxc).TypeHandle);
                var shaderProfile = $"{GetShaderProfile(stage)}_6_0";

                var arguments = new string[]
                {
                    "-T", shaderProfile,
                    "-E", entryPoint,
                };

                var compiler = Dxc.CreateDxcCompiler();
                var result = compiler.Compile(
                    Dxc.CreateBlobForText(source),
                    fileName,
                    entryPoint,
                    shaderProfile,
                    arguments,
                    arguments.Length,
                    null,
                    0,
                    Dxc.Library.CreateIncludeHandler()
                    );

                if (result.GetStatus() == 0)
                {
                    var blob = result.GetResult();
                    var bytecode = Dxc.GetBytesFromBlob(blob);

                    var containReflection = Dxc.CreateDxcContainerReflection();
                    containReflection.Load(blob);
                    int hr = containReflection.FindFirstPartKind(Dxc.DFCC_DXIL, out uint dxilPartIndex);
                    if (hr < 0)
                    {
                        //MessageBox.Show("Debug information not found in container.");
                        //return;
                    }

                    var f = containReflection.GetPartReflection(dxilPartIndex, typeof(ID3D12ShaderReflection).GUID, out var nativePtr);
                    using (var shaderReflection = new ID3D12ShaderReflection(nativePtr))
                    {
                        var shaderReflectionDesc = shaderReflection.Description;

                        foreach (var parameterDescription in shaderReflection.InputParameters)
                        {
                        }

                        foreach (var resource in shaderReflection.Resources)
                        {
                        }
                    }

                    unsafe
                    {
                        IDxcBlob part = containReflection.GetPartContent(dxilPartIndex);
                        uint* p = (uint*)part.GetBufferPointer();
                        var v = DescribeProgramVersion(*p);
                    }

                    // Disassemble
                    var disassembleBlob = compiler.Disassemble(blob);
                    string disassemblyText = Dxc.GetStringFromBlob(disassembleBlob);

                    return new Vortice.Graphics.ShaderBytecode(stage, bytecode);
                }
                else
                {
                    var resultText = Dxc.GetStringFromBlob(result.GetErrors());
                }
            }
            else
            {
                uint flags = 0;
                var shaderProfile = $"{GetShaderProfile(stage)}_5_0";
                int hr = D3DCompiler.D3DCompiler.D3DCompile(
                    source,
                    source.Length,
                    fileName,
                    null,
                    0,
                    entryPoint,
                    shaderProfile,
                    flags,
                    0,
                    out IDxcBlob blob,
                    out IDxcBlob errorMsgs);

                if (hr != 0)
                {
                    if (errorMsgs != null)
                    {
                        var errorText = Dxc.GetStringFromBlob(errorMsgs);
                    }
                }
                else
                {
                    var bytecode = Dxc.GetBytesFromBlob(blob);
                    return new Vortice.Graphics.ShaderBytecode(stage, bytecode);
                }
            }

            return default;
        }

        private static string GetDefaultEntryPoint(ShaderStages stage)
        {
            switch (stage)
            {
                case ShaderStages.Vertex:
                    return "VSMain";
                case ShaderStages.Hull:
                    return "HSMain";
                case ShaderStages.Domain:
                    return "DSMain";
                case ShaderStages.Geometry:
                    return "GSMain";
                case ShaderStages.Pixel:
                    return "PSMain";
                case ShaderStages.Compute:
                    return "CSMain";
                default:
                    return string.Empty;
            }
        }

        private static string GetShaderProfile(ShaderStages stage)
        {
            switch (stage)
            {
                case ShaderStages.Vertex:
                    return "vs";
                case ShaderStages.Hull:
                    return "hs";
                case ShaderStages.Domain:
                    return "ds";
                case ShaderStages.Geometry:
                    return "gs";
                case ShaderStages.Pixel:
                    return "ps";
                case ShaderStages.Compute:
                    return "cs";
                default:
                    return string.Empty;
            }
        }

        private static string DescribeProgramVersion(uint programVersion)
        {
            uint kind, major, minor;
            kind = ((programVersion & 0xffff0000) >> 16);
            major = (programVersion & 0xf0) >> 4;
            minor = (programVersion & 0xf);
            string[] shaderKinds = "Pixel,Vertex,Geometry,Hull,Domain,Compute".Split(',');
            return shaderKinds[kind] + " " + major + "." + minor;
        }
    }
}
