using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;

namespace IWBTM.Game.UserInterface
{
    public partial class InteractiveContainer : Container
    {
        protected override Container<Drawable> Content => content;

        private readonly Container content;
        protected readonly Container ScalableContent;

        public InteractiveContainer()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChild = ScalableContent = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Child = content = new Container
                {
                    RelativeSizeAxes = Axes.Both
                }
            };
        }

        private float zoom = 1f;

        public void Reset()
        {
            ScalableContent.ClearTransforms();
            ScalableContent.Anchor = Anchor.Centre;
            ScalableContent.Origin = Anchor.Centre;
            ScalableContent.Position = Vector2.Zero;
            ScalableContent.Size = Vector2.One;
            ScalableContent.Scale = Vector2.One;
            zoom = 1f;
        }

        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
        {
            base.OnDrag(e);
            ScalableContent.Position += e.Delta;
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            base.OnScroll(e);

            ScalableContent.OriginPosition = ToSpaceOfOtherDrawable(e.MousePosition, ScalableContent);
            ScalableContent.Anchor = Anchor.TopLeft;
            ScalableContent.Position = e.MousePosition;

            zoom += (e.ScrollDelta.Y > 0 ? 1 : -1) * zoom * 0.1f;
            ScalableContent.ScaleTo(zoom, 200, Easing.OutQuint);

            return true;
        }
    }
}
