using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace IWBTM.Game.Screens.Create
{
    public class SkinSelector : BasicDropdown<string>
    {
        public SkinSelector()
        {
            RelativeSizeAxes = Axes.X;

            AddDropdownItem("Default");
            AddDropdownItem("Avoider");
            AddDropdownItem("Zeus_red");
            AddDropdownItem("Zeus_blue");
            AddDropdownItem("Zelda2");
            AddDropdownItem("blowgame");
            AddDropdownItem("biker_brad");
            AddDropdownItem("k3s4");

            Current.Value = "Default";
        }
    }
}
