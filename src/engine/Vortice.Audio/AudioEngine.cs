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
                backend = AudioBackend.OpenAL;
            }

            switch (backend)
            {
                case AudioBackend.OpenAL:
#if !VORTICE_NO_OPENAL
                    _backendEngine = new OpenAL.ALAudioEngine();
#else
                    throw new AudioException($"{AudioBackend.XAudio2} Backend is not supported");
#endif
                    break;
            }

            if (!_backendEngine.Initialize())
            {
                //throw new AudioException($"{backend} Backend is not supported");
            }
        }
    }

    public interface IAudioEngine
    {
        bool Initialize();
        void Shutdown();
    }
}
