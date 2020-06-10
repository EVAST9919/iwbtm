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
            : base(room, true, true)
        {
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

            DrawableTile placed = null;

            foreach (var child in Children)
            {
                if (child.Position == snappedPosition)
                {
                    placed = child;
                    break;
                }
            }

            if (placed == null)
            {
                addTile(type, snappedPosition);
                return;
            }

            if (placed.Tile.Type == type)
                return;

            addTile(type, snappedPosition);
        }

        public void TryRemove(Vector2 position)
        {
            foreach (var child in Children)
            {
                if (position.X >= child.X && position.X < child.X + child.Size.X)
                {
                    if (position.Y >= child.Y && position.Y < child.Y + child.Size.Y)
                        child.Expire();
                }
            }

            save();
        }

        private void addUniqueTile(Vector2 position, TileType type)
        {
            foreach (var child in Children)
            {
                if (child.Tile.Type == type)
                    child.Expire();
            }

            foreach (var child in Children)
            {
                if (child.Position == position)
                {
                    child.Expire();
                    break;
                }
            }

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

            if (tile.Type == TileType.Cherry)
                Add(new DrawableCherry(tile));
            else
                Add(new DrawableTile(tile));

            save();
        }

        public bool SpawnDefined()
        {
            foreach (var child in Children)
            {
                if (DrawableTile.IsGroup(child, TileGroup.Start))
                    return true;
            }

            return false;
        }

        public bool EndDefined()
        {
            foreach (var child in Children)
            {
                if (DrawableTile.IsGroup(child, TileGroup.Warp))
                    return true;
            }

            return false;
        }

        public void ClearTiles()
        {
            foreach (var c in Children)
                c.Expire();

            save();
        }

        private void save()
        {
            Scheduler.AddDelayed(() =>
            {
                Room.Tiles = Children.Select(c => c.Tile).ToList();
            }, 10);
        }
    }
}
