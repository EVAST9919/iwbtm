using IWBTM.Game.UserInterface;
using osuTK;
using System;

namespace IWBTM.Game.Screens.Edit
{
    public class EditorButton : IWannaButton
    {
        public EditorButton(string text, Action action)
            : base(text, action)
        {
            Size = new Vector2(100, 50);
        }
    }
}
