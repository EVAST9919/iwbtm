using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using IWBTM.Game.Screens.Play.Playfield;

namespace IWBTM.Game.Screens.Edit
{
    public class ObjectSelector : CompositeDrawable
    {
        public Bindable<TileType> Selected => control.Current;

        private readonly ObjectSelectorTabControl control;

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
                Child = control = new ObjectSelectorTabControl()
            });
        }

        protected override bool OnHover(HoverEvent e) => true;
    }
}
