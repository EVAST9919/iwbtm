using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.Screens.Edit
{
    public class Grid : CompositeDrawable
    {
        public readonly Bindable<int> Current = new Bindable<int>();

        public Grid()
        {
            Size = DefaultPlayfield.BASE_SIZE;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Current.BindValueChanged(newValue => updateSnap(newValue.NewValue), true);
        }

        private void updateSnap(int snapValue)
        {
            ClearInternal();

            if (snapValue < 4)
                return;

            for (int i = 0; i <= DefaultPlayfield.BASE_SIZE.X; i += snapValue)
            {
                AddInternal(new VerticalLine
                {
                    X = i
                });
            }

            for (int i = 0; i <= DefaultPlayfield.BASE_SIZE.Y; i += snapValue)
            {
                AddInternal(new HorizontalLine
                {
                    Y = i
                });
            }
        }

        private class VerticalLine : Box
        {
            public VerticalLine()
            {
                RelativeSizeAxes = Axes.Y;
                Width = 0.5f;
                Origin = Anchor.TopCentre;
                Colour = Color4.Gray;
                EdgeSmoothness = Vector2.One;
            }
        }

        private class HorizontalLine : Box
        {
            public HorizontalLine()
            {
                RelativeSizeAxes = Axes.X;
                Height = 0.5f;
                Origin = Anchor.CentreLeft;
                Colour = Color4.Gray;
                EdgeSmoothness = Vector2.One;
            }
        }
    }
}
