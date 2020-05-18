using osuTK;

namespace IWBTM.Game.Rooms
{
    public class EmptyRoom : Room
    {
        public EmptyRoom()
            : base(layout, new Vector2(12, 18))
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
