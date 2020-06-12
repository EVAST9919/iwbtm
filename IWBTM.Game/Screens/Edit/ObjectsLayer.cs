using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using osu.Framework.Bindables;
using osuTK;
using System.Linq;

namespace IWBTM.Game.Screens.Edit
{
    public class ObjectsLayer : DrawableRoom
    {
        public readonly Bindable<int> SnapValue = new Bindable<int>();

        public ObjectsLayer(Room room)
            : base(room, true, true, false, false)
        {
        }

        public void UpdateAction(DrawableTile tile, TileAction action)
        {
            foreach (var child in Children)
            {
                if (child == tile)
                {
                    child.Tile.Action = action;
                    break;
                }
            }

            Save();
        }

        public void TryPlace(TileType type, Vector2 position)
        {
            var snappedPosition = BluePrint.GetSnappedPosition(position, SnapValue.Value);

            if (!this.Any())
            {
                addTile(type, snappedPosition);
                return;
            }

            if (type == TileType.PlayerStart || type == TileType.Warp)
            {
                addUniqueTile(snappedPosition, type);
                return;
            }

            if (HasTileAt(snappedPosition, type))
                return;

            addTile(type, snappedPosition);
        }

        public void TryRemove(Vector2 position)
        {
            var tileToRemove = GetAnyTileAt(position);
            tileToRemove?.Expire();

            Save();
        }

        private void addUniqueTile(Vector2 position, TileType type)
        {
            var tilesToRemove = GetAllTiles(type);
            foreach (var t in tilesToRemove)
                t.Expire();

            addTile(type, position);
        }

        private void addTile(TileType type, Vector2 position)
        {
            var tile = new Tile
            {
                Type = type,
                PositionX = (int)position.X,
                PositionY = (int)position.Y
            };

            AddTile(tile);
            Save();
        }

        public bool SpawnDefined() => HasTile(TileType.PlayerStart);

        public bool EndDefined() => HasTile(TileType.Warp);

        public void ClearTiles()
        {
            foreach (var c in Children)
                c.Expire();

            Save();
        }

        public void DeselectAll()
        {
            foreach (var child in Children)
                child.Deselect();
        }

        public void Save()
        {
            Scheduler.AddDelayed(() =>
            {
                Room.Tiles = Children.Select(c => new Tile
                {
                    Type = c.Tile.Type,
                    PositionX = (int)c.Position.X,
                    PositionY = (int)c.Position.Y,
                    Action = c.Tile.Action
                }).ToList();
            }, 10);
        }
    }
}
