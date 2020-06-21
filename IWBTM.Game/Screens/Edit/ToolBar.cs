using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.UserInterface;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
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

        public Action OnTest;
        public Action OnSave;
        public Action OnRoomSelect;
        public Action OnClear;
        public Action OnSettings;
        public Action OnCherries;

        private readonly ObjectSelectorTabControl selector;
        private readonly GridSnapTabControl snapControl;

        public ToolBar()
        {
            RelativeSizeAxes = Axes.Both;
            Masking = true;
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 9f,
                Colour = Color4.Black.Opacity(0.4f),
            };

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = IWannaColour.GrayDark
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
                            new Dimension(GridSizeMode.AutoSize),
                            new Dimension(GridSizeMode.AutoSize),
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
                                    Margin = new MarginPadding {Top = 10},
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
                                    Anchor = Anchor.BottomCentre,
                                    Origin = Anchor.BottomCentre,
                                    AutoSizeAxes = Axes.Both,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(0, 10),
                                    Margin = new MarginPadding { Bottom = 20 },
                                    Children = new Drawable[]
                                    {
                                        new EditorButton("Select", () => OnRoomSelect?.Invoke()),
                                        new EditorButton("Settings", () => OnSettings?.Invoke()),
                                        new EditorButton("Add cherries", () => OnCherries?.Invoke()),
                                        new EditorButton("Clear", () => OnClear?.Invoke()),
                                        new EditorButton("Test", () => OnTest?.Invoke()),
                                        new EditorButton("Save", () => OnSave?.Invoke()),
                                    }
                                }
                            },
                        }
                    },
                }
            });
        }

        protected override bool OnHover(HoverEvent e) => true;
    }
}
