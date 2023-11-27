using Cosmos.HAL.Drivers.Audio;
using Cosmos.System.Audio.IO;
using Cosmos.System.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IL2CPU.API.Attribs;

namespace Aqua.Sounds
{
    public class Audio
    {
        public Audio() { }

        public static AudioMixer mixer = new AudioMixer();
        public static MemoryAudioStream audioStream = MemoryAudioStream.FromWave(logonAudio);
        public static AC97 driver = AC97.Initialize(4096);

        [ManifestResourceStream(ResourceName = "Aqua.Sounds.Logon.wav")]
        public static byte[] logonAudio;

        public static AudioManager audioManager = new AudioManager()
        {
            Stream = mixer,
            Output = driver
        };

        public static void Initialize()
        {
            mixer.Streams.Add(audioStream);
            audioManager.Enable();
        }
    }
}
