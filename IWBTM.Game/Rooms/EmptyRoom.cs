using IWBTM.Game.Playfield;
using osuTK;

namespace IWBTM.Game.Rooms
{
    public class EmptyRoom : Room
    {
        public EmptyRoom()
            : base(layout, new Vector2(12 * Tile.SIZE, 18 * Tile.SIZE))
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
