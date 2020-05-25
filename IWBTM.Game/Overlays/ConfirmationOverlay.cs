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

namespace IWBTM.Game.Overlays
{
    public class ConfirmationOverlay : CompositeDrawable
    {
        private ConfirmationWindow currentWindow;

        private readonly Box dim;

        public ConfirmationOverlay()
        {
            RelativeSizeAxes = Axes.Both;
            AddInternal(dim = new Box
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
                dim.FadeOut();
            },
            () =>
            {
                currentWindow = null;
                dim.FadeOut();
            });

            dim.FadeTo(0.5f);
            AddInternal(window);

            currentWindow = window;
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
                    }
                };
            }

            return base.OnKeyDown(e);
        }

        private class ConfirmationWindow : CompositeDrawable
        {
            private Action confirm;

            public ConfirmationWindow(string text, Action onConfirm, Action onDecline)
            {
                confirm = () =>
                {
                    onConfirm?.Invoke();
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
                    Radius = 5f,
                    Colour = Color4.Black.Opacity(0.25f),
                };
                AddRangeInternal(new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both
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
                                Colour = Color4.Black
                            },
                            new FillFlowContainer<Button>
                            {
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre,
                                Direction = FillDirection.Horizontal,
                                AutoSizeAxes = Axes.Both,
                                Spacing = new Vector2(50, 0),
                                Children = new[]
                                {
                                    new Button("yes", confirm),
                                    new Button("no", () =>
                                    {
                                        onDecline?.Invoke();
                                        Expire();
                                    })
                                }
                            }
                        }
                    }
                });
            }

            public void ForceConfirm()
            {
                confirm?.Invoke();
            }

            private class Button : ClickableContainer
            {
                public Button(string text, Action action)
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
    }
}
