using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.UserInterface;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using System;

namespace IWBTM.Game.Screens.Edit
{
    public class ToolBar : CompositeDrawable
    {
        public Bindable<TileType> Selected => selector.Current;
        public Bindable<int> SnapValue => snapControl.Current;

        public Action OnTest;
        public Action OnSave;

        private readonly ObjectSelectorTabControl selector;
        private readonly GridSnapTabControl snapControl;

        public ToolBar()
        {
            RelativeSizeAxes = Axes.Both;

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = IWannaColour.IWannaGrayDarker
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(10),
                    Child = new GridContainer
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
                                new FillFlowContainer
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
                                        },
                                        selector = new ObjectSelectorTabControl()
                                    }
                                }
                            },
                            new Drawable[]
                            {
                                new FillFlowContainer
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
                                        },
                                        snapControl = new GridSnapTabControl()
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
                                    Spacing = new Vector2(0, 20),
                                    Children = new Drawable[]
                                    {
                                        new EditorButton("Test", () => OnTest?.Invoke()),
                                        new EditorButton("Save", () => OnSave?.Invoke())
                                    }
                                }
                            },
                        }
                    },
                }
            });
        }

        private class EditorButton : IWannaButton
        {
            public EditorButton(string text, Action action)
                : base(text, action)
            {
                Size = new Vector2(100, 50);
            }
        }
    }
}
