using IWBTM.Game.Rooms;
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
        public Action<CarouselRoomItem> Selected;
        public Action<CarouselRoomItem> Deleted;
        public Action<CarouselRoomItem> OnEdit;

        public Room Room { get; private set; }
        public string RoomName { get; private set; }

        public MenuItem[] ContextMenuItems => new[]
        {
            new MenuItem("Delete", onDelete),
            new MenuItem("Edit", () => OnEdit?.Invoke(this)),
        };

        public CarouselRoomItem((Room, string) room)
        {
            Room = room.Item1;
            RoomName = room.Item2;

            RelativeSizeAxes = Axes.X;
            Height = 50;
            Masking = true;
            BorderColour = Color4.Red;
            CornerRadius = 7;
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
                    Text = room.Item2,
                    Colour = Color4.Black
                }
            });
        }

        private void onDelete() => Deleted?.Invoke(this);

        public void Select()
        {
            Selected?.Invoke(this);
            BorderThickness = 5;
        }

        public void Deselect()
        {
            BorderThickness = 0;
        }

        protected override bool OnClick(ClickEvent e)
        {
            base.OnClick(e);
            Select();
            return true;
        }
    }
}
