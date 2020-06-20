using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace IWBTM.Game.UserInterface
{
    public class IWannaCheckbox : Checkbox
    {
        public string LabelText
        {
            set
            {
                if (labelText != null)
                    labelText.Text = value;
            }
        }

        private readonly SpriteText labelText;
        private readonly Nub nub;

        public IWannaCheckbox()
        {
            AutoSizeAxes = Axes.Both;
            Child = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(10, 0),
                Children = new Drawable[]
                {
                    nub = new Nub
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft
                    },
                    labelText = new SpriteText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft
                    },
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Current.BindValueChanged(onCurrentChanged, true);
            nub.ForceTransformsCompletion();
        }

        private void onCurrentChanged(ValueChangedEvent<bool> value)
        {
            if (value.NewValue)
                nub.Activate();
            else
                nub.Deactivate();
        }

        private class Nub : CompositeDrawable
        {
            private readonly Circle bg;
            private readonly Circle fg;

            public Nub()
            {
                Size = new Vector2(25);
                AddRangeInternal(new Drawable[]
                {
                    bg = new Circle
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    fg = new Circle
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Colour = IWannaColour.GrayDarkest
                    }
                });
            }

            public void Activate()
            {
                bg.FadeColour(IWannaColour.Blue, 200, Easing.Out);
                fg.ResizeTo(0.6f, 200, Easing.Out);
            }

            public void Deactivate()
            {
                bg.FadeColour(IWannaColour.GrayLighter, 200, Easing.Out);
                fg.ResizeTo(0.85f, 200, Easing.Out);
            }

            public void ForceTransformsCompletion()
            {
                bg.FinishTransforms();
                fg.FinishTransforms();
            }
        }
    }
}
