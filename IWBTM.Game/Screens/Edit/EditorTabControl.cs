using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osuTK;

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
            private readonly Background background;

            public EditorTabItem(U value)
                : base(value)
            {
                Size = new Vector2(DrawableTile.SIZE);
                Add(background = new Background());
            }

            protected override void OnActivated() => background.Activate();

            protected override void OnDeactivated() => background.Deactivate();

            private class Background : IWannaButtonBackground
            {
                public Background()
                {
                    BorderColour = IWannaColour.Blue;
                }

                public void Activate() => BorderThickness = 5;

                public void Deactivate() => BorderThickness = 0;
            }
        }
    }
}
