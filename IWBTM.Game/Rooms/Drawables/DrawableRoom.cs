using osu.Framework.Graphics.Containers;
using osuTK;
using System.Collections.Generic;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableRoom : Container<DrawableTile>
    {
        public Vector2 PlayerSpawnPosition { get; private set; }

        protected Room Room { get; set; }

        public DrawableRoom(Room room, bool showPlayerSpawn, bool showBulletBlocker, bool animatedCherry)
        {
            Room = room;
            Size = new Vector2(room.SizeX, room.SizeY) * DrawableTile.SIZE;

            if (room != null)
            {
                foreach (var t in room.Tiles)
                {
                    if (t.Type == TileType.Save)
                    {
                        Add(new SaveTile(t));
                        continue;
                    }

                    if (t.Type == TileType.BulletBlocker)
                    {
                        Add(new DrawableBulletBlocker(t, showBulletBlocker));
                        continue;
                    }

                    if (t.Type == TileType.Cherry)
                    {
                        Add(new DrawableCherry(t, animatedCherry));
                        continue;
                    }

                    if (t.Type == TileType.PlayerStart)
                    {
                        PlayerSpawnPosition = new Vector2(t.PositionX, t.PositionY);

                        if (showPlayerSpawn)
                            Add(new DrawableTile(t));

                        continue;
                    }

                    Add(new DrawableTile(t));
                }
            }
        }

        public bool HasTileAt(Vector2 pixelPosition, TileGroup group) => GetTileAt(pixelPosition, group) != null;

        public DrawableTile GetTileAt(Vector2 pixelPosition, TileGroup group)
        {
            foreach (var child in Children)
            {
                var tilePosition = child.Position;
                var tileSize = child.Size;

                if (pixelPosition.X >= tilePosition.X && pixelPosition.X < tilePosition.X + tileSize.X)
                {
                    if (pixelPosition.Y >= tilePosition.Y && pixelPosition.Y < tilePosition.Y + tileSize.Y)
                    {
                        if (DrawableTile.IsGroup(child, group))
                            return child;
                    }
                }
            }

            return null;
        }

        public List<DrawableTile> GetTilesAt(Vector2 pixelPosition)
        {
            var tiles = new List<DrawableTile>();

            foreach (var child in Children)
            {
                var tilePosition = child.Position;
                var tileSize = child.Size;

                if (pixelPosition.X >= tilePosition.X && pixelPosition.X < tilePosition.X + tileSize.X)
                {
                    if (pixelPosition.Y >= tilePosition.Y && pixelPosition.Y < tilePosition.Y + tileSize.Y)
                        tiles.Add(child);
                }
            }

            return tiles;
        }

        public static DrawableTile GetTileOfGroup(List<DrawableTile> tiles, TileGroup group)
        {
            foreach (var t in tiles)
            {
                if (DrawableTile.GetGroup(t.Tile.Type) == group)
                    return t;
            }

            return null;
        }

        public static bool ContainsTileOfGroup(List<DrawableTile> tiles, TileGroup group) => GetTileOfGroup(tiles, group) != null;
    }
}
