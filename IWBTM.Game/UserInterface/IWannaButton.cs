using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using System;

namespace IWBTM.Game.UserInterface
{
    public abstract class IWannaButton : ClickableContainer
    {
        protected readonly Box Background;

        public IWannaButton(string text, Action action)
        {
            AddRange(new Drawable[]
            {
                new IWannaButtonBackground(),
                new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = text,
                }
            });

            Action = () => action?.Invoke();
        }

        protected override bool OnHover(HoverEvent e)
        {
            base.OnHover(e);
            return true;
        }
    }
}
