using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace IWBTM.Game.Screens.Select
{
    public class RoomPreviewContainer : CompositeDrawable
    {
        private readonly PlayfieldAdjustmentContainer roomPreview;

        public RoomPreviewContainer()
        {
            RelativeSizeAxes = Axes.Both;
            Padding = new MarginPadding(10);

            AddInternal(roomPreview = new PlayfieldAdjustmentContainer());
        }

        public void Preview(Room room)
        {
            roomPreview.Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                new DrawableRoom(room, true)
            };
        }
    }
}
