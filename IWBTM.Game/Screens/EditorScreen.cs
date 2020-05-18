using IWBTM.Game.Playfield;
using IWBTM.Game.Screens.Edit;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
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
            BluePrint blueprint;

            AddRangeInternal(new Drawable[]
            {
                selectedText = new SpriteText
                {
                    Margin = new MarginPadding(10),
                },
                new PlayfieldAdjustmentContainer
                {
                    Scale = new Vector2(0.9f),
                    Child = blueprint = new BluePrint()
                },
                selector = new ObjectSelector
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight
                }
            });

            selectedObject.BindTo(selector.Selected);
            blueprint.Selected.BindTo(selector.Selected);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            selectedObject.BindValueChanged(newSelected => selectedText.Text = $"Selected: {newSelected.NewValue.ToString()}", true);
        }
    }
}
