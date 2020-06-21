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
using System.Collections.Generic;

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
        private readonly ActionsLayer actionsLayer;
        private readonly Container cherriesLayer;
        private readonly Room room;

        public BluePrint(Room room)
        {
            this.room = room;

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
                actionsLayer = new ActionsLayer(),
                hoverLayer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
                cherriesLayer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                }
            });

            grid.Current.BindTo(SnapValue);
            objectsLayer.SnapValue.BindTo(SnapValue);

            objectsLayer.Updated += tiles => actionsLayer.UpdateActions(tiles);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            objectsLayer.Save();
            Tool.BindValueChanged(onToolChanged, true);
        }

        private void onToolChanged(ValueChangedEvent<ToolEnum> tool)
        {
            switch (tool.NewValue)
            {
                case ToolEnum.Place:
                    TileToEdit.Value = null;
                    objectsLayer.DeselectAll();
                    actionsLayer.Hide();
                    return;

                case ToolEnum.Select:
                    actionsLayer.Show();
                    return;
            }
        }

        public void UpdateCherriesPreview(List<DrawableTile> cherries)
        {
            cherriesLayer.Clear();

            if (cherries.Any())
                cherriesLayer.Children = cherries;
        }

        public void AddCherriesRange(List<DrawableTile> cherries)
        {
            cherries.ForEach(c =>
            {
                objectsLayer.AddTile(c.Tile, false);
            });

            objectsLayer.Save();
        }

        public bool SpawnDefined() => objectsLayer.SpawnDefined();

        public bool EndDefined() => objectsLayer.EndDefined();

        public void Clear() => objectsLayer.ClearTiles();

        private DrawableTile tileToPlace;
        private Vector2 mousePosition;

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (!IsHovered)
                return false;

            mousePosition = e.MousePosition;
            var buttons = e.CurrentState.Mouse.Buttons;

            if (Tool.Value == ToolEnum.Select)
            {
                if (buttons.IsPressed(MouseButton.Left))
                {
                    var tile = TileToEdit.Value;

                    if (tile != null)
                    {
                        tile.Position = GetSnappedPosition(mousePosition, SnapValue.Value, tile.Tile.Type);
                        reselectTile(tile);
                        objectsLayer.Save();
                    }
                }

                return true;
            }

            if (!hoverLayer.Any())
                hoverLayer.Child = tileToPlace = createTile(new Tile { Type = Selected.Value }, room.Skin).With(t =>
                {
                    t.Alpha = 0.5f;
                });

            tileToPlace.Position = GetSnappedPosition(mousePosition, SnapValue.Value, Selected.Value);

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

        protected override bool OnHover(HoverEvent e)
        {
            if (Tool.Value == ToolEnum.Select)
            {
                hoverLayer.Clear();
                tileToPlace?.Expire();
                return true;
            }

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            if (IsDisposed)
                return;

            hoverLayer.Clear();
            tileToPlace?.Expire();
        }

        public void UpdateAction(DrawableTile tile, TileAction action)
        {
            objectsLayer.UpdateAction(tile, action);
            reselectTile(tile);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (!IsHovered)
                return false;

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

                    var selectedTile = objectsLayer.GetFirstTileAt(mousePosition);
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
            if (TileToEdit.Value == null)
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
                            reselectTile(tile);
                            objectsLayer.Save();
                        }
                        return true;

                    case Key.Down:
                        newY = tile.Y + SnapValue.Value;
                        if (newY < Size.Y)
                        {
                            tile.MoveToY(newY);
                            reselectTile(tile);
                            objectsLayer.Save();
                        }
                        return true;

                    case Key.Left:
                        newX = tile.X - SnapValue.Value;
                        if (newX >= 0)
                        {
                            tile.MoveToX(newX);
                            reselectTile(tile);
                            objectsLayer.Save();
                        }
                        return true;

                    case Key.Right:
                        newX = tile.X + SnapValue.Value;
                        if (newX < Size.X)
                        {
                            tile.MoveToX(newX);
                            reselectTile(tile);
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

        public static Vector2 GetSnappedPosition(Vector2 input, int snapValue, TileType type)
        {
            var position = new Vector2((int)(input.X / snapValue), (int)(input.Y / snapValue)) * snapValue;

            if (type == TileType.Cherry || type == TileType.Jumprefresher)
                position -= DrawableTile.GetSize(type) / 2f;

            return position;
        }

        private DrawableTile createTile(Tile tile, string skin)
        {
            if (tile.Type == TileType.Cherry)
                return new DrawableCherry(tile, skin, false, false);

            return new DrawableTile(tile, skin, false);
        }

        private void reselectTile(DrawableTile tile)
        {
            TileToEdit.Value = null;
            TileToEdit.Value = tile;
        }
    }
}
