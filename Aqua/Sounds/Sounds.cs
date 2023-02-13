using Cosmos.System.Audio.IO;
using IL2CPU.API.Attribs;

namespace Aqua.Sounds
{
    public class Sounds
    {
        // To add an audio file, go to the file propreties in Visual Studio and make it embedded.
        [ManifestResourceStream(ResourceName = "Aqua.Sounds.Logon.wav")] static readonly byte[] logonSound;
        [ManifestResourceStream(ResourceName = "Aqua.Sounds.Logoff.wav")] static readonly byte[] logoffSound;

        public static void LogonSound()
        {
            var audioStream = MemoryAudioStream.FromWave(logonSound);
            Kernel.mixer.Streams.Add(audioStream);
        }

        public static void LogoffSound()
        {
            var audioStream = MemoryAudioStream.FromWave(logoffSound);
            Kernel.mixer.Streams.Add(audioStream);
        }
    }
}
