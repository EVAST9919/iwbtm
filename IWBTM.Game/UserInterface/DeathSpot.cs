using IWBTM.Game.Screens.Play.Player;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace IWBTM.Game.UserInterface
{
    public class DeathSpot : CompositeDrawable
    {
        public DeathSpot()
        {
            Origin = Anchor.Centre;
            Size = DefaultPlayer.SIZE;
            Masking = true;
            BorderColour = Color4.Red;
            BorderThickness = 2;
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Red,
                Alpha = 0.5f
            });
        }
    }
}
