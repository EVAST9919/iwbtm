using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IWBTM.Game.Screens.Select
{
    public class LevelPreviewContainer : CompositeDrawable
    {
        private Container placeholder;
        private Container buttonsContainer;
        private Button leftButton;
        private Button rightButton;
        private IWannaProgressBar progress;
        private SpriteText roomCounter;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Padding = new MarginPadding(10);
            AddRangeInternal(new Drawable[]
            {
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 5),
                    Margin = new MarginPadding { Top = 10 },
                    Children = new Drawable[]
                    {
                        progress = new IWannaProgressBar(),
                        roomCounter = new SpriteText()
                    }
                },
                placeholder = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
                buttonsContainer = new Container
                {
                    Alpha = 0,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        leftButton = new Button(trySelectPrev, FontAwesome.Solid.ChevronLeft)
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Alpha = 0
                        },
                        rightButton = new Button(trySelectNext, FontAwesome.Solid.ChevronRight)
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Alpha = 0
                        },

                    }
                }
            });
        }

        private List<Room> rooms;
        private int selected;
        private bool showPlayerSpawn;
        private List<(Vector2, int)> deathSpots;

        public void Preview(Level level, bool showPlayerSpawn = true, List<(Vector2, int)> deathSpots = null)
        {
            rooms = level.Rooms;
            selected = 0;
            this.showPlayerSpawn = showPlayerSpawn;
            this.deathSpots = deathSpots;

            preview(rooms.ElementAt(selected), selected, showPlayerSpawn, deathSpots);
            buttonsContainer.Alpha = 1;
        }

        private void preview(Room room, int roomIndex, bool showPlayerSpawn, List<(Vector2, int)> deathSpots)
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

            content.Add(new DrawableRoom(room, showPlayerSpawn, true, false, false, false, true));

            if (deathSpots != null)
            {
                foreach (var spot in deathSpots)
                {
                    if (roomIndex == spot.Item2)
                    {
                        content.Add(new DeathSpot
                        {
                            Position = spot.Item1
                        });
                    }
                }
            }

            leftButton.Alpha = roomIndex == 0 ? 0 : 1;
            rightButton.Alpha = roomIndex == rooms.Count - 1 ? 0 : 1;

            var count = rooms.Count;

            progress.ProgressTo((roomIndex + 1) / (float)count);
            roomCounter.Text = $"Room {roomIndex + 1} of {count}";
        }

        private void trySelectNext()
        {
            if (rooms == null || !rooms.Any())
                return;

            if (selected + 1 == rooms.Count)
                return;

            selected++;
            preview(rooms.ElementAt(selected), selected, showPlayerSpawn, deathSpots);
        }

        private void trySelectPrev()
        {
            if (rooms == null || !rooms.Any())
                return;

            if (selected == 0)
                return;

            selected--;
            preview(rooms.ElementAt(selected), selected, showPlayerSpawn, deathSpots);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        trySelectPrev();
                        return true;

                    case Key.Right:
                        trySelectNext();
                        return true;
                }
            }

            return base.OnKeyDown(e);
        }

        private class Button : ClickableContainer
        {
            private readonly Container content;

            public Button(Action action, IconUsage icon)
            {
                Size = new Vector2(30, 100);
                Action = action;
                Add(content = new Container
                {
                    Alpha = 0.6f,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new IWannaButtonBackground(),
                        new SpriteIcon
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Icon = icon,
                            Size = new Vector2(20)
                        }
                    }
                });
            }

            protected override bool OnHover(HoverEvent e)
            {
                content.FadeIn(200, Easing.Out);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                content.FadeTo(0.6f, 200, Easing.Out);
                base.OnHoverLost(e);
            }
        }
    }
}
