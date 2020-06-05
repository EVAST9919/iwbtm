using IWBTM.Game.Rooms;
using IWBTM.Game.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using System;

namespace IWBTM.Game.Screens.Select
{
    public class CarouselItem : ClickableContainer, IHasContextMenu
    {
        public Action<CarouselItem> Selected;
        public Action<CarouselItem> Deleted;
        public Action<CarouselItem> OnEdit;

        public Room Room { get; private set; }
        public string RoomName { get; private set; }

        public MenuItem[] ContextMenuItems => new[]
        {
            new MenuItem("Delete", onDelete),
            new MenuItem("Edit", () => OnEdit?.Invoke(this)),
        };

        private readonly Background background;

        public CarouselItem((Room, string) room)
        {
            Room = room.Item1;
            RoomName = room.Item2;

            RelativeSizeAxes = Axes.X;
            Height = 50;
            AddRangeInternal(new Drawable[]
            {
                background = new Background(),
                new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = room.Item2
                }
            });
        }

        private void onDelete() => Deleted?.Invoke(this);

        public void Select()
        {
            Selected?.Invoke(this);
            background.Activate();
        }

        public void Deselect() => background.Deactivate();

        protected override bool OnClick(ClickEvent e)
        {
            base.OnClick(e);
            Select();
            return true;
        }

        private class Background : IWannaButtonBackground
        {
            public Background()
            {
                BorderColour = IWannaColour.Blue;
            }

            public void Activate() => BorderThickness = 5;

            public void Deactivate() => BorderThickness = 0;
        }
    }
}
