using osu.Framework.Allocation;
using IWBTM.Game.Playfield;
using osuTK;
using osu.Framework.Audio.Track;

namespace IWBTM.Game
{
    public class IWBTMGame : IWBTMGameBase
    {
        private Track track;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new PlayfieldAdjustmentContainer
            {
                Child = new DefaultPlayfield()
            });

            track = Audio.Tracks.Get("Ghost Rule");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            track.Start();
        }
    }
}
