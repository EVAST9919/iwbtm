using IWBTM.Game.Playfield;
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
using osu.Framework.Platform;
using System.IO;
using IWBTM.Game.Rooms;
using IWBTM.Game.Overlays;
using osu.Framework.Graphics.UserInterface;

namespace IWBTM.Game.Screens
{
    public class EditorScreen : GameScreen
    {
        private readonly Bindable<TileType> selectedObject = new Bindable<TileType>();

        private SpriteText selectedText;
        private BluePrint blueprint;
        private EditorTextbox textbox;

        private Storage storage;

        private NotificationOverlay notifications;

        [BackgroundDependencyLoader]
        private void load(Storage storage, NotificationOverlay notifications)
        {
            this.storage = storage.GetStorageForDirectory(@"Rooms");
            this.notifications = notifications;

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
                    Child = blueprint = new BluePrint()
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

            this.Push(new TestGameplayScreen(new Room(blueprint.Layout(), blueprint.PlayerSpawnPosition())));
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

            using (StreamWriter sw = File.CreateText(storage.GetFullPath(name)))
            {
                sw.WriteLine(blueprint.Layout());
                sw.WriteLine(playerPosition.X.ToString());
                sw.WriteLine(playerPosition.Y.ToString());
            }

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
