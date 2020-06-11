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
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Input;
using System.Collections.Generic;
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
        private readonly RoomEditOverlay roomSettings;
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
                                OnRoomSelect = selectRoom,
                                OnClear = clear,
                                OnSettings = onSettings
                            }
                        }
                    }
                },
                roomSelector = new RoomSelectorOverlay(),
                roomSettings = new RoomEditOverlay()
            });

            roomSelector.Rooms.BindTo(rooms);
            roomSelector.Selected.BindTo(selectedRoom);

            selectedObject.BindTo(toolbar.Selected);

            roomSettings.CreatedRoom += onRoomChanged;
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

            this.Push(new TestGameplayScreen(new Level
            {
                Rooms = new List<Room>
                {
                    new Room
                    {
                        Music = room.Music,
                        Tiles = blueprint.GetTiles(),
                        SizeX = room.SizeX,
                        SizeY = room.SizeY
                    }
                }
            }, name));
        }

        private void save()
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (!RoomHelper.SpawnDefined(rooms[i]))
                {
                    notifications.Push($"Player spawn is not defined in room-{i}", NotificationState.Bad);
                    return;
                }

                if (!RoomHelper.EndDefined(rooms[i]))
                {
                    notifications.Push($"Room end is not defined in room-{i}", NotificationState.Bad);
                    return;
                }
            }

            LevelStorage.CreateLevel(name, new Level
            {
                Rooms = rooms.ToList()
            });
            notifications.Push("Level has been saved!", NotificationState.Good);
        }

        private void clear()
        {
            confirmationOverlay.Push("Are you sure you want to clear entire room?", blueprint.Clear);
        }

        private void selectRoom() => roomSelector.Show();

        private void onSettings()
        {
            roomSettings.Edit(selectedRoom.Value);
        }

        private void onRoomChanged(Room room)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i] == selectedRoom.Value)
                {
                    rooms[i] = room;
                    selectedRoom.Value = rooms[i];
                    return;
                }
            }
        }

        protected override void OnExit()
        {
            confirmationOverlay.Push("Are you sure you want to exit? All unsaved progress will be lost.", () => base.OnExit());
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.ControlPressed)
            {
                if (!e.Repeat)
                {
                    switch (e.Key)
                    {
                        case Key.S:
                            save();
                            return true;
                    }
                }
            }

            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Tab:
                        selectRoom();
                        return true;

                    case Key.Enter:
                        test();
                        return true;
                }
            }

            return base.OnKeyDown(e);
        }
    }
}
