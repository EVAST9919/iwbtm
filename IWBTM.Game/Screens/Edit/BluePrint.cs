using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using System.Linq;
using osuTK.Input;
using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;

namespace IWBTM.Game.Screens.Edit
{
    public class BluePrint : CompositeDrawable
    {
        public readonly Bindable<TileType> Selected = new Bindable<TileType>();
        public readonly Bindable<int> SnapValue = new Bindable<int>();
        public readonly Bindable<ToolEnum> Tool = new Bindable<ToolEnum>();
        public readonly Bindable<DrawableTile> TileToEdit = new Bindable<DrawableTile>();

        private readonly ObjectsLayer objectsLayer;
        private readonly Container hoverLayer;

        public BluePrint(Room room)
        {
            Size = new Vector2(room.SizeX, room.SizeY) * DrawableTile.SIZE;

            Grid grid;

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                objectsLayer = new ObjectsLayer(room),
                grid = new Grid(new Vector2(room.SizeX, room.SizeY)),
                hoverLayer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
            });

            grid.Current.BindTo(SnapValue);
            objectsLayer.SnapValue.BindTo(SnapValue);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Tool.BindValueChanged(onToolChanged, true);
        }

        private void onToolChanged(ValueChangedEvent<ToolEnum> tool)
        {
            switch (tool.NewValue)
            {
                case ToolEnum.Place:
                    TileToEdit.Value = null;
                    objectsLayer.DeselectAll();
                    return;

                case ToolEnum.Select:
                    return;
            }
        }

        public bool SpawnDefined() => objectsLayer.SpawnDefined();

        public bool EndDefined() => objectsLayer.EndDefined();

        public void Clear() => objectsLayer.ClearTiles();

        private DrawableTile tileToPlace;
        private Vector2 mousePosition;

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            mousePosition = e.MousePosition;

            if (Tool.Value == ToolEnum.Select)
            {
                hoverLayer.Clear();
                tileToPlace?.Expire();
                return false;
            }

            if (!hoverLayer.Any())
                hoverLayer.Child = tileToPlace = createTile(new Tile { Type = Selected.Value }).With(t =>
                {
                    t.Alpha = 0.5f;
                });

            tileToPlace.Position = GetSnappedPosition(mousePosition, SnapValue.Value);

            var buttons = e.CurrentState.Mouse.Buttons;

            if (buttons.IsPressed(MouseButton.Left))
            {
                objectsLayer.TryPlace(Selected.Value, mousePosition);
                return true;
            }

            if (buttons.IsPressed(MouseButton.Right))
            {
                objectsLayer.TryRemove(mousePosition);
                return true;
            }

            return base.OnMouseMove(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            if (IsDisposed)
                return;

            hoverLayer.Clear();
            tileToPlace?.Expire();
        }

        public void ReplaceAction(DrawableTile old, DrawableTile newTile)
        {
            objectsLayer.ReplaceAction(old, newTile);
            TileToEdit.Value = newTile;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            switch (Tool.Value)
            {
                case ToolEnum.Place:
                    switch (e.Button)
                    {
                        case MouseButton.Left:
                            objectsLayer.TryPlace(Selected.Value, mousePosition);
                            return true;

                        case MouseButton.Right:
                            objectsLayer.TryRemove(mousePosition);
                            return true;
                    }
                    break;

                case ToolEnum.Select:
                    objectsLayer.DeselectAll();

                    var selectedTile = objectsLayer.GetAnyTileAt(mousePosition);
                    if (selectedTile == null)
                    {
                        TileToEdit.Value = null;
                        return true;
                    }

                    TileToEdit.Value = selectedTile;
                    selectedTile.Select();
                    break;
            }

            return base.OnMouseDown(e);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (TileToEdit == null)
                return base.OnKeyDown(e);

            float newX, newY;
            var tile = TileToEdit.Value;

            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        newY = tile.Y - SnapValue.Value;
                        if (newY >= 0)
                        {
                            tile.MoveToY(newY);
                            TileToEdit.Value = null;
                            TileToEdit.Value = tile;
                            objectsLayer.Save();
                        }
                        return true;

                    case Key.Down:
                        newY = tile.Y + SnapValue.Value;
                        if (newY < Size.Y)
                        {
                            tile.MoveToY(newY);
                            TileToEdit.Value = null;
                            TileToEdit.Value = tile;
                            objectsLayer.Save();
                        }
                        return true;

                    case Key.Left:
                        newX = tile.X - SnapValue.Value;
                        if (newX >= 0)
                        {
                            tile.MoveToX(newX);
                            TileToEdit.Value = null;
                            TileToEdit.Value = tile;
                            objectsLayer.Save();
                        }
                        return true;

                    case Key.Right:
                        newX = tile.X + SnapValue.Value;
                        if (newX < Size.X)
                        {
                            tile.MoveToX(newX);
                            TileToEdit.Value = null;
                            TileToEdit.Value = tile;
                            objectsLayer.Save();
                        }
                        return true;

                    case Key.Delete:
                        tile.Expire();
                        TileToEdit.Value = null;
                        objectsLayer.Save();
                        return true;
                }
            }

            return base.OnKeyDown(e);
        }

        public static Vector2 GetSnappedPosition(Vector2 input, int snapValue)
        {
            return new Vector2((int)(input.X / snapValue), (int)(input.Y / snapValue)) * snapValue;
        }

        private static DrawableTile createTile(Tile tile)
        {
            if (tile.Type == TileType.Cherry)
                return new DrawableCherry(tile, false);

            return new DrawableTile(tile);
        }
    }
}
