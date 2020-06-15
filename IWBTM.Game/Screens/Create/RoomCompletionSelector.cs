using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace IWBTM.Game.Screens.Create
{
    public class RoomCompletionSelector : BasicDropdown<RoomCompletionType>
    {
        public RoomCompletionSelector()
        {
            RelativeSizeAxes = Axes.X;

            AddDropdownItem(RoomCompletionType.Warp);
            AddDropdownItem(RoomCompletionType.BoundaryExit);

            Current.TriggerChange();
        }
    }

    public enum RoomCompletionType
    {
        Warp,
        BoundaryExit
    }
}
