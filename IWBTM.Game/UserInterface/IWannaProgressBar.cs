using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace IWBTM.Game.UserInterface
{
    public class IWannaProgressBar : CompositeDrawable
    {
        private readonly Box bar;

        public IWannaProgressBar()
        {
            RelativeSizeAxes = Axes.X;
            Height = 10;
            AddInternal(new CircularContainer
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = IWannaColour.GrayDarkest
                    },
                    bar = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Width = 0,
                        Colour = IWannaColour.Blue
                    }
                }
            });
        }

        public void ProgressTo(double value)
        {
            bar.ResizeWidthTo((float)value, 300, Easing.Out);
        }
    }
}
