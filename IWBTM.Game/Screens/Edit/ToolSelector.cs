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
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using System;
using System.Linq;

namespace IWBTM.Game.Screens.Edit
{
    public class ToolSelector : CompositeDrawable
    {
        public Bindable<ToolEnum> Current => control.Current;
        public readonly Bindable<DrawableTile> SelectedTile = new Bindable<DrawableTile>();

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        [Resolved]
        private ConfirmationOverlay confirmation { get; set; }

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

        protected override bool OnHover(HoverEvent e) => true;

        private void onSelectedTileChanged(ValueChangedEvent<DrawableTile> tile)
        {
            if (tile.NewValue == null)
            {
                selectedTilePlaceholder.Clear();
                return;
            }

            FillFlowContainer flow;

            selectedTilePlaceholder.Child = flow = new FillFlowContainer
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
                    }
                }
            };

            if (!DrawableTile.IsGroup(tile.NewValue, TileGroup.Spike) && tile.NewValue.Tile.Type != TileType.Cherry)
                return;

            var hasAction = tile.NewValue.Tile.Action != null;

            flow.Add(new EditorButton($"{(hasAction ? "Edit" : "Add")} action", () => edit(tile.NewValue)));

            if (hasAction)
                flow.Add(new EditorButton("Delete action", () => tryDelete(tile.NewValue)));
        }

        private void tryDelete(DrawableTile tile)
        {
            confirmation.Push("Are you sure you want to delete action for this tile?", () =>
            {
                Edited?.Invoke(tile, null);
            });
        }

        private void edit(DrawableTile tile)
        {
            if (selectedTilePlaceholder.Children.OfType<ActionEditor>().Any())
                return;

            var actionEditor = new ActionEditor(tile.Tile.Action);
            selectedTilePlaceholder.Add(actionEditor);
            actionEditor.OnConfirm += actionParams => trySave(tile, actionParams.type, actionParams.time, actionParams.x, actionParams.y);
        }

        public Action<DrawableTile, TileAction> Edited;

        private void trySave(DrawableTile tile, TileActionType type, string timeString, string xString, string yString)
        {
            float time, x, y;

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
                Type = type,
            };

            Edited?.Invoke(tile, action);
        }

        private class ActionEditor : Container
        {
            public Action<(TileActionType type, string time, string x, string y)> OnConfirm;

            private readonly IWannaTextBox timeTextbox;
            private readonly IWannaTextBox xTextbox;
            private readonly IWannaTextBox yTextbox;
            private readonly ActionTypeSelector typeSelector;

            public ActionEditor(TileAction existing = null)
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;
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
                                typeSelector = new ActionTypeSelector(),
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
                                new EditorButton("Ok", () => OnConfirm?.Invoke((typeSelector.Current.Value, timeTextbox.Current.Value, xTextbox.Current.Value, yTextbox.Current.Value)))
                            }
                        }
                    }
                };

                if (existing != null)
                {
                    typeSelector.Current.Value = existing.Type;
                    xTextbox.Current.Value = existing.EndX.ToString();
                    yTextbox.Current.Value = existing.EndY.ToString();
                    timeTextbox.Current.Value = existing.Time.ToString();
                }
            }

            private class ActionTypeSelector : BasicDropdown<TileActionType>
            {
                public ActionTypeSelector()
                {
                    RelativeSizeAxes = Axes.X;

                    AddDropdownItem(TileActionType.Movement);
                    AddDropdownItem(TileActionType.Rotation);

                    Current.TriggerChange();
                }
            }
        }
    }
}
