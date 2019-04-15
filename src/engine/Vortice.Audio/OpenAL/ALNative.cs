// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Vortice.Audio.OpenAL
{
    public static unsafe partial class ALNative
    {
        private static readonly IALNativeLibrary s_alLibrary;

        private static IALNativeLibrary LoadOpenAL()
        {
            string[] names;
            if (Platform.PlatformType == PlatformType.Windows)
            {
                names = new[] {
                    "soft_oal.dll",
                    "OpenAL32.dll"
                };
                return new Win32ALLibraryLoader(names);
            }
            else if (Platform.PlatformType == PlatformType.Linux)
            {
                names = new[]
                {
                    "libopenal.so",
                    "libopenal.so.1"
                };
                return new UnixALLibraryLoader(names);
            }
            else if (Platform.PlatformType == PlatformType.macOS)
            {
                names = new[] { "libopenal.dylib" };
                return new UnixALLibraryLoader(names);
            }
            else
            {
                Debug.WriteLine("Unknown OpenAL platform. Attempting to load \"OpenAL\"");
                names = new[] { "OpenAL" };
                return new UnixALLibraryLoader(names);
            }
        }

        static ALNative()
        {
            s_alLibrary = LoadOpenAL();

            LoadAlc();
            LoadAl();
        }

        private static T LoadFunction<T>(string name) => s_alLibrary.LoadFunction<T>(name);

        internal interface IALNativeLibrary
        {
            T LoadFunction<T>(string name);
        }

        private class Win32ALLibraryLoader : IALNativeLibrary
        {
            private readonly IntPtr _handle;

            public Win32ALLibraryLoader(string[] names)
            {
                foreach (var name in names)
                {
                    if (_handle != IntPtr.Zero)
                    {
                        break;
                    }

                    foreach (var libraryName in GetLookupPaths(name))
                    {
                        _handle = LoadLibrary(libraryName);
                        if (_handle != IntPtr.Zero)
                        {
                            break;
                        }
                    }
                }
            }

            public T LoadFunction<T>(string name)
            {
                var functionPtr = GetProcAddress(_handle, name);
                if (functionPtr == IntPtr.Zero)
                {
                    throw new InvalidOperationException($"No function was found with the name {name}.");
                }

                return Marshal.GetDelegateForFunctionPointer<T>(functionPtr);
            }

            private static IEnumerable<string> GetLookupPaths(string name)
            {
                yield return Path.Combine(AppContext.BaseDirectory, name);
                yield return name;
                yield return Path.Combine(AppContext.BaseDirectory, IntPtr.Size == 8 ? "win-x64" : "win-x86", name);
            }

            [DllImport("kernel32")]
            public static extern IntPtr LoadLibrary(string fileName);

            [DllImport("kernel32")]
            public static extern IntPtr GetProcAddress(IntPtr module, string procName);

            [DllImport("kernel32")]
            public static extern int FreeLibrary(IntPtr module);
        }

        private class UnixALLibraryLoader : IALNativeLibrary
        {
            private readonly IntPtr _handle;
            private const string LibName = "libdl";
            private const int RTLD_NOW = 0x002;

            public UnixALLibraryLoader(string[] names)
            {
                foreach (var name in names)
                {
                    _handle = dlopen(name, RTLD_NOW);
                    if (_handle != IntPtr.Zero)
                    {
                        break;
                    }
                }
            }

            public T LoadFunction<T>(string name)
            {
                var functionPtr = dlsym(_handle, name);
                if (functionPtr == IntPtr.Zero)
                {
                    throw new InvalidOperationException($"No function was found with the name {name}.");
                }

                return Marshal.GetDelegateForFunctionPointer<T>(functionPtr);
            }

            [DllImport(LibName)]
            public static extern IntPtr dlopen(string fileName, int flags);

            [DllImport(LibName)]
            public static extern IntPtr dlsym(IntPtr handle, string name);

            [DllImport(LibName)]
            public static extern int dlclose(IntPtr handle);

            [DllImport(LibName)]
            public static extern string dlerror();
        }
    }
}
