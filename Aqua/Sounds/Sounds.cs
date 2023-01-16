using Cosmos.System.Audio;
using Cosmos.System.Audio.IO;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqua.Sounds
{
    public class Sounds
    {
        // To add an audio file, go to the file propreties in Visual Studio and make it embedded.
        [ManifestResourceStream(ResourceName = "Aqua.Sounds.Startup.wav")] static byte[] sound;
        
        public static void StartupSound()
        {
            var audioStream = MemoryAudioStream.FromWave(sound);
            Kernel.mixer.Streams.Add(audioStream);
        }
    }
}
