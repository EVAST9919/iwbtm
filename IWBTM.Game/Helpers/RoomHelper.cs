using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Player;
using osuTK;

namespace IWBTM.Game.Helpers
{
    public static class RoomHelper
    {
        public static Vector2 PlayerSpawnPosition(Room room)
        {
            Tile spawn = null;

            foreach (var tile in room.Tiles)
            {
                if (tile.Type == TileType.PlayerStart)
                {
                    spawn = tile;
                    break;
                }
            }

            if (spawn == null)
                return Vector2.Zero;

            return new Vector2(spawn.PositionX + 16, spawn.PositionY + DrawableTile.SIZE - DefaultPlayer.SIZE.Y / 2f);
        }
    }
}
