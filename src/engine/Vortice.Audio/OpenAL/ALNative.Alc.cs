// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Vortice.Audio.OpenAL
{
    public static unsafe partial class ALNative
    {
        public const int ALC_FALSE = 0x0000;
        public const int ALC_TRUE = 0x0001;
        public const int ALC_FREQUENCY = 0x1007;
        public const int ALC_REFRESH = 0x1008;
        public const int ALC_SYNC = 0x1009;

        public const int ALC_MONO_SOURCES = 0x1010;
        public const int ALC_STEREO_SOURCES = 0x1011;

        public const int ALC_NO_ERROR = 0x0000;
        public const int ALC_INVALID_DEVICE = 0xA001;
        public const int ALC_INVALID_CONTEXT = 0xA002;
        public const int ALC_INVALID_ENUM = 0xA003;
        public const int ALC_INVALID_VALUE = 0xA004;
        public const int ALC_OUT_OF_MEMORY = 0xA005;

        public const int ALC_MAJOR_VERSION = 0x1000;
        public const int ALC_MINOR_VERSION = 0x1001;

        public const int ALC_ATTRIBUTES_SIZE = 0x1002;
        public const int ALC_ALL_ATTRIBUTES = 0x1003;
        public const int ALC_DEFAULT_DEVICE_SPECIFIER = 0x1004;
        public const int ALC_DEVICE_SPECIFIER = 0x1005;
        public const int ALC_EXTENSIONS = 0x1006;

        public const int ALC_EXT_CAPTURE = 1;
        public const int ALC_CAPTURE_DEVICE_SPECIFIER = 0x310;
        public const int ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER = 0x311;
        public const int ALC_CAPTURE_SAMPLES = 0x312;

        public const int ALC_ENUMERATE_ALL_EXT = 1;
        public const int ALC_DEFAULT_ALL_DEVICES_SPECIFIER = 0x1012;
        public const int ALC_ALL_DEVICES_SPECIFIER = 0x1013;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr alcCreateContext_t(IntPtr device, int[] attribs);
        private static alcCreateContext_t s_alc_createContext;
        public static IntPtr alcCreateContext(IntPtr device, int[] attribs) => s_alc_createContext(device, attribs);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alcMakeContextCurrent_t(IntPtr context);
        private static alcMakeContextCurrent_t s_alc_makeContextCurrent;
        public static void alcMakeContextCurrent(IntPtr context) => s_alc_makeContextCurrent(context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alcProcessContext_t(IntPtr context);
        private static alcProcessContext_t s_alc_processContext;
        public static void alcProcessContext(IntPtr context) => s_alc_processContext(context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alcSuspendContext_t(IntPtr context);
        private static alcSuspendContext_t s_alc_suspendContext;
        public static void alcSuspendContext(IntPtr context) => s_alc_suspendContext(context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alcDestroyContext_t(IntPtr context);
        private static alcDestroyContext_t s_alc_destroyContext;
        public static void alcDestroyContext(IntPtr handle) => s_alc_destroyContext(handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr alcGetCurrentContext_t();
        private static alcGetCurrentContext_t s_alcGetCurrentContext;
        public static IntPtr alcGetCurrentContext() => s_alcGetCurrentContext();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr alcGetContextsDevice_t(IntPtr context);
        private static alcGetContextsDevice_t s_alcGetContextsDevice;
        public static IntPtr alcGetContextsDevice(IntPtr context) => s_alcGetContextsDevice(context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr alcOpenDevice_t(string name);
        private static alcOpenDevice_t s_alc_openDevice;
        public static IntPtr alcOpenDevice(string name) => s_alc_openDevice(name);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alcCloseDevice_t(IntPtr handle);
        private static alcCloseDevice_t s_alc_closeDevice;
        public static void alcCloseDevice(IntPtr handle) => s_alc_closeDevice(handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int alcGetError_t(IntPtr device);
        private static alcGetError_t s_alc_getError;
        public static int alcGetError(IntPtr device) => s_alc_getError(device);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte alcIsExtensionPresent_t(IntPtr device, string extensionName);
        private static alcIsExtensionPresent_t s_alcIsExtensionPresent;
        public static bool alcIsExtensionPresent(IntPtr device, string extensionName) => s_alcIsExtensionPresent(device, extensionName) == 1;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr alcGetProcAddress_t(IntPtr device, string funcName);
        private static alcGetProcAddress_t s_alcGetProcAddress;
        public static IntPtr alcGetProcAddress(IntPtr device, string funcName) => s_alcGetProcAddress(device, funcName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int alcGetEnumValue_t(IntPtr device, string enumName);
        private static alcGetEnumValue_t s_alcGetEnumValue;
        public static int alcGetEnumValue(IntPtr device, string enumName) => s_alcGetEnumValue(device, enumName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr alcGetString_t(IntPtr device, int param);
        private static alcGetString_t s_alcGetString;
        public static string alcGetString(IntPtr device, int param) => Marshal.PtrToStringAnsi(s_alcGetString(device, param));

        public static string[] alcGetStringv(IntPtr device, int param)
        {
            return GetStringArray(s_alcGetString(device, param));
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alcGetIntegerv_t(IntPtr device, int param, int size, int[] values);
        private static alcGetIntegerv_t s_alcGetIntegerv;
        public static void alcGetIntegerv(IntPtr device, int param, int size, int[] values) => s_alcGetIntegerv(device, param, size, values);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr alcCaptureOpenDevice_t(string deviceName, uint frequency, int format, int bufferSize);
        private static alcCaptureOpenDevice_t s_alcCaptureOpenDevice;
        public static IntPtr alcCaptureOpenDevice(string deviceName, uint frequency, int format, int bufferSize)
        {
            return s_alcCaptureOpenDevice(deviceName, frequency, format, bufferSize);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte alcCaptureCloseDevice_t(IntPtr device);
        private static alcCaptureCloseDevice_t s_alcCaptureCloseDevice;
        public static bool alcCaptureCloseDevice(IntPtr device) => s_alcCaptureCloseDevice(device) == 1;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alcCaptureStart_t(IntPtr device);
        private static alcCaptureStart_t s_alcCaptureStart;
        public static void alcCaptureStart(IntPtr device) => s_alcCaptureStart(device);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alcCaptureStop_t(IntPtr device);
        private static alcCaptureStop_t s_alcCaptureStop;
        public static void alcCaptureStop(IntPtr device) => s_alcCaptureStop(device);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alcCaptureSamples_t(IntPtr device, IntPtr buffer, int samples);
        private static alcCaptureSamples_t s_alcCaptureSamples;
        public static void alcCaptureSamples(IntPtr device, IntPtr buffer, int samples) => s_alcCaptureSamples(device, buffer, samples);

        private static void LoadAlc()
        {
            s_alc_createContext = LoadFunction<alcCreateContext_t>(nameof(alcCreateContext));
            s_alc_makeContextCurrent = LoadFunction<alcMakeContextCurrent_t>(nameof(alcMakeContextCurrent));
            s_alc_processContext = LoadFunction<alcProcessContext_t>(nameof(alcProcessContext));
            s_alc_suspendContext = LoadFunction<alcSuspendContext_t>(nameof(alcSuspendContext));
            s_alc_destroyContext = LoadFunction<alcDestroyContext_t>(nameof(alcDestroyContext));
            s_alcGetCurrentContext = LoadFunction<alcGetCurrentContext_t>(nameof(alcGetCurrentContext));
            s_alcGetContextsDevice = LoadFunction<alcGetContextsDevice_t>(nameof(alcGetContextsDevice));

            s_alc_openDevice = LoadFunction<alcOpenDevice_t>(nameof(alcOpenDevice));
            s_alc_closeDevice = LoadFunction<alcCloseDevice_t>(nameof(alcCloseDevice));

            s_alc_getError = LoadFunction<alcGetError_t>(nameof(alcGetError));

            s_alcIsExtensionPresent = LoadFunction<alcIsExtensionPresent_t>(nameof(alcIsExtensionPresent));
            s_alcGetProcAddress = LoadFunction<alcGetProcAddress_t>(nameof(alcGetProcAddress));
            s_alcGetEnumValue = LoadFunction<alcGetEnumValue_t>(nameof(alcGetEnumValue));
            s_alcGetString = LoadFunction<alcGetString_t>(nameof(alcGetString));
            s_alcGetIntegerv = LoadFunction<alcGetIntegerv_t>(nameof(alcGetIntegerv));

            s_alcCaptureOpenDevice = LoadFunction<alcCaptureOpenDevice_t>(nameof(alcCaptureOpenDevice));
            s_alcCaptureCloseDevice = LoadFunction<alcCaptureCloseDevice_t>(nameof(alcCaptureCloseDevice));
            s_alcCaptureStart = LoadFunction<alcCaptureStart_t>(nameof(alcCaptureStart));
            s_alcCaptureStop = LoadFunction<alcCaptureStop_t>(nameof(alcCaptureStop));
            s_alcCaptureSamples = LoadFunction<alcCaptureSamples_t>(nameof(alcCaptureSamples));
        }

        private static string[] GetStringArray(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            var result = new List<string>();
            var builder = new StringBuilder();
            int index = 0;
            while (true)
            {
                char ch = (char)Marshal.ReadByte(ptr, index);
                if (ch == '\0')
                {
                    if (builder.Length == 0)
                    {
                        break;
                    }
                    result.Add(builder.ToString());
                    builder.Length = 0;
                }
                else
                {
                    builder.Append(ch);
                }
                index++;
            }

            return result.ToArray();
        }
    }
}
