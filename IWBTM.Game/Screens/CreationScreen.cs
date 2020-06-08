using IWBTM.Game.Helpers;
using IWBTM.Game.Overlays;
using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Create;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Input;
using System.Collections.Generic;

namespace IWBTM.Game.Screens
{
    public class CreationScreen : IWannaScreen
    {
        [Resolved]
        private NotificationOverlay notifications { get; set; }

        private readonly IWannaTextBox textbox;
        private readonly RoomCreationOverlay roomCreationOverlay;

        public CreationScreen()
        {
            ValidForResume = false;

            AddRangeInternal(new Drawable[]
            {
                new FillFlowContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10),
                    Children = new Drawable[]
                    {
                        new SettingName("Name"),
                        textbox = new IWannaTextBox
                        {
                            Width = 300,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        },
                        new SettingName("First room settings"),
                        new IWannaBasicButton("Adjust", onAdjust)
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre
                        },
                        new IWannaBasicButton("Ok", onCommit)
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Margin = new MarginPadding { Top = 20 }
                        }
                    }
                },
                roomCreationOverlay = new RoomCreationOverlay(),
            });
        }

        private void onAdjust() => roomCreationOverlay.Show();

        private void onCommit()
        {
            var name = textbox.Current.Value;

            if (!canBeCreated(name))
                return;

            var level = new Level
            {
                Rooms = new List<Room>
                {
                    roomCreationOverlay.GenerateRoom()
                }
            };

            this.Push(new EditorScreen(level, name));
        }

        private bool canBeCreated(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                notifications.Push("Level name can't be empty", NotificationState.Bad);
                return false;
            }

            if (LevelStorage.LevelExists(name))
            {
                notifications.Push($"\"{name}\" level already exists", NotificationState.Bad);
                return false;
            }

            return true;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        onCommit();
                        return true;
                }
            };

            return base.OnKeyDown(e);
        }
    }
}
