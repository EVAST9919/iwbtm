using IWBTM.Game.Rooms;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osuTK;
using osuTK.Graphics;
using System;
using System.IO;

namespace IWBTM.Game.Screens.Select
{
    public class Carousel : CompositeDrawable
    {
        public Action<Room> OnSelection;

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            FillFlowContainer<RoomItem> flow;

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
                        new RoomItem("Boss room", new BossRoom())
                        {
                            Selected = room => OnSelection(room)
                        },
                        new RoomItem("Empty room", new EmptyRoom())
                        {
                            Selected = room => OnSelection(room)
                        }
                    }
                }
            });

            var roomsStorage = storage.GetStorageForDirectory(@"Rooms");

            foreach (var file in roomsStorage.GetFiles(""))
            {
                using (StreamReader sr = File.OpenText(roomsStorage.GetFullPath(file)))
                {
                    var layout = sr.ReadLine();
                    var x = sr.ReadLine();
                    var y = sr.ReadLine();

                    var room = new Room(layout, new Vector2(float.Parse(x), float.Parse(y)));

                    flow.Add(new RoomItem(file, room)
                    {
                        Selected = room => OnSelection(room)
                    });
                }
            }
        }

        private class RoomItem : ClickableContainer
        {
            public Action<Room> Selected;

            private readonly Room room;

            public RoomItem(string name, Room room)
            {
                this.room = room;

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
                        Text = name,
                        Colour = Color4.Black
                    }
                });
            }

            protected override bool OnClick(ClickEvent e)
            {
                base.OnClick(e);
                Selected?.Invoke(room);
                return true;
            }
        }
    }
}
