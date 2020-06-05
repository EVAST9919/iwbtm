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

namespace IWBTM.Game.Screens
{
    public class EditorScreen : IWannaScreen
    {
        private readonly Bindable<TileType> selectedObject = new Bindable<TileType>();

        private readonly SpriteText selectedItemText;
        private readonly BluePrint blueprint;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        [Resolved]
        private ConfirmationOverlay confirmationOverlay { get; set; }

        private readonly Room room;
        private readonly string name;

        public EditorScreen(Room room, string name)
        {
            this.room = room;
            this.name = name;

            ToolBar toolbar;

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
                            new PlayfieldAdjustmentContainer
                            {
                                Scale = new Vector2(0.9f),
                                Child = blueprint = new BluePrint(room)
                            },
                            toolbar = new ToolBar()
                            {
                                OnTest = test,
                                OnSave = save
                            }
                        }
                    }
                },
            });

            selectedObject.BindTo(toolbar.Selected);
            blueprint.Selected.BindTo(toolbar.Selected);
            blueprint.SnapValue.BindTo(toolbar.SnapValue);
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
                Music = room.Music,
                Tiles = blueprint.GetTiles()
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

            RoomStorage.UpdateRoomTiles(room, name, blueprint.GetTiles());
            notifications.Push("Room has been saved!", NotificationState.Good);
        }

        protected override void OnExit()
        {
            confirmationOverlay.Push("Are you sure you want to exit? All unsaved progress will be lost.", () => base.OnExit());
        }
    }
}
