using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace IWBTM.Game.Screens.Create
{
    public class MusicSelector : BasicDropdown<string>
    {
        public MusicSelector()
        {
            RelativeSizeAxes = Axes.X;

            AddDropdownItem("none");
            AddDropdownItem("room-1");
            AddDropdownItem("room-2");
            AddDropdownItem("room-3");

            Current.Value = "none";
        }
    }
}
