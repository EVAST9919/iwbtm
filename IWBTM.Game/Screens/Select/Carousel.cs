using IWBTM.Game.Helpers;
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
using osuTK;
using osuTK.Graphics;
using System;
using System.Linq;

namespace IWBTM.Game.Screens.Select
{
    public class Carousel : CompositeDrawable
    {
        public Action<Room> OnSelection;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        private readonly FillFlowContainer<RoomItem> flow;

        public Carousel()
        {
            RelativeSizeAxes = Axes.Both;
            Padding = new MarginPadding(10);

            AddInternal(new BasicScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = flow = new FillFlowContainer<RoomItem>
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10),
                    Children = new[]
                    {
                        new RoomItem(new BossRoom())
                        {
                            Selected = room => OnSelection(room),
                            Deleted = deleteRequested
                        },
                        new RoomItem(new EmptyRoom())
                        {
                            Selected = room => OnSelection(room),
                            Deleted = deleteRequested
                        }
                    }
                }
            });

            foreach (var room in RoomStorage.GetRooms())
            {
                flow.Add(new RoomItem(room, true)
                {
                    Selected = room => OnSelection(room),
                    Deleted = deleteRequested
                });
            }
        }

        private void deleteRequested(RoomItem item, string name)
        {
            RoomStorage.DeleteRoom(name);
            notifications.Push($"{name} room has been deleted!", NotificationState.Good);
            flow.Children.FirstOrDefault().Select();
            item.Expire();
        }

        private class RoomItem : ClickableContainer, IHasContextMenu
        {
            public Action<Room> Selected;
            public Action<RoomItem, string> Deleted;

            private readonly Room room;
            private readonly bool custom;

            [Resolved]
            private NotificationOverlay notifications { get; set; }

            public MenuItem[] ContextMenuItems => new[]
            {
                new MenuItem("Delete", onDelete)
            };

            public RoomItem(Room room, bool custom = false)
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
}
