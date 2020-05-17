using osuTK;

namespace IWBTM.Game.Rooms
{
    public class EmptyRoom : Room
    {
        public override Vector2 GetPlayerSpawnPosition() => new Vector2(12, 18);

        protected override string CreateLayout() =>
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
