using IWBTM.Game.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using System;

namespace IWBTM.Game.Screens.Edit
{
    public class ToolSelectorTabControl : TabControl<ToolEnum>
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

    public enum ToolEnum
    {
        Place,
        Select
    }
}
