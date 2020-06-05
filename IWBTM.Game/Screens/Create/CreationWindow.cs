using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using IWBTM.Game.UserInterface;
using osu.Framework.Graphics.Effects;
using osuTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;

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
            Masking = true;
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 9f,
                Colour = Color4.Black.Opacity(0.4f),
            };
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = IWannaColour.GrayDark
            });
        }
    }
}
