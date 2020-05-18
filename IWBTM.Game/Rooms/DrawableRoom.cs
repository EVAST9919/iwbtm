﻿using IWBTM.Game.Playfield;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace IWBTM.Game.Rooms
{
    public class DrawableRoom : CompositeDrawable
    {
        private readonly Room map;

        public DrawableRoom(Room map)
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

                    if (!Room.TileIsEmpty(tile))
                    {
                        AddInternal(new Tile(getTileType(tile))
                        {
                            Position = new Vector2(i * Tile.SIZE, j * Tile.SIZE),
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

                case 'q':
                    return TileType.SpikeBottom;

                case 'w':
                    return TileType.SpikeTop;

                case 'e':
                    return TileType.SpikeLeft;

                case 'r':
                    return TileType.SpikeRight;

                default:
                    throw new NotImplementedException($"char {input} is not supported");
            }
        }
    }
}
