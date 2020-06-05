using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using System;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using osuTK;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Input.Events;
using osuTK.Input;
using IWBTM.Game.UserInterface;

namespace IWBTM.Game.Overlays
{
    public class ConfirmationOverlay : OverlayContainer
    {
        private ConfirmationWindow currentWindow;

        private readonly Box dim;

        public ConfirmationOverlay()
        {
            RelativeSizeAxes = Axes.Both;
            Add(dim = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Alpha = 0,
                Colour = Color4.Black
            });
        }

        public void Push(string text, Action onConfirm)
        {
            if (currentWindow != null)
                return;

            var window = new ConfirmationWindow(text, () =>
            {
                onConfirm?.Invoke();
                currentWindow = null;
                Hide();
            },
            () =>
            {
                currentWindow = null;
                Hide();
            });

            Add(window);

            currentWindow = window;

            Show();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (currentWindow != null)
            {
                if (!e.Repeat)
                {
                    switch (e.Key)
                    {
                        case Key.Enter:
                            currentWindow.ForceConfirm();
                            return true;

                        case Key.Escape:
                            currentWindow.ForceDecline();
                            return true;
                    }
                };
            }

            return base.OnKeyDown(e);
        }

        protected override void PopIn()
        {
            dim.FadeTo(0.5f);
        }

        protected override void PopOut()
        {
            dim.FadeOut(200, Easing.Out);
        }

        private class ConfirmationWindow : CompositeDrawable
        {
            private readonly Action confirm;
            private readonly Action decline;

            public ConfirmationWindow(string text, Action onConfirm, Action onDecline)
            {
                confirm = () =>
                {
                    onConfirm?.Invoke();
                    Expire();
                };

                decline = () =>
                {
                    onDecline?.Invoke();
                    Expire();
                };

                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                RelativeSizeAxes = Axes.X;
                Height = 200;
                Masking = true;
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Shadow,
                    Radius = 9f,
                    Colour = Color4.Black.Opacity(0.4f),
                };
                AddRangeInternal(new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = IWannaColour.GrayDark
                    },
                    new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.X,
                        RelativeSizeAxes = Axes.Y,
                        Padding = new MarginPadding {Vertical = 30},
                        Children = new Drawable[]
                        {
                            new SpriteText
                            {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                Text = text,
                            },
                            new FillFlowContainer<IWannaBasicButton>
                            {
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre,
                                Direction = FillDirection.Horizontal,
                                AutoSizeAxes = Axes.Both,
                                Spacing = new Vector2(50, 0),
                                Children = new[]
                                {
                                    new IWannaBasicButton("yes", confirm),
                                    new IWannaBasicButton("no", decline)
                                }
                            }
                        }
                    }
                });
            }

            public void ForceConfirm() => confirm?.Invoke();

            public void ForceDecline() => decline?.Invoke();
        }
    }
}
