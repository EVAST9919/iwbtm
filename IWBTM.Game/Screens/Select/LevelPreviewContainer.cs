﻿using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
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
                buttonsContainer = new Container
                {
                    Alpha = 0,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Button(trySelectPrev, FontAwesome.Solid.ChevronLeft)
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft
                        },
                        new Button(trySelectNext, FontAwesome.Solid.ChevronRight)
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight
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

            content.AddRange(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                new DrawableRoom(room, showPlayerSpawn, true)
            });

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
            public Button(Action action, IconUsage icon)
            {
                Size = new Vector2(30, 100);
                Action = action;
                Alpha = 0.6f;
                AddRange(new Drawable[]
                {
                    new IWannaButtonBackground(),
                    new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Icon = icon,
                        Size = new Vector2(20)
                    }
                });
            }

            protected override bool OnHover(HoverEvent e)
            {
                this.FadeIn(200, Easing.Out);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                this.FadeTo(0.6f, 200, Easing.Out);
                base.OnHoverLost(e);
            }
        }
    }
}
