using osuTK;
using System.Collections.Generic;

namespace IWBTM.Game.Rooms
{
    public class Room
    {
        public string Music { get; set; }

        public List<Tile> Tiles { get; set; }

        public float SizeX { get; set; } = 24;

        public float SizeY { get; set; } = 19;
    }
}
