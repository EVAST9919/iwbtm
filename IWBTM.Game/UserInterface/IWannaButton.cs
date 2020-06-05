using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using System;

namespace IWBTM.Game.UserInterface
{
    public abstract class IWannaButton : ClickableContainer
    {
        private const int duration = 200;

        protected readonly Box Background;

        public IWannaButton(string text, Action action)
        {
            Masking = true;
            CornerRadius = 5;
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 1f,
                Colour = Color4.Black.Opacity(0.5f),
            };
            Children = new Drawable[]
            {
                Background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = IWannaColour.IWannaGrayDark
                },
                new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = text,
                }
            };
            Action = () => action?.Invoke();
        }

        protected override bool OnHover(HoverEvent e)
        {
            TweenEdgeEffectTo(new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 4f,
                Offset = new Vector2(0, 1),
                Colour = Color4.Black.Opacity(0.4f),
            }, duration, Easing.OutQuint);

            Background.FadeColour(IWannaColour.IWannaGrayLight, duration, Easing.OutQuint);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            TweenEdgeEffectTo(new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 1f,
                Colour = Color4.Black.Opacity(0.5f),
            }, duration, Easing.OutQuint);

            Background.FadeColour(IWannaColour.IWannaGrayDark, duration, Easing.OutQuint);

            base.OnHoverLost(e);
        }
    }
}
