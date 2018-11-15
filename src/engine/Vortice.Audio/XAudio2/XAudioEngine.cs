// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.


using SharpDX.XAudio2;

namespace Vortice.Audio.XAudio
{
    internal unsafe class XAudioEngine : IAudioEngine
    {
        public readonly XAudio2 XAudio;
        public readonly MasteringVoice MasteringVoice;

        public XAudioEngine(bool validation = true)
        {
#if VORTICE_PLATFORM_UWP
            validation = false;
#endif

            try
            {
                // Fails if the XAudio2 SDK is not installed.
                XAudio = new XAudio2(
                    validation ? XAudio2Flags.DebugEngine : XAudio2Flags.None,
                    ProcessorSpecifier.AnyProcessor
                    );
                XAudio.StartEngine();
            }
            catch
            {
                validation = false;
                XAudio = new XAudio2(XAudio2Flags.None, ProcessorSpecifier.DefaultProcessor);
                XAudio.StartEngine();
            }

            MasteringVoice = new MasteringVoice(XAudio, 2, 44100);
        }

        public void Destroy()
        {
            MasteringVoice.Dispose();
            XAudio.StopEngine();
        }
    }
}
