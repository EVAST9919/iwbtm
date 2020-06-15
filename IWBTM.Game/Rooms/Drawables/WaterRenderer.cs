using osu.Framework.Graphics.Containers;
using osuTK;

namespace IWBTM.Game.Rooms.Drawables
{
    public class WaterRenderer : CompositeDrawable
    {
        public WaterRenderer(Room room)
        {
            Size = new Vector2(room.SizeX, room.SizeY) * DrawableTile.SIZE;

            foreach (var tile in room.Tiles)
            {
                if (tile.Type == TileType.Water3)
                    AddInternal(new DrawableTile(tile, room.Skin, false));
            }
        }
    }
}
