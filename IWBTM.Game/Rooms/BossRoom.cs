using osuTK;

namespace IWBTM.Game.Rooms
{
    public class BossRoom : Room
    {
        public BossRoom()
            : base(layout, new Vector2(2, 18))
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
