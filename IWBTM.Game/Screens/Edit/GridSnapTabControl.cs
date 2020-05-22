using IWBTM.Game.Rooms.Drawables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.Screens.Edit
{
    public class GridSnapTabControl : TabControl<int>
    {
        public GridSnapTabControl()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            AddItem(32);
            AddItem(16);
            AddItem(8);
            AddItem(4);
            AddItem(2);
            AddItem(1);
        }

        protected override Dropdown<int> CreateDropdown() => null;

        protected override TabItem<int> CreateTabItem(int value) => new GridSnapTabItem(value);

        protected override TabFillFlowContainer CreateTabFlow() => new TabFillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            AllowMultiline = true,
            Spacing = new Vector2(5),
        };

        private class GridSnapTabItem : TabItem<int>
        {
            private readonly Box background;

            public GridSnapTabItem(int value)
                : base(value)
            {
                Size = new Vector2(DrawableTile.SIZE);

                Children = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.White,
                    },
                    new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = value.ToString(),
                        Colour = Color4.Black
                    }
                };
            }

            protected override void OnActivated()
            {
                background.Colour = Color4.Red;
            }

            protected override void OnDeactivated()
            {
                background.Colour = Color4.White;
            }
        }
    }
}
