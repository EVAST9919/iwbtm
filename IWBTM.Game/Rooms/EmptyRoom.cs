using IWBTM.Game.Screens.Play.Playfield;
using osuTK;

namespace IWBTM.Game.Rooms
{
    public class EmptyRoom : Room
    {
        public EmptyRoom()
            : base("Empty", layout, new Vector2(12 * Tile.SIZE, 17 * Tile.SIZE))
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
