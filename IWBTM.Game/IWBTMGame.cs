using osu.Framework.Allocation;
using IWBTM.Game.Playfield;
using osuTK;

namespace IWBTM.Game
{
    public class IWBTMGame : IWBTMGameBase
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new PlayfieldAdjustmentContainer
            {
                Child = new DefaultPlayfield()
            });
        }
    }
}
