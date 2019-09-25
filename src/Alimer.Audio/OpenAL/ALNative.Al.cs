// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Alimer.Audio.OpenAL
{
    public static unsafe partial class ALNative
    {
        /* typedef int ALenum; */
        public const int AL_NONE = 0x0000;
        public const int AL_FALSE = 0x0000;
        public const int AL_TRUE = 0x0001;

        public const int AL_SOURCE_RELATIVE = 0x0202;

        public const int AL_CONE_INNER_ANGLE = 0x1001;
        public const int AL_CONE_OUTER_ANGLE = 0x1002;

        public const int AL_PITCH = 0x1003;
        public const int AL_POSITION = 0x1004;
        public const int AL_DIRECTION = 0x1005;
        public const int AL_VELOCITY = 0x1006;
        public const int AL_LOOPING = 0x1007;
        public const int AL_BUFFER = 0x1009;
        public const int AL_GAIN = 0x100A;
        public const int AL_MIN_GAIN = 0x100D;
        public const int AL_MAX_GAIN = 0x100E;
        public const int AL_ORIENTATION = 0x100F;

        public const int AL_SOURCE_STATE = 0x1010;
        public const int AL_INITIAL = 0x1011;
        public const int AL_PLAYING = 0x1012;
        public const int AL_PAUSED = 0x1013;
        public const int AL_STOPPED = 0x1014;

        public const int AL_BUFFERS_QUEUED = 0x1015;
        public const int AL_BUFFERS_PROCESSED = 0x1016;

        public const int AL_REFERENCE_DISTANCE = 0x1020;
        public const int AL_ROLLOFF_FACTOR = 0x1021;
        public const int AL_CONE_OUTER_GAIN = 0x1022;

        public const int AL_MAX_DISTANCE = 0x1023;

        public const int AL_SOURCE_TYPE = 0x1027;
        public const int AL_STATIC = 0x1028;
        public const int AL_STREAMING = 0x1029;
        public const int AL_UNDETERMINED = 0x1030;

        public const int AL_FORMAT_MONO8 = 0x1100;
        public const int AL_FORMAT_MONO16 = 0x1101;
        public const int AL_FORMAT_STEREO8 = 0x1102;
        public const int AL_FORMAT_STEREO16 = 0x1103;

        public const int AL_FREQUENCY = 0x2001;
        public const int AL_BITS = 0x2002;
        public const int AL_CHANNELS = 0x2003;
        public const int AL_SIZE = 0x2004;

        public const int AL_NO_ERROR = 0x0000;
        public const int AL_INVALID_NAME = 0xA001;
        public const int AL_INVALID_ENUM = 0xA002;
        public const int AL_INVALID_VALUE = 0xA003;
        public const int AL_INVALID_OPERATION = 0xA004;
        public const int AL_OUT_OF_MEMORY = 0xA005;

        public const int AL_VENDOR = 0xB001;
        public const int AL_VERSION = 0xB002;
        public const int AL_RENDERER = 0xB003;
        public const int AL_EXTENSIONS = 0xB004;

        public const int AL_DOPPLER_FACTOR = 0xC000;
        /* Deprecated! */
        public const int AL_DOPPLER_VELOCITY = 0xC001;

        public const int AL_DISTANCE_MODEL = 0xD000;
        public const int AL_INVERSE_DISTANCE = 0xD001;
        public const int AL_INVERSE_DISTANCE_CLAMPED = 0xD002;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int alGetError_t();
        private static alGetError_t s_alGetError;
        public static int alGetError() => s_alGetError();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr alGetString_t(int param);
        private static alGetString_t s_alGetString;
        public static string alGetString(int param) => Marshal.PtrToStringAnsi(s_alGetString(param));

        /* n refers to an ALsizei */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alGenBuffers_t(int n, uint[] buffers);
        private static alGenBuffers_t s_alGenBuffers;
        public static void alGenBuffers(int n, uint[] buffers) => s_alGenBuffers(n, buffers);

        /* n refers to an ALsizei */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alDeleteBuffers_t(int n, uint[] buffers);
        private static alDeleteBuffers_t s_alDeleteBuffers;
        public static void alDeleteBuffers(int n, uint[] buffers) => s_alDeleteBuffers(n, buffers);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte alIsBuffer_t(uint buffer);
        private static alIsBuffer_t s_alIsBuffer;
        public static bool alIsBuffer(uint buffer) => s_alIsBuffer(buffer) == 1;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alBufferData_t(uint buffer, int format, IntPtr data, int size, int freq);
        private static alBufferData_t s_alBufferData;
        public static void alBufferData(uint buffer, int format, IntPtr data, int size, int freq) => s_alBufferData(buffer, format, data, size, freq);

        /* n refers to an ALsizei */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alGenSources_t(int n, uint[] sources);
        private static alGenSources_t s_alGenSources;
        public static void alGenSources(int n, uint[] sources) => s_alGenSources(n, sources);

        /* n refers to an ALsizei */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alDeleteSources_t(int n, uint[] sources);
        private static alDeleteSources_t s_alDeleteSources;
        public static void alDeleteSources(int n, uint[] sources) => s_alDeleteSources(n, sources);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alSourceQueueBuffers_t(uint source, int n, uint[] buffers);
        private static alSourceQueueBuffers_t s_alSourceQueueBuffers;
        public static void alSourceQueueBuffers(uint source, int n, uint[] sources) => s_alSourceQueueBuffers(source, n, sources);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alSourceUnqueueBuffers_t(uint source, int n, uint[] buffers);
        private static alSourceUnqueueBuffers_t s_alSourceUnqueueBuffers;
        public static void alSourceUnqueueBuffers(uint source, int n, uint[] sources) => s_alSourceUnqueueBuffers(source, n, sources);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alSourcePlay_t(uint source);
        private static alSourcePlay_t s_alSourcePlay;
        public static void alSourcePlay(uint source) => s_alSourcePlay(source);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alSourceStop_t(uint source);
        private static alSourceStop_t s_alSourceStop;
        public static void alSourceStop(uint source) => s_alSourceStop(source);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alSourcei_t(uint source, int param, int value);
        private static alSourcei_t s_alSourcei;
        public static void alSourcei(uint source, int param, int value) => s_alSourcei(source, param, value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alGetSourcei_t(uint source, int param, out int value);
        private static alGetSourcei_t s_alGetSourcei;
        public static void alGetSourcei(uint source, int param, out int value) => s_alGetSourcei(source, param, out value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alSourcef_t(uint source, int param, float value);
        private static alSourcef_t s_alSourcef;
        public static void alSourcef(uint source, int param, float value) => s_alSourcef(source, param, value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void alGetSourcef_t(uint source, int param, out float value);
        private static alGetSourcef_t s_alGetSourcef;
        public static void alGetSourcef(uint source, int param, out float value) => s_alGetSourcef(source, param, out value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool alIsExtensionPresent_t(string extname);
        private static alIsExtensionPresent_t s_alIsExtensionPresent;
        public static bool alIsExtensionPresent(string extname) => s_alIsExtensionPresent(extname);

        private static void LoadAl()
        {
            s_alGetError = LoadFunction<alGetError_t>(nameof(alGetError));
            s_alGetString = LoadFunction<alGetString_t>(nameof(alGetString));

            s_alIsExtensionPresent = LoadFunction<alIsExtensionPresent_t>(nameof(alIsExtensionPresent));

            s_alGenBuffers = LoadFunction<alGenBuffers_t>(nameof(alGenBuffers));
            s_alDeleteBuffers = LoadFunction<alDeleteBuffers_t>(nameof(alDeleteBuffers));
            s_alIsBuffer = LoadFunction<alIsBuffer_t>(nameof(alIsBuffer));

            s_alBufferData = LoadFunction<alBufferData_t>(nameof(alBufferData));

            s_alGenSources = LoadFunction<alGenSources_t>(nameof(alGenSources));
            s_alDeleteSources = LoadFunction<alDeleteSources_t>(nameof(alDeleteSources));

            s_alSourcePlay = LoadFunction<alSourcePlay_t>(nameof(alSourcePlay));
            s_alSourceStop = LoadFunction<alSourceStop_t>(nameof(alSourceStop));
            s_alSourceQueueBuffers = LoadFunction<alSourceQueueBuffers_t>(nameof(alSourceQueueBuffers));
            s_alSourceUnqueueBuffers = LoadFunction<alSourceUnqueueBuffers_t>(nameof(alSourceUnqueueBuffers));

            s_alSourcei = LoadFunction<alSourcei_t>(nameof(alSourcei));
            s_alGetSourcei = LoadFunction<alGetSourcei_t>(nameof(alGetSourcei));

            s_alSourcef = LoadFunction<alSourcef_t>(nameof(alSourcef));
            s_alGetSourcef = LoadFunction<alGetSourcef_t>(nameof(alGetSourcef));
        }
    }
}
