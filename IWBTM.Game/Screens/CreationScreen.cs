using IWBTM.Game.Helpers;
using IWBTM.Game.Overlays;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Input;

namespace IWBTM.Game.Screens
{
    public class CreationScreen : IWannaScreen
    {
        [Resolved]
        private ConfirmationOverlay confirmationOverlay { get; set; }

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        private readonly IWannaTextBox textbox;
        private readonly MusicSelector musicSelector;

        public CreationScreen()
        {
            ValidForResume = false;

            AddInternal(new FillFlowContainer
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
                        Text = "Room name"
                    },
                    textbox = new IWannaTextBox
                    {
                        Width = 300,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                    new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = "Room audio"
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
                    new IWannaBasicButton("Ok", onCommit)
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    }
                }
            });
        }

        private void onCommit()
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

            RoomStorage.CreateRoomDirectory(text);
            var room = RoomStorage.CreateEmptyRoom(text, musicSelector.Current.Value);
            this.Push(new EditorScreen(room, text));
        }

        protected override void OnExit()
        {
            confirmationOverlay.Push("Are you sure you want to exit? All unsaved progress will be lost.", () => base.OnExit());
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
