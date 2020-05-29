using IWBTM.Game.Rooms;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace IWBTM.Game.Screens.Edit
{
    public class MusicSelector : BasicDropdown<string>
    {
        private readonly Room room;

        public MusicSelector(Room room)
        {
            this.room = room;

            RelativeSizeAxes = Axes.X;

            AddDropdownItem("none");
            AddDropdownItem("room-1");
            AddDropdownItem("room-2");
            AddDropdownItem("room-3");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (room != null)
            {
                if (!string.IsNullOrEmpty(room.Music))
                {
                    Current.Value = room.Music;
                    return;
                }
            }

            Current.Value = "none";
        }
    }
}
