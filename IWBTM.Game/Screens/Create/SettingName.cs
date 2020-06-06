using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;

namespace IWBTM.Game.Screens.Create
{
    public class SettingName : SpriteText
    {
        public SettingName(string text)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Text = text;
        }
    }
}
