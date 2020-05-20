using IWBTM.Game.Overlays;
using IWBTM.Game.Rooms;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK.Graphics;
using System;

namespace IWBTM.Game.Screens.Select
{
    public class CarouselRoomItem : ClickableContainer, IHasContextMenu
    {
        public Action<Room> Selected;
        public Action<CarouselRoomItem, string> Deleted;
        public Action<Room> OnEdit;

        private readonly Room room;
        private readonly bool custom;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        public MenuItem[] ContextMenuItems => new[]
        {
            new MenuItem("Delete", onDelete),
            new MenuItem("Edit", () => OnEdit?.Invoke(room)),
        };

        public CarouselRoomItem(Room room, bool custom = false)
        {
            this.room = room;
            this.custom = custom;

            RelativeSizeAxes = Axes.X;
            Height = 50;
            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = room.Name,
                    Colour = Color4.Black
                }
            });
        }

        private void onDelete()
        {
            if (!custom)
            {
                notifications.Push("Can't delete not custom room.", NotificationState.Bad);
                return;
            }

            Deleted?.Invoke(this, room.Name);
        }

        public void Select() => Selected?.Invoke(room);

        protected override bool OnClick(ClickEvent e)
        {
            base.OnClick(e);
            Select();
            return true;
        }
    }
}
