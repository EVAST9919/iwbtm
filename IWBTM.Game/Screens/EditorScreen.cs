using IWBTM.Game.Playfield;
using IWBTM.Game.Screens.Edit;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Screens;

namespace IWBTM.Game.Screens
{
    public class EditorScreen : GameScreen
    {
        private readonly Bindable<TileType> selectedObject = new Bindable<TileType>();

        private readonly SpriteText selectedText;
        private readonly BluePrint blueprint;

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
                    Child = blueprint = new BluePrint()
                },
                selector = new ObjectSelector
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight
                },
                new TestButon()
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Margin = new MarginPadding { Right = 50, Bottom = 50 },
                    Action = test
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

        private void test()
        {
            this.Push(new GameplayScreen(blueprint.CreateRoom()));
        }

        private class TestButon : ClickableContainer
        {
            public TestButon()
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
                        Text = "Test",
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Color4.Black
                    }
                });
            }
        }
    }
}
