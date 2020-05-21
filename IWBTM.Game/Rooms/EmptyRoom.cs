using IWBTM.Game.Rooms.Drawables;
using osuTK;

namespace IWBTM.Game.Rooms
{
    public class EmptyRoom : Room
    {
        public EmptyRoom()
            : base("Empty", layout, new Vector2(12 * DrawableTile.SIZE, 17 * DrawableTile.SIZE))
        {
        }

        private static readonly string layout =
            "+XXXXXXXXXXXXXXXXXXXXXX+" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "-                      -" +
            "+XXXXXXXXXXXXXXXXXXXXXX+";
    }
}
