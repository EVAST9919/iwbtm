using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics;
using IWBTM.Game.Rooms;

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
