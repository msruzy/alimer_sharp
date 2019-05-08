// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

// https://github.com/Microsoft/DirectXShaderCompiler/blob/master/tools/clang/tools/dotnetc/D3DCompiler.cs
namespace D3DCompiler
{
    using DotNetDxc;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct D3D_SHADER_MACRO
    {
        [MarshalAs(UnmanagedType.LPStr)] public readonly string Name;
        [MarshalAs(UnmanagedType.LPStr)] public readonly string Definition;
    }

    internal static class D3DCompiler
    {
        [DllImport("d3dcompiler_47.dll", CallingConvention = CallingConvention.Winapi, SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = true)]
        public extern static int D3DCompile(
            [MarshalAs(UnmanagedType.LPStr)] string srcData, int srcDataSize,
            [MarshalAs(UnmanagedType.LPStr)] string sourceName,
            [MarshalAs(UnmanagedType.LPArray)] D3D_SHADER_MACRO[] defines,
            int pInclude,
            [MarshalAs(UnmanagedType.LPStr)] string entryPoint,
            [MarshalAs(UnmanagedType.LPStr)] string target,
            uint Flags1,
            uint Flags2,
            out IDxcBlob code, out IDxcBlob errorMsgs);

        [DllImport("d3dcompiler_47.dll", CallingConvention = CallingConvention.Winapi, SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = true)]
        public extern static Int32 D3DDisassemble(
            IntPtr ptr, uint ptrSize, uint flags,
            [MarshalAs(UnmanagedType.LPStr)] string szComments,
            out IDxcBlob disassembly);
    }
}
