using IWBTM.Game.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;

namespace IWBTM.Game.Screens
{
    public partial class IWannaScreen : Screen
    {
        public IWannaScreen()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = IWannaColour.GrayDarker
            });
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        OnExit();
                        return true;
                }
            }

            return base.OnKeyDown(e);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            this.FadeInFromZero(200, Easing.Out);
        }

        public override bool OnExiting(ScreenExitEvent e)
        {
            this.FadeOut(200, Easing.Out);
            this.ScaleTo(0.9f, 200, Easing.Out);
            return base.OnExiting(e);
        }

        public override void OnSuspending(ScreenTransitionEvent e)
        {
            base.OnSuspending(e);
            this.ScaleTo(1.1f, 200, Easing.Out);
            this.FadeOut(200, Easing.Out);
        }

        public override void OnResuming(ScreenTransitionEvent e)
        {
            base.OnResuming(e);
            this.ScaleTo(1, 200, Easing.Out);
            this.FadeIn(200, Easing.Out);
        }

        protected virtual void OnExit() => this.Exit();
    }
}
