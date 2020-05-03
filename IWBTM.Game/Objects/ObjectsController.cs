using IWBTM.Game.Player;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;

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
    }
}
