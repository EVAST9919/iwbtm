using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IWBTM.Game.Screens.Select
{
    public class RoomPreviewContainer : CompositeDrawable
    {
        private Container placeholder;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Padding = new MarginPadding(10);
            AddRangeInternal(new Drawable[]
            {
                placeholder = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
                new Button(trySelectPrev)
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft
                },
                new Button(trySelectNext)
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight
                },
            });
        }

        private List<Room> rooms;
        private int selected;
        private bool showPlayerSpawn;
        private List<Vector2> deathSpots;

        public void Preview(Level level, bool showPlayerSpawn = true, List<Vector2> deathSpots = null)
        {
            rooms = level.Rooms;
            selected = 0;
            this.showPlayerSpawn = showPlayerSpawn;
            this.deathSpots = deathSpots;

            preview(rooms.ElementAt(0), showPlayerSpawn, deathSpots);
        }

        private void trySelectNext()
        {
            if (rooms == null || !rooms.Any())
                return;

            if (selected + 1 == rooms.Count)
                return;

            selected++;
            preview(rooms.ElementAt(selected), showPlayerSpawn, deathSpots);
        }

        private void trySelectPrev()
        {
            if (rooms == null || !rooms.Any())
                return;

            if (selected == 0)
                return;

            selected--;
            preview(rooms.ElementAt(selected), showPlayerSpawn, deathSpots);
        }

        private void preview(Room room, bool showPlayerSpawn, List<Vector2> deathSpots)
        {
            Container content;

            placeholder.Child = new FullRoomPreviewContainer(new Vector2(room.SizeX, room.SizeY))
            {
                Child = content = new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            };

            content.AddRange(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                new DrawableRoom(room, showPlayerSpawn)
            });

            if (deathSpots != null)
            {
                foreach (var spot in deathSpots)
                {
                    content.Add(new DeathSpot
                    {
                        Position = spot
                    });
                }
            }
        }

        private class Button : ClickableContainer
        {
            public Button(Action action)
            {
                Size = new Vector2(30, 100);
                Action = action;
                AddRange(new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black,
                        Alpha = 0.5f
                    }
                });
            }
        }
    }
}
