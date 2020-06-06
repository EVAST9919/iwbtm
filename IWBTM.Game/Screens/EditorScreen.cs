using IWBTM.Game.Helpers;
using IWBTM.Game.Overlays;
using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Edit;
using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using System.Linq;

namespace IWBTM.Game.Screens
{
    public class EditorScreen : IWannaScreen
    {
        private readonly Bindable<TileType> selectedObject = new Bindable<TileType>();
        private readonly BindableList<Room> rooms = new BindableList<Room>();
        private readonly Bindable<Room> selectedRoom = new Bindable<Room>();

        private readonly SpriteText selectedItemText;
        private BluePrint blueprint;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        [Resolved]
        private ConfirmationOverlay confirmationOverlay { get; set; }

        private readonly string name;
        private readonly RoomSelectorOverlay roomSelector;
        private readonly Container drawableRoomPlaceholder;
        private readonly ToolBar toolbar;

        public EditorScreen(Level level, string name)
        {
            this.name = name;
            rooms.AddRange(level.Rooms);
            selectedRoom.Value = rooms[0];

            AddRangeInternal(new Drawable[]
            {
                selectedItemText = new SpriteText
                {
                    Margin = new MarginPadding(10),
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ColumnDimensions = new[]
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.Absolute, 200)
                    },
                    RowDimensions = new[]
                    {
                        new Dimension()
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            drawableRoomPlaceholder = new Container
                            {
                                RelativeSizeAxes = Axes.Both
                            },
                            toolbar = new ToolBar()
                            {
                                OnTest = test,
                                OnSave = save,
                                OnRoomSelect = selectRoom
                            }
                        }
                    }
                },
                roomSelector = new RoomSelectorOverlay()
            });

            roomSelector.Rooms.BindTo(rooms);
            roomSelector.Selected.BindTo(selectedRoom);

            selectedObject.BindTo(toolbar.Selected);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            selectedObject.BindValueChanged(newSelected => selectedItemText.Text = $"Selected: {newSelected.NewValue.ToString()}", true);
            selectedRoom.BindValueChanged(onSelectedRoomChanged, true);
        }

        private void onSelectedRoomChanged(ValueChangedEvent<Room> room)
        {
            var newRoom = room.NewValue;

            drawableRoomPlaceholder.Child = new FullRoomPreviewContainer(new Vector2(newRoom.SizeX, newRoom.SizeY))
            {
                Scale = new Vector2(0.9f),
                Child = blueprint = new BluePrint(newRoom)
            };

            blueprint.Selected.BindTo(toolbar.Selected);
            blueprint.SnapValue.BindTo(toolbar.SnapValue);
        }

        private void test()
        {
            if (!blueprint.SpawnDefined())
            {
                notifications.Push("Player spawn is not defined", NotificationState.Bad);
                return;
            }

            var room = selectedRoom.Value;

            this.Push(new TestGameplayScreen(new Room
            {
                Music = room.Music,
                Tiles = blueprint.GetTiles(),
                SizeX = room.SizeX,
                SizeY = room.SizeY
            }, name));
        }

        private void save()
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

            LevelStorage.UpdateLevel(name, new Level
            {
                Rooms = rooms.ToList()
            });
            notifications.Push("Level has been saved!", NotificationState.Good);
        }

        private void selectRoom()
        {
            roomSelector.Show();
        }

        protected override void OnExit()
        {
            confirmationOverlay.Push("Are you sure you want to exit? All unsaved progress will be lost.", () => base.OnExit());
        }
    }
}
