using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osuTK;
using System.Linq;

namespace IWBTM.Game.Screens.Edit
{
    public class ObjectsLayer : Container<DrawableTile>
    {
        public readonly Bindable<int> SnapValue = new Bindable<int>();

        public ObjectsLayer()
        {
            Size = DefaultPlayfield.BASE_SIZE;
        }

        public void TryPlace(TileType type, Vector2 position)
        {
            var snappedPosition = BluePrint.GetSnappedPosition(position, SnapValue.Value);

            if (!this.Any())
            {
                addTile(type, snappedPosition);
                return;
            }

            if (type == TileType.PlayerStart)
            {
                tryPlacePlayerStart(snappedPosition);
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

            placed.Expire();
            addTile(type, snappedPosition);
        }

        public void TryRemove(Vector2 position)
        {
            foreach (var child in Children)
            {
                if (position.X >= child.X && position.X <= child.X + DrawableTile.SIZE - 1)
                {
                    if (position.Y >= child.Y && position.Y <= child.Y + DrawableTile.SIZE - 1)
                        child.Expire();
                }
            }
        }

        private void tryPlacePlayerStart(Vector2 position)
        {
            foreach (var child in Children)
            {
                if (child.Tile.Type == TileType.PlayerStart)
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

            addTile(TileType.PlayerStart, position);
        }

        private void addTile(TileType type, Vector2 position)
        {
            var tile = new Tile
            {
                Type = type,
                PositionX = (int)position.X,
                PositionY = (int)position.Y
            };

            Add(new DrawableTile(tile));
        }

        public void SetRoom(Room room)
        {
            foreach (var t in room.Tiles)
                Add(new DrawableTile(t));
        }

        public Vector2 GetPlayerSpawnPosition()
        {
            DrawableTile player = null;

            foreach (var child in Children)
            {
                if (child.Tile.Type == TileType.PlayerStart)
                {
                    player = child;
                    break;
                }
            }

            if (player == null)
                return new Vector2(-1);

            return BluePrint.GetSnappedPosition(player.Position, SnapValue.Value);
        }
    }
}
