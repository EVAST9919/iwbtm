using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.UserInterface
{
    public class IWannaButtonBackground : CompositeDrawable
    {
        private const int duration = 200;

        private readonly Box background;

        public IWannaButtonBackground()
        {
            RelativeSizeAxes = Axes.Both;
            Masking = true;
            CornerRadius = 5;
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 1.5f,
                Colour = Color4.Black.Opacity(0.5f),
            };
            AddInternal(background = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = IWannaColour.GrayLighter
            });
        }

        protected override bool OnHover(HoverEvent e)
        {
            TweenEdgeEffectTo(new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 5f,
                Offset = new Vector2(0, 1),
                Colour = Color4.Black.Opacity(0.4f),
            }, duration, Easing.OutQuint);

            background.FadeColour(IWannaColour.GrayLightest, duration, Easing.OutQuint);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            TweenEdgeEffectTo(new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 1.5f,
                Colour = Color4.Black.Opacity(0.5f),
            }, duration, Easing.OutQuint);

            background.FadeColour(IWannaColour.GrayLighter, duration, Easing.OutQuint);

            base.OnHoverLost(e);
        }
    }
}
