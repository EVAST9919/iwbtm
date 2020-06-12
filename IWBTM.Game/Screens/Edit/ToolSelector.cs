using IWBTM.Game.Overlays;
using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
using System;

namespace IWBTM.Game.Screens.Edit
{
    public class ToolSelector : CompositeDrawable
    {
        public Bindable<ToolEnum> Current => control.Current;
        public readonly Bindable<DrawableTile> SelectedTile = new Bindable<DrawableTile>();

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        private readonly ToolSelectorTabControl control;
        private readonly FillFlowContainer selectedTilePlaceholder;

        public ToolSelector()
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
                selectedTilePlaceholder = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Spacing = new Vector2(0, 10),
                    Direction = FillDirection.Vertical,
                    Padding = new MarginPadding(10),
                },
                control = new ToolSelectorTabControl
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Margin = new MarginPadding { Bottom = 20 }
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            SelectedTile.BindValueChanged(onSelectedTileChanged, true);
        }

        private void onSelectedTileChanged(ValueChangedEvent<DrawableTile> tile)
        {
            if (tile.NewValue == null)
            {
                selectedTilePlaceholder.Clear();
                return;
            }

            selectedTilePlaceholder.Child = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Spacing = new Vector2(0, 10),
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new SpriteText
                    {
                        Text = tile.NewValue.Tile.Type.ToString()
                    },
                    new SpriteText
                    {
                        Text = $"X: {tile.NewValue.X}"
                    },
                    new SpriteText
                    {
                        Text = $"Y: {tile.NewValue.Y}"
                    },
                    new SpriteText
                    {
                        Text = $"Action: {tile.NewValue.Tile.Action != null}"
                    },
                    new EditorButton("Add action", () => onAddAction(tile.NewValue)),
                }
            };
        }

        private void onAddAction(DrawableTile tile)
        {
            IWannaTextBox timeTextbox;
            IWannaTextBox xTextbox;
            IWannaTextBox yTextbox;

            var container = new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = IWannaColour.GrayDarker,
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Padding = new MarginPadding(10),
                        Child = new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Spacing = new Vector2(0, 10),
                            Direction = FillDirection.Vertical,
                            Children = new Drawable[]
                            {
                                xTextbox = new IWannaTextBox
                                {
                                    RelativeSizeAxes = Axes.X,
                                    PlaceholderText = "X"
                                },
                                yTextbox = new IWannaTextBox
                                {
                                    RelativeSizeAxes = Axes.X,
                                    PlaceholderText = "Y"
                                },
                                timeTextbox = new IWannaTextBox
                                {
                                    RelativeSizeAxes = Axes.X,
                                    PlaceholderText = "Time (ms)"
                                },
                                new EditorButton("Ok", () => trySave(tile, timeTextbox.Current.Value, xTextbox.Current.Value, yTextbox.Current.Value))
                            }
                        }
                    }
                }
            };

            selectedTilePlaceholder.Add(container);
        }

        public Action<DrawableTile, TileAction> Edited;

        private void trySave(DrawableTile tile, string timeString, string xString, string yString)
        {
            float time = 0;
            float x = 0;
            float y = 0;

            try
            {
                time = float.Parse(timeString);
                x = float.Parse(xString);
                y = float.Parse(yString);
            }
            catch
            {
                notifications.Push("Make sure input is correct", NotificationState.Bad);
                return;
            }

            var action = new TileAction
            {
                EndX = x,
                EndY = y,
                Time = time,
            };

            Edited?.Invoke(tile, action);
        }
    }
}
