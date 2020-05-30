using System.Collections.Generic;

namespace IWBTM.Game.Rooms
{
    public class Room
    {
        public string Music { get; set; }

        public bool CustomAudio { get; set; }

        public List<Tile> Tiles { get; set; }
    }
}
