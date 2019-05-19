// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotNetDxc
{
    [ComImport]
    [Guid("8BA5FB08-5195-40e2-AC58-0D989C3A0102")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDxcBlob
    {
        [PreserveSig]
        unsafe char* GetBufferPointer();

        [PreserveSig]
        uint GetBufferSize();
    }

    [ComImport]
    [Guid("8BA5FB08-5195-40e2-AC58-0D989C3A0102")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDxcBlobEncoding : IDxcBlob
    {
        uint GetEncoding(out bool unknown, out uint codePage);
    }

    [ComImport]
    [Guid("CEDB484A-D4E9-445A-B991-CA21CA157DC2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDxcOperationResult
    {
        int GetStatus();
        IDxcBlob GetResult();
        IDxcBlobEncoding GetErrors();
    }

    [ComImport]
    [Guid("7f61fc7d-950d-467f-b3e3-3c02fb49187c")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDxcIncludeHandler
    {
        IDxcBlob LoadSource(string fileName);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DXCEncodedText
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string pText;

        public uint Size;

        public uint CodePage; //should always be UTF-8 for this use
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DXCDefine
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pName;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string pValue;
    }

    [ComImport]
    [Guid("e5204dc7-d18c-4c3c-bdfb-851673980fe7")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDxcLibrary
    {
        void SetMalloc(object malloc);
        IDxcBlob CreateBlobFromBlob(IDxcBlob blob, UInt32 offset, UInt32 length);
        IDxcBlobEncoding CreateBlobFromFile(string fileName, System.IntPtr codePage);
        IDxcBlobEncoding CreateBlobWithEncodingFromPinned(byte[] text, UInt32 size, UInt32 codePage);
        // IDxcBlobEncoding CreateBlobWithEncodingOnHeapCopy(IntrPtr text, UInt32 size, UInt32 codePage);
        IDxcBlobEncoding CreateBlobWithEncodingOnHeapCopy(string text, UInt32 size, UInt32 codePage);
        IDxcBlobEncoding CreateBlobWithEncodingOnMalloc(string text, object malloc, UInt32 size, UInt32 codePage);
        IDxcIncludeHandler CreateIncludeHandler();
        System.Runtime.InteropServices.ComTypes.IStream CreateStreamFromBlobReadOnly(IDxcBlob blob);
        IDxcBlobEncoding GetBlobAstUf8(IDxcBlob blob);
        IDxcBlobEncoding GetBlobAstUf16(IDxcBlob blob);
    }

    [ComImport]
    [Guid("8c210bf3-011f-4422-8d70-6f9acb8db617")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDxcCompiler
    {
        IDxcOperationResult Compile(IDxcBlob source, string sourceName, string entryPoint, string targetProfile,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType =UnmanagedType.LPWStr)]
            string[] arguments,
            int argCount, DXCDefine[] defines, int defineCount, IDxcIncludeHandler includeHandler);
        IDxcOperationResult Preprocess(IDxcBlob source, string sourceName, string[] arguments, int argCount,
            DXCDefine[] defines, int defineCount, IDxcIncludeHandler includeHandler);
        IDxcBlobEncoding Disassemble(IDxcBlob source);
    }

    [ComImport]
    [Guid("d2c21b26-8350-4bdc-976a-331ce6f4c54c")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDxcContainerReflection
    {
        void Load(IDxcBlob container);
        uint GetPartCount();
        uint GetPartKind(uint idx);
        IDxcBlob GetPartContent(uint idx);
        [PreserveSig]
        int FindFirstPartKind(uint kind, out uint result);

        int GetPartReflection(uint idx, ref Guid iid, out IntPtr ppvObject);
    }

    public static class Dxc
    {
        private static Guid CLSID_DxcAssembler = new Guid("D728DB68-F903-4F80-94CD-DCCF76EC7151");
        private static Guid CLSID_DxcDiaDataSource = new Guid("CD1F6B73-2AB0-484D-8EDC-EBE7A43CA09F");
        private static Guid CLSID_DxcIntelliSense = new Guid("3047833c-d1c0-4b8e-9d40-102878605985");
        private static Guid CLSID_DxcRewriter = new Guid("b489b951-e07f-40b3-968d-93e124734da4");
        private static Guid CLSID_DxcCompiler = new Guid("73e22d93-e6ce-47f3-b5bf-f0664f39c1b0");
        private static Guid CLSID_DxcLinker = new Guid("EF6A8087-B0EA-4D56-9E45-D07E1A8B7806");
        private static Guid CLSID_DxcContainerReflection = new Guid("b9f54489-55b8-400c-ba3a-1675e4728b91");
        private static Guid CLSID_DxcLibrary = new Guid("6245D6AF-66E0-48FD-80B4-4D271796748C");
        private static Guid CLSID_DxcOptimizer = new Guid("AE2CD79F-CC22-453F-9B6B-B124E7A5204C");
        private static Guid CLSID_DxcValidator = new Guid("8CA3E215-F728-4CF3-8CDD-88AF917587A1");

        public const uint DFCC_DXIL = 1279875140;
        public const uint DFCC_SHDR = 1380206675;
        public const uint DFCC_SHEX = 1480935507;
        public const uint DFCC_ILDB = 1111772233;
        public const uint DFCC_SPDB = 1111773267;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IDxcCompiler CreateDxcCompiler()
        {
            DxcCreateInstance(CLSID_DxcCompiler, typeof(IDxcCompiler).GUID, out object result);
            return (IDxcCompiler)result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IDxcLibrary CreateDxcLibrary()
        {
            DxcCreateInstance(CLSID_DxcLibrary, typeof(IDxcLibrary).GUID, out object result);
            return (IDxcLibrary)result;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static IDxcContainerReflection CreateDxcContainerReflection()
        {
            DxcCreateInstance(CLSID_DxcContainerReflection, typeof(IDxcContainerReflection).GUID, out object result);
            return (IDxcContainerReflection)result;
        }

        public static IDxcBlobEncoding CreateBlobForText(string text)
        {
            return CreateBlobForText(Library, text);
        }

        public static IDxcBlobEncoding CreateBlobForText(IDxcLibrary library, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            const uint CP_UTF16 = 1200;
            return library.CreateBlobWithEncodingOnHeapCopy(text, (uint)(text.Length * 2), CP_UTF16);
        }

        public static string GetStringFromBlob(IDxcBlob blob)
        {
            return GetStringFromBlob(Library, blob);
        }

        public static string GetStringFromBlob(IDxcLibrary library, IDxcBlob blob)
        {
            unsafe
            {
                blob = library.GetBlobAstUf16(blob);
                return new string(blob.GetBufferPointer(), 0, (int)(blob.GetBufferSize() / 2));
            }
        }

        public static byte[] GetBytesFromBlob(IDxcBlob blob)
        {
            unsafe
            {
                byte* pMem = (byte*)blob.GetBufferPointer();
                uint size = blob.GetBufferSize();
                byte[] result = new byte[size];
                fixed (byte* pTarget = result)
                {
                    for (uint i = 0; i < size; ++i)
                        pTarget[i] = pMem[i];
                }
                return result;
            }
        }

        public static readonly IDxcLibrary Library;

        static Dxc()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _createInstanceFn = LoadDxcCreateInstanceWindows();
            }

            Library = CreateDxcLibrary();
        }

        private delegate int DxcCreateInstanceFn(ref Guid clsid, ref Guid iid, [MarshalAs(UnmanagedType.IUnknown)] out object instance);
        private static DxcCreateInstanceFn _createInstanceFn;

        private static int DxcCreateInstance(Guid clsid, Guid iid, out object instance)
        {
            return _createInstanceFn(ref clsid, ref iid, out instance);
        }

        private static DxcCreateInstanceFn LoadDxcCreateInstanceWindows()
        {
            var handle = LoadLibraryW("dxcompiler.dll");
            if (handle == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception();
            }

            var fnPtr = GetProcAddress(handle, "DxcCreateInstance");
            return (DxcCreateInstanceFn)Marshal.GetDelegateForFunctionPointer(fnPtr, typeof(DxcCreateInstanceFn));
        }

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern IntPtr LoadLibraryW([MarshalAs(UnmanagedType.LPWStr)] string fileName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    }
}
