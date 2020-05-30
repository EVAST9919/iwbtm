using osu.Framework.Audio;
using osu.Framework.IO.Stores;
using osu.Framework.Threading;

namespace IWBTM.Game
{
    public class LevelAudioManager : AudioManager
    {
        public LevelAudioManager(AudioThread audioThread, ResourceStore<byte[]> trackStore, ResourceStore<byte[]> sampleStore)
            : base(audioThread, trackStore, sampleStore)
        {
        }
    }
}
