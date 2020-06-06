using IWBTM.Game.UserInterface;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using System;

namespace IWBTM.Game.Screens.Create
{
    public class SizeSetting : CompositeDrawable
    {
        public Action AdjustRequested;

        public readonly Bindable<Vector2> Current = new Bindable<Vector2>(new Vector2(25, 19));

        private readonly SpriteText sizeText;

        public SizeSetting()
        {
            AutoSizeAxes = Axes.Both;
            AddInternal(new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(10, 0),
                Children = new Drawable[]
                {
                    sizeText = new SpriteText
                    {
                        Font = FontUsage.Default.With(size: 20),
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft
                    },
                    new IWannaBasicButton("Adjust", () => AdjustRequested?.Invoke())
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft
                    }
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Current.BindValueChanged(onCurrentChanged, true);
        }

        private void onCurrentChanged(ValueChangedEvent<Vector2> value)
        {
            sizeText.Text = $"{value.NewValue.X} x {value.NewValue.Y}";
        }
    }
}
