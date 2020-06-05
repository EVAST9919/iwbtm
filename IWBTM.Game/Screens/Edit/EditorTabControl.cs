using IWBTM.Game.Rooms.Drawables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.Screens.Edit
{
    public abstract class EditorTabControl<T> : TabControl<T>
    {
        public EditorTabControl()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
        }

        protected override Dropdown<T> CreateDropdown() => null;

        protected override TabFillFlowContainer CreateTabFlow() => new TabFillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            AllowMultiline = true,
            Spacing = new Vector2(5),
        };

        protected override TabItem<T> CreateTabItem(T value) => CreateItem(value);

        protected abstract EditorTabItem<T> CreateItem(T value);

        protected class EditorTabItem<U> : TabItem<U>
        {
            public EditorTabItem(U value)
                : base(value)
            {
                Size = new Vector2(DrawableTile.SIZE);
                Masking = true;
                CornerRadius = 5;
                BorderColour = Color4.Red;

                Add(new Box
                {
                    RelativeSizeAxes = Axes.Both,
                });
            }

            protected override void OnActivated()
            {
                BorderThickness = 5;
            }

            protected override void OnDeactivated()
            {
                BorderThickness = 0;
            }
        }
    }
}
