using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
using System;

namespace IWBTM.Game.UserInterface
{
    public class WhiteButton : ClickableContainer
    {
        public WhiteButton(string text, Action action)
        {
            Size = new Vector2(120, 40);
            Masking = true;
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Offset = new Vector2(0f, 1f),
                Radius = 3f,
                Colour = Color4.Black.Opacity(0.25f),
            };
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = text,
                    Colour = Color4.Black
                }
            };
            Action = () => action?.Invoke();
        }
    }
}
