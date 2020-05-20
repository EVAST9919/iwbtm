using IWBTM.Game.Screens.Edit;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Screens;
using osu.Framework.Allocation;
using IWBTM.Game.Rooms;
using IWBTM.Game.Overlays;
using osu.Framework.Graphics.UserInterface;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.Helpers;

namespace IWBTM.Game.Screens
{
    public class EditorScreen : GameScreen
    {
        private readonly Bindable<TileType> selectedObject = new Bindable<TileType>();

        private SpriteText selectedText;
        private BluePrint blueprint;
        private EditorTextbox textbox;

        private NotificationOverlay notifications;

        public EditorScreen(Room room = null)
        {
            ObjectSelector selector;

            AddRangeInternal(new Drawable[]
            {
                selectedText = new SpriteText
                {
                    Margin = new MarginPadding(10),
                },
                new PlayfieldAdjustmentContainer
                {
                    Scale = new Vector2(0.9f),
                    Child = blueprint = new BluePrint(room)
                },
                selector = new ObjectSelector
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight
                },
                new Container
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Margin = new MarginPadding(50),
                    AutoSizeAxes = Axes.Both,
                    Child = new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(10),
                        Children = new Drawable[]
                        {
                            textbox = new EditorTextbox(),
                            new EditorButon("Test")
                            {
                                Action = test
                            },
                            new EditorButon("Save")
                            {
                                Action = save
                            }
                        }
                    }
                }
            });

            selectedObject.BindTo(selector.Selected);
            blueprint.Selected.BindTo(selector.Selected);

            if (room != null)
                textbox.Text = room.Name;
        }

        [BackgroundDependencyLoader]
        private void load(NotificationOverlay notifications)
        {
            this.notifications = notifications;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            selectedObject.BindValueChanged(newSelected => selectedText.Text = $"Selected: {newSelected.NewValue.ToString()}", true);
        }

        private void test()
        {
            if (blueprint.PlayerSpawnPosition() == new Vector2(-1))
            {
                notifications.Push("Set player spawn position", NotificationState.Bad);
                return;
            }

            this.Push(new TestGameplayScreen(new Room("", blueprint.Layout(), blueprint.PlayerSpawnPosition())));
        }

        private void save()
        {
            var playerPosition = blueprint.PlayerSpawnPosition();

            if (playerPosition == new Vector2(-1))
            {
                notifications.Push("Set player spawn position", NotificationState.Bad);
                return;
            }

            var name = textbox.Text;

            if (string.IsNullOrEmpty(name))
            {
                notifications.Push("Set room name", NotificationState.Bad);
                return;
            }

            RoomStorage.CreateRoom(name, blueprint.Layout(), playerPosition);
            notifications.Push("Room has been saved!", NotificationState.Good);
        }

        private class EditorTextbox : BasicTextBox
        {
            public EditorTextbox()
            {
                Height = 30;
                Width = 100;
            }
        }

        private class EditorButon : ClickableContainer
        {
            public EditorButon(string text)
            {
                Size = new Vector2(100, 50);

                AddRangeInternal(new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    new SpriteText
                    {
                        Text = text,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Color4.Black
                    }
                });
            }
        }
    }
}
