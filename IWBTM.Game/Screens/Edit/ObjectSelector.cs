using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace IWBTM.Game.Screens.Edit
{
    public class ObjectSelector : CompositeDrawable
    {
        public ObjectSelector()
        {
            Width = 200;
            RelativeSizeAxes = Axes.Y;

            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Gray
            });

            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(5),
                Child = new ObjectSelectorTabControl()
            });
        }
    }
}
