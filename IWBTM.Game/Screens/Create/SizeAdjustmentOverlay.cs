using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Input;
using osuTK;
using IWBTM.Game.UserInterface;
using System;
using osu.Framework.Allocation;
using IWBTM.Game.Overlays;

namespace IWBTM.Game.Screens.Create
{
    public class SizeAdjustmentOverlay : IWannaOverlay
    {
        public Action<Vector2> NewSize;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        private readonly Container settings;
        private readonly IWannaTextBox widthTextBox;
        private readonly IWannaTextBox heightTextBox;

        public SizeAdjustmentOverlay()
        {
            Add(settings = new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = IWannaColour.GrayDarker
                    },
                    new FillFlowContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 10),
                        Margin = new MarginPadding(10),
                        Children = new Drawable[]
                        {
                            widthTextBox = new IWannaTextBox
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                PlaceholderText = "Width",
                                Width = 300,
                            },
                            heightTextBox = new IWannaTextBox
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                PlaceholderText = "Height",
                                Width = 300,
                            },
                            new IWannaBasicButton("Ok", onConfirm)
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                            }
                        }
                    }
                }
            });
        }

        protected override void PopIn()
        {
            base.PopIn();
            settings.ScaleTo(1, 200, Easing.Out);
            settings.FadeIn(200, Easing.Out);
        }

        protected override void PopOut()
        {
            base.PopOut();
            settings.ScaleTo(1.1f, 200, Easing.Out);
            settings.FadeOut(200, Easing.Out);
        }

        private void onConfirm()
        {
            try
            {
                NewSize.Invoke(new Vector2(float.Parse(widthTextBox.Current.Value), float.Parse(heightTextBox.Current.Value)));
                Hide();
            }
            catch
            {
                notifications.Push("Make sure input is correct", NotificationState.Bad);
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        onConfirm();
                        return true;
                }
            }

            return base.OnKeyDown(e);
        }
    }
}
