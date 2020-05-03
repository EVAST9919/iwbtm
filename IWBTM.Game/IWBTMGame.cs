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
            Add(new DefaultPlayfield()
            {
                Scale = new Vector2(2) // Temp
            });
        }
    }
}
