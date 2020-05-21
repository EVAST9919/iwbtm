using IWBTM.Game.Rooms.Drawables;
using System.Collections.Generic;

namespace IWBTM.Game.Rooms
{
    public class BossRoom : Room
    {
        public BossRoom()
        {
            Tiles = new List<Tile>
            {
                new Tile
                {
                    Type = TileType.PlayerStart,
                    PositionX = 2 * DrawableTile.SIZE,
                    PositionY = 17 * DrawableTile.SIZE
                }
            };
            Name = "Miku";
        }
    }
}
