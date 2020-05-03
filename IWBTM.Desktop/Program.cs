using IWBTM.Game;
using osu.Framework;
using osu.Framework.Platform;

namespace IWBTM.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using GameHost host = Host.GetSuitableHost(@"IWBTM");

            using osu.Framework.Game game = new IWBTMGame();
            host.Run(game);
        }
    }
}
