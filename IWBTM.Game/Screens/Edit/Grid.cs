using IWBTM.Game.Rooms.Drawables;
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

        public Grid(Vector2 roomSize)
        {
            Size = new Vector2(roomSize.X, roomSize.Y) * DrawableTile.SIZE;
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

            for (int i = 0; i <= Size.X; i += snapValue)
            {
                AddInternal(new VerticalLine
                {
                    X = i
                });
            }

            for (int i = 0; i <= Size.Y; i += snapValue)
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
