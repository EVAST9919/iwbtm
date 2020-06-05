using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using IWBTM.Game.UserInterface;
using osu.Framework.Graphics.Sprites;
using IWBTM.Game.Overlays;
using osu.Framework.Allocation;
using IWBTM.Game.Helpers;
using System;

namespace IWBTM.Game.Screens.Create
{
    public class NameSettingWindow : CreationWindow
    {
        public Action<string> OnCommit;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        private readonly IWannaTextBox textbox;

        public NameSettingWindow()
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
                            Text = "Set the room name",
                        },
                        textbox = new IWannaTextBox
                        {
                            Width = 300,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        },
                        new IWannaBasicButton("Ok", onPressed)
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        }
                    }
                }
            });

            textbox.OnCommit += (u, v) => onPressed();
        }

        private void onPressed()
        {
            var text = textbox.Current.Value;

            if (string.IsNullOrEmpty(text))
            {
                notifications.Push("Name can't be empty", NotificationState.Bad);
                return;
            }

            if (RoomStorage.RoomExists(text))
            {
                notifications.Push($"\"{text}\" room already exists", NotificationState.Bad);
                return;
            }

            OnCommit?.Invoke(text);
        }
    }
}
