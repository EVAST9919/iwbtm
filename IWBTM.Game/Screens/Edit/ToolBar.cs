﻿using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using System;

namespace IWBTM.Game.Screens.Edit
{
    public class ToolBar : CompositeDrawable
    {
        public Bindable<TileType> Selected => selector.Current;
        public Bindable<int> SnapValue => snapControl.Current;
        public Bindable<string> SelectedMusic => musicSelector.Current;

        public Action<string> OnTest;
        public Action<string, string> OnSave;

        private readonly ObjectSelectorTabControl selector;
        private readonly GridSnapTabControl snapControl;
        private readonly EditorTextbox textbox;
        private readonly MusicSelector musicSelector;

        public ToolBar(Room room, string name)
        {
            Width = 200;
            RelativeSizeAxes = Axes.Y;

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Gray
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ColumnDimensions = new[]
                    {
                        new Dimension()
                    },
                    RowDimensions = new[]
                    {
                        new Dimension(),
                        new Dimension(),
                        new Dimension(),
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Padding = new MarginPadding(5),
                                Child = new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(0, 5),
                                    Children = new Drawable[]
                                    {
                                        new SpriteText
                                        {
                                            Text = "Object selector",
                                            Colour = Color4.Black
                                        },
                                        selector = new ObjectSelectorTabControl()
                                    }
                                }
                            }
                        },
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Padding = new MarginPadding(5),
                                Child = new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(0, 5),
                                    Children = new Drawable[]
                                    {
                                        new SpriteText
                                        {
                                            Text = "Snap distance",
                                            Colour = Color4.Black
                                        },
                                        snapControl = new GridSnapTabControl()
                                    }
                                }
                            }
                        },
                        new Drawable[]
                        {
                            new FillFlowContainer
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                AutoSizeAxes = Axes.Both,
                                Direction = FillDirection.Vertical,
                                Spacing = new Vector2(0, 10),
                                Children = new Drawable[]
                                {
                                    textbox = new EditorTextbox(),
                                    new Container
                                    {
                                        Depth = -float.MaxValue,
                                        RelativeSizeAxes = Axes.X,
                                        Height = 30,
                                        Child = musicSelector = new MusicSelector(room)
                                    },
                                    new EditorButon("Test")
                                    {
                                        Action = () => OnTest?.Invoke(musicSelector.Current.Value)
                                    },
                                    new EditorButon("Save")
                                    {
                                        Action = () => OnSave?.Invoke(textbox.Current.Value, musicSelector.Current.Value)
                                    }
                                }
                            }
                        },
                    }
                },
            });

            if (!string.IsNullOrEmpty(name))
            {
                textbox.Text = name;
            }
        }

        protected override bool OnHover(HoverEvent e) => true;

        private class EditorTextbox : BasicTextBox
        {
            public EditorTextbox()
            {
                Height = 30;
                Width = 100;
            }
        }

        private class EditorButon : ClickableContainer
        {
            public EditorButon(string text)
            {
                Size = new Vector2(100, 50);

                AddRangeInternal(new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    new SpriteText
                    {
                        Text = text,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Color4.Black
                    }
                });
            }
        }
    }
}
