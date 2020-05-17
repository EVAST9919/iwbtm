using IWBTM.Game.Playfield;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace IWBTM.Game.Maps
{
    public class DrawableMap : CompositeDrawable
    {
        private readonly Map map;

        public DrawableMap(Map map)
        {
            this.map = map;

            Size = DefaultPlayfield.BASE_SIZE;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            for (int i = 0; i < DefaultPlayfield.TILES_WIDTH; i++)
            {
                for (int j = 0; j < DefaultPlayfield.TILES_HEIGHT; j++)
                {
                    var tile = map.GetTileAt(i, j);

                    if (tile != ' ')
                    {
                        AddInternal(new Tile(getTileType(tile))
                        {
                            Position = new Vector2(i * Tile.SIZE, j * Tile.SIZE),
                            Size = new Vector2(Tile.SIZE + 0.1f)
                        });
                    }
                }
            }
        }

        private TileType getTileType(char input)
        {
            switch (input)
            {
                case '+':
                    return TileType.PlatformCorner;

                case 'X':
                    return TileType.PlatformMiddle;

                case '-':
                    return TileType.PlatformMiddleRotated;

                default:
                    throw new NotImplementedException($"char {input} is not supported");
            }
        }
    }
}
