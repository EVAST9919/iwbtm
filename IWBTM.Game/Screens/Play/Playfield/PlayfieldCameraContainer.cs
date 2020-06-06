using IWBTM.Game.Rooms.Drawables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace IWBTM.Game.Screens.Play.Playfield
{
    public class PlayfieldCameraContainer : Container
    {
        protected override Container<Drawable> Content => content;
        private readonly Container content;

        public PlayfieldCameraContainer()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
            InternalChild = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                FillAspectRatio = 25f / 19,
                Child = content = new ScalingContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Name = "Scaling Container"
                }
            };
        }

        private class ScalingContainer : Container
        {
            protected override void Update()
            {
                base.Update();
                Scale = new Vector2(Parent.ChildSize.X / (25 * DrawableTile.SIZE));
                Size = Vector2.Divide(Vector2.One, Scale);
            }
        }
    }
}
