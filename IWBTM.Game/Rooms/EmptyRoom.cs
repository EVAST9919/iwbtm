using IWBTM.Game.Rooms.Drawables;
using System.Collections.Generic;

namespace IWBTM.Game.Rooms
{
    public class EmptyRoom : Room
    {
        public EmptyRoom()
        {
            Tiles = new List<Tile>
            {
                new Tile
                {
                    Type = TileType.PlayerStart,
                    PositionX = 12 * DrawableTile.SIZE,
                    PositionY = 17 * DrawableTile.SIZE
                }
            };
            Name = "Empty";
        }
    }
}
