using System.Collections.Generic;

namespace IWBTM.Game.Rooms
{
    public class Room
    {
        public string Name { get; set; }

        public string Music { get; set; }

        public List<Tile> Tiles { get; set; }
    }
}
