using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using System;
using osu.Framework.Bindables;
using IWBTM.Game.UserInterface;
using IWBTM.Game.Overlays;
using osu.Framework.Allocation;

namespace IWBTM.Game.Screens.Create
{
    public class MusicSettingWindow : CreationWindow
    {
        public Action<(bool, string)> OnCommit;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        private readonly BasicCheckbox customMusic;
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
                            Text = "Set the room music",
                            Colour = Color4.Black
                        },
                        customMusic = new BasicCheckbox
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            LabelText = "Custom music",
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
                        new WhiteButton("Ok", onPressed)
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        }
                    }
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            customMusic.Current.BindValueChanged(onCustomMusicChanged, true);
        }

        private void onCustomMusicChanged(ValueChangedEvent<bool> custom)
        {
            musicSelector.Alpha = custom.NewValue ? 0 : 1;
        }

        private void onPressed()
        {
            var custom = customMusic.Current.Value;

            if (custom)
            {
                notifications.Push("Custom music is not supported for now", NotificationState.Bad);
                return;
            }

            OnCommit?.Invoke((custom, musicSelector.Current.Value));
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
