using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace IWBTM.Game.Playfield
{
    public class PlayfieldAdjustmentContainer : Container
    {
        protected override Container<Drawable> Content => content;
        private readonly Container content;

        public PlayfieldAdjustmentContainer()
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
                Child = content = new ScalingContainer
                {
                    RelativeSizeAxes = Axes.Both
                }
            };
        }

        private class ScalingContainer : Container
        {
            protected override void Update()
            {
                base.Update();
                Scale = new Vector2(Parent.ChildSize.X / (DefaultPlayfield.WIDTH * Tile.SIZE));
                Size = Vector2.Divide(Vector2.One, Scale);
            }
        }
    }
}
