using IWBTM.Game.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;

namespace IWBTM.Game.Screens
{
    public class GameScreen : Screen
    {
        public GameScreen()
        {
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
            };

            return base.OnKeyDown(e);
        }

        protected virtual void OnExit()
        {
            this.Exit();
        }
    }
}
