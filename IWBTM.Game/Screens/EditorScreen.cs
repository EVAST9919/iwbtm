using IWBTM.Game.Helpers;
using IWBTM.Game.Overlays;
using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Create;
using IWBTM.Game.Screens.Edit;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
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
        private readonly BindableList<Room> rooms = new BindableList<Room>();
        private readonly Bindable<Room> selectedRoom = new Bindable<Room>();

        private BluePrint blueprint;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        [Resolved]
        private ConfirmationOverlay confirmationOverlay { get; set; }

        private readonly string name;
        private readonly RoomSelectorOverlay roomSelector;
        private readonly RoomEditOverlay roomSettings;
        private readonly CherriesEditOverlay cherriesSettings;
        private readonly Container drawableRoomPlaceholder;
        private readonly ToolBar toolbar;
        private readonly ToolSelector toolSelector;

        public EditorScreen(Level level, string name)
        {
            this.name = name;
            rooms.AddRange(level.Rooms);
            selectedRoom.Value = rooms[0];

            AddRangeInternal(new Drawable[]
            {
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ColumnDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Absolute, 200),
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
                            toolSelector = new ToolSelector
                            {
                                Depth = -3
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Children = new Drawable[]
                                {
                                    drawableRoomPlaceholder = new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                    },
                                    new IWannaBasicButton("Move to centre", moveToCentre)
                                    {
                                        Margin = new MarginPadding(10)
                                    }
                                }
                            },
                            toolbar = new ToolBar()
                            {
                                OnTest = test,
                                OnSave = save,
                                OnRoomSelect = selectRoom,
                                OnClear = clear,
                                OnSettings = onSettings,
                                OnCherries = onCherries,
                            }
                        }
                    }
                },
                cherriesSettings = new CherriesEditOverlay(),
                roomSelector = new RoomSelectorOverlay(),
                roomSettings = new RoomEditOverlay()
            });

            roomSelector.Rooms.BindTo(rooms);
            roomSelector.Selected.BindTo(selectedRoom);

            roomSettings.CreatedRoom += onRoomChanged;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
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

            moveToCentre();

            blueprint.Selected.BindTo(toolbar.Selected);
            blueprint.SnapValue.BindTo(toolbar.SnapValue);
            blueprint.Tool.BindTo(toolSelector.Current);
            toolSelector.SelectedTile.BindTo(blueprint.TileToEdit);

            toolSelector.Edited += blueprint.UpdateAction;
            cherriesSettings.Skin = room.NewValue.Skin;
            cherriesSettings.PreviewUpdated += blueprint.UpdateCherriesPreview;
            cherriesSettings.CherriesAdded += blueprint.AddCherriesRange;
        }

        private void moveToCentre()
        {
            drawableRoomPlaceholder.ScaleTo(Vector2.One, 200, Easing.Out);
            drawableRoomPlaceholder.MoveTo(Vector2.Zero, 200, Easing.Out);
        }

        private void test()
        {
            if (!blueprint.SpawnDefined())
            {
                notifications.Push("Player spawn is not defined", NotificationState.Bad);
                return;
            }

            this.Push(new TestGameplayScreen(new Level
            {
                Rooms = new List<Room>
                {
                    selectedRoom.Value
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

                if (rooms[i].RoomCompletionType == RoomCompletionType.Warp && !RoomHelper.EndDefined(rooms[i]))
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

        private void onCherries()
        {
            cherriesSettings.Show();
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

        protected override bool OnScroll(ScrollEvent e)
        {
            base.OnScroll(e);

            var delta = e.ScrollDelta.Y;
            var scrollDeltaFloat = (e.IsPrecise ? 10 : 80) * delta;
            if (e.ShiftPressed)
                scrollDeltaFloat *= 10;

            if (e.AltPressed)
            {
                var oldScale = drawableRoomPlaceholder.Scale;
                var newScale = oldScale.X + scrollDeltaFloat / 100;
                if (newScale <= 0)
                    newScale = 0;

                drawableRoomPlaceholder.ScaleTo(newScale, 100, Easing.Out);
                return true;
            }

            if (e.ControlPressed)
            {
                drawableRoomPlaceholder.MoveToOffset(new Vector2(scrollDeltaFloat, 0), 100, Easing.Out);
                return true;
            }

            drawableRoomPlaceholder.MoveToOffset(new Vector2(0, scrollDeltaFloat), 100, Easing.Out);
            return true;
        }
    }
}
