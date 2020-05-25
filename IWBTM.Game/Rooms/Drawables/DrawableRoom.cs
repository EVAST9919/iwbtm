using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableRoom : Container<DrawableTile>
    {
        public Room Room { get; private set; }

        public Vector2 PlayerSpawnPosition { get; private set; }

        public DrawableRoom(Room room, bool showPlayerSpawn = false)
        {
            Room = room;

            Size = DefaultPlayfield.BASE_SIZE;

            foreach (var t in Room.Tiles)
            {
                if (t.Type == TileType.Save)
                {
                    Add(new SaveTile(t));
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

        public bool HasTileAt(Vector2 pixelPosition, TileGroup group) => GetTileAt(pixelPosition, group) != null;

        public DrawableTile GetTileAt(Vector2 pixelPosition, TileGroup group)
        {
            foreach (var child in Children)
            {
                var tilePosition = child.Position;
                var tileSize = child.Size;

                if (pixelPosition.X >= tilePosition.X && pixelPosition.X <= tilePosition.X + tileSize.X - 1)
                {
                    if (pixelPosition.Y >= tilePosition.Y && pixelPosition.Y <= tilePosition.Y + tileSize.Y - 1)
                    {
                        if (DrawableTile.IsGroup(child, group))
                            return child;
                    }
                }
            }

            return null;
        }
    }
}
