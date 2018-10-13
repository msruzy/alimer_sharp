// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Audio
{
    /// <summary>
    /// Defines an audio engine.
    /// </summary>
    public sealed class AudioEngine
    {
        private readonly IAudioEngine _backendEngine;

        public AudioEngine(AudioBackend backend = AudioBackend.Default)
        {
            if (backend == AudioBackend.Default)
            {
                backend = AudioBackend.XAudio2;
            }

            switch (backend)
            {
                case AudioBackend.XAudio2:
#if !VORTICE_NO_XAUDIO2
                    _backendEngine = new XAudio.XAudioEngine();
#else
                    throw new AudioException($"{AudioBackend.XAudio2} Backend is not supported");
#endif
                    break;
            }
        }
    }

    public interface IAudioEngine
    {
    }
}
