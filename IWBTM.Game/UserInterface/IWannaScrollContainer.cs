using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.UserInterface
{
    public class IWannaScrollContainer : BasicScrollContainer
    {
        protected override ScrollbarContainer CreateScrollbar(Direction direction) => new IWannaScrollbar(direction);

        private class IWannaScrollbar : ScrollbarContainer
        {
            private Color4 hoverColour;
            private Color4 defaultColour;

            private readonly Box box;

            public IWannaScrollbar(Direction direction)
                : base(direction)
            {
                CornerRadius = 5;

                const float margin = 3;

                Margin = new MarginPadding
                {
                    Left = direction == Direction.Vertical ? margin : 0,
                    Right = direction == Direction.Vertical ? margin : 0,
                    Top = direction == Direction.Horizontal ? margin : 0,
                    Bottom = direction == Direction.Horizontal ? margin : 0,
                };

                defaultColour = IWannaColour.Blue;
                hoverColour = IWannaColour.Blue.Lighten(0.1f);

                Masking = true;
                Child = box = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = defaultColour
                };
            }

            public override void ResizeTo(float val, int duration = 0, Easing easing = Easing.None)
            {
                Vector2 size = new Vector2(10)
                {
                    [(int)ScrollDirection] = val
                };
                this.ResizeTo(size, duration, easing);
            }

            protected override bool OnHover(HoverEvent e)
            {
                box.FadeColour(hoverColour, 100);
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                box.FadeColour(defaultColour, 100);
            }
        }
    }
}
