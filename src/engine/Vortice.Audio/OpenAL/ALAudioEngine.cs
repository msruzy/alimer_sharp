// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using static Vortice.Audio.OpenAL.ALNative;

namespace Vortice.Audio.OpenAL
{
    public class ALAudioEngine : IAudioEngine
    {
        private IntPtr _device;
        private IntPtr _context;

        public bool Initialize()
        {
            try
            {
                if (alcIsExtensionPresent(IntPtr.Zero, "ALC_ENUMERATE_ALL_EXT"))
                {
                    var defaultDevice = alcGetString(IntPtr.Zero, ALC_DEFAULT_ALL_DEVICES_SPECIFIER);
                    var defaultDevices = alcGetStringv(IntPtr.Zero, ALC_ALL_DEVICES_SPECIFIER);
                }

                if (alcIsExtensionPresent(IntPtr.Zero, "ALC_EXT_CAPTURE"))
                {
                    var defaultCaptureDevice = alcGetString(IntPtr.Zero, ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER);
                    var captureDevices = alcGetStringv(IntPtr.Zero, ALC_CAPTURE_DEVICE_SPECIFIER);
                }

                _device = alcOpenDevice(null);
                if (_device == IntPtr.Zero)
                {
                    return false;
                }

                _context = alcCreateContext(_device, null);
                if (_context == IntPtr.Zero)
                {
                    return false;
                }

                alcMakeContextCurrent(_context);
                //_floatSupport = alIsExtensionPresent("AL_EXT_FLOAT32");

                int error = alcGetError(_device);
                if (error != ALC_NO_ERROR)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void Shutdown()
        {
            alcDestroyContext(_context);
            alcCloseDevice(_device);
        }

        [Conditional("DEBUG")]
        private void CheckAlcError()
        {
            int error = alcGetError(_device);
            if (error != ALC_NO_ERROR)
            {
                //throw new AudioException("OpenAL Error: " + error);
            }
        }
    }
}
