using osuTK;

namespace IWBTM.Game.Maps
{
    public class EmptyMap : Map
    {
        public override Vector2 GetPlayerSpawnPosition() => new Vector2(12, 18);

        protected override string CreatePlayfield() =>
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
