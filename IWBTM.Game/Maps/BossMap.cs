using osuTK;

namespace IWBTM.Game.Maps
{
    public class BossMap : Map
    {
        public override Vector2 GetPlayerSpawnPosition() => new Vector2(2, 18);

        protected override string CreatePlayfield() =>
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
