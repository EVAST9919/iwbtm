using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

namespace IWBTM.Game.UserInterface
{
    public class IWannaTextBox : BasicTextBox
    {
        public IWannaTextBox()
        {
            Height = 40;
            CornerRadius = 5;
            LengthLimit = 1000;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            BackgroundUnfocused = BackgroundFocused = IWannaColour.GrayDarkest;
            BackgroundCommit = IWannaColour.GrayDarker;
            BorderColour = IWannaColour.Blue;
        }

        protected override void OnFocus(FocusEvent e)
        {
            BorderThickness = 2;
            base.OnFocus(e);
        }

        protected override void OnFocusLost(FocusLostEvent e)
        {
            if (!IsHovered)
                BorderThickness = 0;

            base.OnFocusLost(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            BorderThickness = 2;
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!HasFocus)
                BorderThickness = 0;

            base.OnHoverLost(e);
        }
    }
}
