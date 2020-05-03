using osuTK;

namespace IWBTM.Game.Playfield
{
    public class BasicTile : Tile
    {
        public BasicTile(TileType type)
            : base(type)
        {
            Size = new Vector2(SIZE);
        }
    }
}
