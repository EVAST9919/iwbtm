using IWBTM.Game.Screens.Edit;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osu.Framework.Screens;
using osu.Framework.Allocation;
using IWBTM.Game.Rooms;
using IWBTM.Game.Overlays;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.Helpers;
using IWBTM.Game.Rooms.Drawables;

namespace IWBTM.Game.Screens
{
    public class EditorScreen : GameScreen
    {
        private readonly Bindable<TileType> selectedObject = new Bindable<TileType>();

        private readonly SpriteText selectedItemText;
        private readonly BluePrint blueprint;

        private NotificationOverlay notifications;

        public EditorScreen(Room room = null)
        {
            ToolBar toolbar;

            AddRangeInternal(new Drawable[]
            {
                selectedItemText = new SpriteText
                {
                    Margin = new MarginPadding(10),
                },
                new PlayfieldAdjustmentContainer
                {
                    Scale = new Vector2(0.9f),
                    Child = blueprint = new BluePrint(room)
                },
                toolbar = new ToolBar
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    OnTest = test,
                    OnSave = save
                }
            });

            selectedObject.BindTo(toolbar.Selected);
            blueprint.Selected.BindTo(toolbar.Selected);
            blueprint.SnapValue.BindTo(toolbar.SnapValue);

            if (room != null)
                toolbar.SetRoomName(room.Name);
        }

        [BackgroundDependencyLoader]
        private void load(NotificationOverlay notifications)
        {
            this.notifications = notifications;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            selectedObject.BindValueChanged(newSelected => selectedItemText.Text = $"Selected: {newSelected.NewValue.ToString()}", true);
        }

        private void test()
        {
            if (!blueprint.SpawnDefined())
            {
                notifications.Push("Player spawn is not defined", NotificationState.Bad);
                return;
            }

            this.Push(new TestGameplayScreen(new Room
            {
                Name = "",
                Tiles = blueprint.GetTiles()
            }));
        }

        private void save(string name)
        {
            if (!blueprint.SpawnDefined())
            {
                notifications.Push("Player spawn is not defined", NotificationState.Bad);
                return;
            }

            if (!blueprint.EndDefined())
            {
                notifications.Push("Room end is not defined", NotificationState.Bad);
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                notifications.Push("Set room name", NotificationState.Bad);
                return;
            }

            RoomStorage.CreateRoom(name, blueprint.GetTiles());
            notifications.Push("Room has been saved!", NotificationState.Good);
        }
    }
}
