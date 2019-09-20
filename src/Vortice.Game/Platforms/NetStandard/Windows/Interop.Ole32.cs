// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vortice.Windows
{
    internal static class Ole32
    {
		public const string LibraryName = "ole32";

		[Flags]
		public enum CLSCTX : uint
		{
			ClsctxInprocServer = 0x1,
			ClsctxInprocHandler = 0x2,
			ClsctxLocalServer = 0x4,
			ClsctxInprocServer16 = 0x8,
			ClsctxRemoteServer = 0x10,
			ClsctxInprocHandler16 = 0x20,
			ClsctxReserved1 = 0x40,
			ClsctxReserved2 = 0x80,
			ClsctxReserved3 = 0x100,
			ClsctxReserved4 = 0x200,
			ClsctxNoCodeDownload = 0x400,
			ClsctxReserved5 = 0x800,
			ClsctxNoCustomMarshal = 0x1000,
			ClsctxEnableCodeDownload = 0x2000,
			ClsctxNoFailureLog = 0x4000,
			ClsctxDisableAaa = 0x8000,
			ClsctxEnableAaa = 0x10000,
			ClsctxFromDefaultContext = 0x20000,
			ClsctxInproc = ClsctxInprocServer | ClsctxInprocHandler,
			ClsctxServer = ClsctxInprocServer | ClsctxLocalServer | ClsctxRemoteServer,
			ClsctxAll = ClsctxServer | ClsctxInprocHandler
		}

		[DllImport(LibraryName, ExactSpelling = true, EntryPoint = "CoCreateInstance", PreserveSig = true)]
		private static extern int CoCreateInstance(
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
			IntPtr pUnkOuter,
			CLSCTX dwClsContext,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
			out IntPtr comObject);

		internal static void CreateComInstance(Guid clsid, CLSCTX clsctx, Guid riid, out IntPtr comObject)
		{
			var result = CoCreateInstance(clsid, IntPtr.Zero, clsctx, riid, out comObject);
			if (result < 0)
			{
				throw new System.ComponentModel.Win32Exception();
			}
		}

		internal static bool TryCreateComInstance(Guid clsid, CLSCTX clsctx, Guid riid, out IntPtr comObject)
		{
			var result = CoCreateInstance(clsid, IntPtr.Zero, clsctx, riid, out comObject);
			return result >= 0;
		}
	}

	//[ComImport]
	//[Guid("42F85136-DB7E-439C-85F1-E4075D135FC8")]
	//[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	//internal interface IFileDialog
	//{
	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	[PreserveSig()]
	//	uint Show([In, Optional] IntPtr hwndOwner); //IModalWindow 

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint SetFileTypes([In] uint cFileTypes, [In, MarshalAs(UnmanagedType.LPArray)] IntPtr rgFilterSpec);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint SetFileTypeIndex([In] uint iFileType);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint GetFileTypeIndex(out uint piFileType);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint Advise([In, MarshalAs(UnmanagedType.Interface)] IntPtr pfde, out uint pdwCookie);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint Unadvise([In] uint dwCookie);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint SetOptions([In] uint fos);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint GetOptions(out uint fos);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	void SetDefaultFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint SetFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint GetFolder([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszName);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint SetOkButtonLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszText);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint GetResult([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint AddPlace([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, uint fdap);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint SetDefaultExtension([In, MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint Close([MarshalAs(UnmanagedType.Error)] uint hr);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint SetClientGuid([In] ref Guid guid);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint ClearClientData();

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
	//}

	//[ComImport]
	//[Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
	//[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	//internal interface IShellItem
	//{
	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint BindToHandler([In] IntPtr pbc, [In] ref Guid rbhid, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IntPtr ppvOut);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint GetDisplayName([In] uint sigdnName, out IntPtr ppszName);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint GetAttributes([In] uint sfgaoMask, out uint psfgaoAttribs);

	//	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	//	uint Compare([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] uint hint, out int piOrder);

	//}

	//[ComImport, Guid("DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7")]
	//class FileDialogComObject
	//{
	//}
}
