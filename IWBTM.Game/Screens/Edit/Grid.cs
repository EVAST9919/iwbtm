using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace IWBTM.Game.Screens.Edit
{
    public class Grid : CompositeDrawable
    {
        public Grid()
        {
            Size = DefaultPlayfield.BASE_SIZE;

            for (int i = 0; i <= DefaultPlayfield.TILES_WIDTH; i++)
            {
                AddInternal(new VerticalLine
                {
                    X = i * DrawableTile.SIZE
                });
            }

            for (int i = 0; i <= DefaultPlayfield.TILES_HEIGHT; i++)
            {
                AddInternal(new HorizontalLine
                {
                    Y = i * DrawableTile.SIZE
                });
            }
        }

        private class VerticalLine : Box
        {
            public VerticalLine()
            {
                RelativeSizeAxes = Axes.Y;
                Width = 2;
                Origin = Anchor.TopCentre;
                Colour = Color4.Gray;
            }
        }

        private class HorizontalLine : Box
        {
            public HorizontalLine()
            {
                RelativeSizeAxes = Axes.X;
                Height = 2;
                Origin = Anchor.CentreLeft;
                Colour = Color4.Gray;
            }
        }
    }
}
