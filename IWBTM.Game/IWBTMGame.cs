using osu.Framework.Screens;
using IWBTM.Game.Screens;

namespace IWBTM.Game
{
    public class IWBTMGame : IWBTMGameBase
    {
        public IWBTMGame()
        {
            ScreenStack screens = new ScreenStack();
            Add(screens);
            screens.Push(new MainMenuScreen());
        }
    }
}
