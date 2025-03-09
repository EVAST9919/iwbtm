using System.Collections.Generic;
using IWBTM.Game.Rooms;

namespace IWBTM.Game.BossCreation;

public class BossLevel
{
    public int Width { get; set; }

    public int Height { get; set; }

    public List<Tile> Tiles { get; set; }

    public Composition Root { get; set; }
}
