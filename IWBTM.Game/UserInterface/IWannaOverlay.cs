using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Input;
using osuTK.Graphics;

namespace IWBTM.Game.UserInterface
{
    public class IWannaOverlay : OverlayContainer
    {
        protected override bool BlockNonPositionalInput => State.Value == Visibility.Visible;

        protected override bool StartHidden => true;

        protected virtual float Dim => 0.5f;

        private readonly Box bg;

        public IWannaOverlay()
        {
            RelativeSizeAxes = Axes.Both;
            AddRange(new Drawable[]
            {
                bg = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black
                }
            });
        }

        protected override void PopIn()
        {
            bg.FadeTo(Dim, 200, Easing.Out);
        }

        protected override void PopOut()
        {
            bg.FadeOut(200, Easing.Out);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        Hide();
                        return true;
                }
            }

            return base.OnKeyDown(e);
        }
    }
}
