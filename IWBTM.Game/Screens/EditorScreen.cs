using IWBTM.Game.Playfield;
using IWBTM.Game.Screens.Edit;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace IWBTM.Game.Screens
{
    public class EditorScreen : GameScreen
    {
        private readonly Bindable<TileType> selectedObject = new Bindable<TileType>();

        private readonly SpriteText selectedText;

        public EditorScreen()
        {
            ObjectSelector selector;

            AddRangeInternal(new Drawable[]
            {
                selectedText = new SpriteText
                {
                    Margin = new MarginPadding(10),
                },
                new PlayfieldAdjustmentContainer
                {
                    Scale = new Vector2(0.9f),
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                        new Grid()
                    }
                },
                selector = new ObjectSelector
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight
                }
            });

            selectedObject.BindTo(selector.Selected);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            selectedObject.BindValueChanged(newSelected => selectedText.Text = $"Selected: {newSelected.NewValue.ToString()}", true);
        }
    }
}
