using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using IWBTM.Game.Rooms.Drawables;
using osuTK;
using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace IWBTM.Game.Screens.Edit
{
    public class ToolBar : CompositeDrawable
    {
        public Bindable<TileType> Selected => control.Current;

        public Action OnTest;
        public Action<string> OnSave;

        private readonly ObjectSelectorTabControl control;
        private readonly EditorTextbox textbox;

        public ToolBar()
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
                                Child = control = new ObjectSelectorTabControl()
                            }
                        },
                        new Drawable[]
                        {
                            Empty()
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
                                    new EditorButon("Test")
                                    {
                                        Action = () => OnTest?.Invoke()
                                    },
                                    new EditorButon("Save")
                                    {
                                        Action = () => OnSave?.Invoke(textbox.Text)
                                    }
                                }
                            }
                        },
                    }
                },
            });
        }

        public void SetRoomName(string name) => textbox.Text = name;

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
