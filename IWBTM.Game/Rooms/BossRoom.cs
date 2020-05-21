using IWBTM.Game.Rooms.Drawables;
using osuTK;

namespace IWBTM.Game.Rooms
{
    public class BossRoom : Room
    {
        public BossRoom()
            : base("Miku", layout, new Vector2(2 * DrawableTile.SIZE, 17 * DrawableTile.SIZE))
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
