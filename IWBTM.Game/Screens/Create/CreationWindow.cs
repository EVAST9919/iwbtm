using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace IWBTM.Game.Screens.Create
{
    public class CreationWindow : CompositeDrawable
    {
        public CreationWindow()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Gray
            });
        }
    }
}
