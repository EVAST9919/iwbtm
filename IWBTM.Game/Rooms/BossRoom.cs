using IWBTM.Game.Screens.Play.Playfield;
using osuTK;

namespace IWBTM.Game.Rooms
{
    public class BossRoom : Room
    {
        public BossRoom()
            : base("Miku", layout, new Vector2(2 * Tile.SIZE, 17 * Tile.SIZE))
        {
        }

        private static readonly string layout =
            "+XXXXXXXXXXXXXXXXXXXXXX+" +
            "-                      -" +
            "-                      -" +
            "-  +XXX+  +XXX+        -" +
            "-                      -" +
            "-                      -" +
            "-  +XXX+               -" +
            "-         +XXX+        -" +
            "-                      -" +
            "-  +XXX+               -" +
            "-                      -" +
            "-         +XXX+        -" +
            "-  +XXX+               -" +
            "-                      -" +
            "-                      -" +
            "-  +XXX+  +XXX+        -" +
            "-                      -" +
            "-                      -" +
            "+XXXXXXXXXXXXXXXXXXXXXX+";
    }
}
