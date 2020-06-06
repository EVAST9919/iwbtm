using IWBTM.Game.Helpers;
using IWBTM.Game.Overlays;
using IWBTM.Game.Screens.Create;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
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
        private readonly SizeSetting sizeSetting;
        private readonly SizeAdjustmentOverlay sizeAdjustmentOverlay;

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
                        new SettingName("Size"),
                        sizeSetting = new SizeSetting
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        },
                        new SettingName("Audio"),
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
                },
                sizeAdjustmentOverlay = new SizeAdjustmentOverlay(),
            });

            sizeSetting.AdjustRequested += onSizeAdjust;
            sizeAdjustmentOverlay.NewSize += newSize => sizeSetting.Current.Value = newSize;
        }

        private void onSizeAdjust()
        {
            sizeAdjustmentOverlay.Show();
        }

        private void onCommit()
        {
            var name = textbox.Current.Value;

            if (!canBeCreated(name))
                return;

            RoomStorage.CreateRoomDirectory(name);
            var room = RoomStorage.CreateEmptyRoom(name, musicSelector.Current.Value, sizeSetting.Current.Value);
            this.Push(new EditorScreen(room, name));
        }

        private bool canBeCreated(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                notifications.Push("Name can't be empty", NotificationState.Bad);
                return false;
            }

            if (RoomStorage.RoomExists(name))
            {
                notifications.Push($"\"{name}\" room already exists", NotificationState.Bad);
                return false;
            }

            if (sizeSetting.Current.Value.X < 4 || sizeSetting.Current.Value.Y < 4)
            {
                notifications.Push("Width and Height can't be below 4", NotificationState.Bad);
                return false;
            }

            return true;
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

        private class SettingName : SpriteText
        {
            public SettingName(string text)
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                Text = text;
            }
        }
    }
}
