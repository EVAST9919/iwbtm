﻿using IWBTM.Game;
using osu.Framework;
using osu.Framework.Platform;

namespace IWBTM.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            GameHost host = Host.GetSuitableHost(@"IWBTM");
            host.Run(new IWBTMGame());
        }
    }
}
