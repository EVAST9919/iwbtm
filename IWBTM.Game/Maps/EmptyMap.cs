namespace IWBTM.Game.Maps
{
    public class EmptyMap : Map
    {
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
