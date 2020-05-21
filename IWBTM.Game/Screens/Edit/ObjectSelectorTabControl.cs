using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using System;
using IWBTM.Game.Rooms.Drawables;

namespace IWBTM.Game.Screens.Edit
{
    public class ObjectSelectorTabControl : TabControl<TileType>
    {
        public ObjectSelectorTabControl()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            foreach (var val in Enum.GetValues(typeof(TileType)))
                AddItem((TileType)val);
        }

        protected override Dropdown<TileType> CreateDropdown() => null;

        protected override TabItem<TileType> CreateTabItem(TileType value) => new ObjectSelectorTabItem(value);

        protected override TabFillFlowContainer CreateTabFlow() => new TabFillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            AllowMultiline = true,
            Spacing = new Vector2(5),
        };

        private class ObjectSelectorTabItem : TabItem<TileType>
        {
            private readonly Box background;

            public ObjectSelectorTabItem(TileType value)
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
                    new DrawableTile(value)
                    {
                        Scale = new Vector2(0.8f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
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
