using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.UserInterface;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.Screens.Edit
{
    public class ToolSelector : CompositeDrawable
    {
        public Bindable<ToolEnum> Current => control.Current;
        public readonly Bindable<DrawableTile> SelectedTile = new Bindable<DrawableTile>();

        private readonly ToolSelectorTabControl control;
        private readonly Container selectedTilePlaceholder;

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
                selectedTilePlaceholder = new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
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
                    }
                }
            };
        }
    }
}
