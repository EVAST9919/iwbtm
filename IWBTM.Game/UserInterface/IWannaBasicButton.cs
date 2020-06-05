using osuTK;
using System;

namespace IWBTM.Game.UserInterface
{
    public class IWannaBasicButton : IWannaButton
    {
        public IWannaBasicButton(string text, Action action)
            : base(text, action)
        {
            Size = new Vector2(120, 40);
        }
    }
}
