using IWBTM.Game.Player;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osuTK;
using osu.Framework.Utils;
using IWBTM.Game.Playfield;

namespace IWBTM.Game.Objects
{
    public class ObjectsController : CompositeDrawable
    {
        private readonly DefaultPlayer player;

        public ObjectsController(DefaultPlayer player)
        {
            this.player = player;
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            loadLevel();
        }

        private void loadLevel()
        {
            for (int i = 0; i < 100; i++)
            {
                var startTime = RNG.NextDouble() * 60000;
                var xPos = RNG.NextDouble() * DefaultPlayfield.WIDTH * Tile.SIZE;
                var yPos = RNG.NextDouble() * DefaultPlayfield.HEIGHT * Tile.SIZE;

                var cherry = new Cherry
                {
                    StartTime = startTime,
                    Position = new Vector2((float)xPos, (float)yPos),
                    Scale = new Vector2(0),
                };

                AddInternal(cherry);
            }
        }
    }
}
