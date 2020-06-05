using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Sprites;
using System;
using IWBTM.Game.UserInterface;

namespace IWBTM.Game.Screens.Create
{
    public class MusicSettingWindow : CreationWindow
    {
        public Action<string> OnCommit;

        private readonly MusicSelector musicSelector;

        public MusicSettingWindow()
        {
            AddInternal(new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Both,
                Padding = new MarginPadding(10),
                Child = new FillFlowContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10),
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = "Set default room audio",
                        },
                        new Container
                        {
                            Width = 300,
                            Height = 40,
                            Anchor = Anchor.Centre,
                            Depth = -int.MaxValue,
                            Origin = Anchor.Centre,
                            Child = musicSelector = new MusicSelector()
                        },
                        new IWannaBasicButton("Ok", onPressed)
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        }
                    }
                }
            });
        }

        private void onPressed()
        {
            OnCommit?.Invoke(musicSelector.Current.Value);
        }

        private class MusicSelector : BasicDropdown<string>
        {
            public MusicSelector()
            {
                RelativeSizeAxes = Axes.X;

                AddDropdownItem("none");
                AddDropdownItem("room-1");
                AddDropdownItem("room-2");
                AddDropdownItem("room-3");
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();
                Current.Value = "none";
            }
        }
    }
}
