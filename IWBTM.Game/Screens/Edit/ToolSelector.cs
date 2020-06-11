using IWBTM.Game.UserInterface;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;
using System;

namespace IWBTM.Game.Screens.Edit
{
    public class ToolSelector : CompositeDrawable
    {
        public Bindable<ToolEnum> Current => control.Current;

        private readonly ToolSelectorTabControl control;

        public ToolSelector()
        {
            RelativeSizeAxes = Axes.Both;
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
                control = new ToolSelectorTabControl
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            });
        }

        private class ToolSelectorTabControl : TabControl<ToolEnum>
        {
            public ToolSelectorTabControl()
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;

                foreach (var val in Enum.GetValues(typeof(ToolEnum)))
                    AddItem((ToolEnum)val);
            }

            protected override Dropdown<ToolEnum> CreateDropdown() => null;

            protected override TabFillFlowContainer CreateTabFlow() => new TabFillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AllowMultiline = true,
                Spacing = new Vector2(5),
            };

            protected override TabItem<ToolEnum> CreateTabItem(ToolEnum value) => new ToolSelectorTabItem(value);

            private class ToolSelectorTabItem : TabItem<ToolEnum>
            {
                private readonly IWannaSelectableButtonBackground bg;

                public ToolSelectorTabItem(ToolEnum value)
                    : base(value)
                {
                    Anchor = Anchor.Centre;
                    Origin = Anchor.Centre;
                    Size = new Vector2(100, 50);
                    Children = new Drawable[]
                    {
                        bg = new IWannaSelectableButtonBackground(),
                        new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = value.ToString()
                        }
                    };
                }

                protected override void OnActivated() => bg.Activate();

                protected override void OnDeactivated() => bg.Deactivate();
            }
        }
    }

    public enum ToolEnum
    {
        Place,
        Select
    }
}
