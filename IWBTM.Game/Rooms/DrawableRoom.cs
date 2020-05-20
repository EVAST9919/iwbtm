﻿using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace IWBTM.Game.Rooms
{
    public class DrawableRoom : CompositeDrawable
    {
        private readonly Room room;
        private readonly bool showPlayerSpawn;

        public DrawableRoom(Room room, bool showPlayerSpawn = false)
        {
            this.room = room;
            this.showPlayerSpawn = showPlayerSpawn;

            Size = DefaultPlayfield.BASE_SIZE;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            for (int i = 0; i < DefaultPlayfield.TILES_WIDTH; i++)
            {
                for (int j = 0; j < DefaultPlayfield.TILES_HEIGHT; j++)
                {
                    var tile = room.GetTileAt(i, j);

                    if (!Room.TileIsEmpty(tile))
                    {
                        AddInternal(new Tile(GetTileType(tile))
                        {
                            Position = new Vector2(i * Tile.SIZE, j * Tile.SIZE),
                        });
                    }
                }
            }

            if (showPlayerSpawn)
            {
                AddInternal(new Tile(TileType.PlayerStart)
                {
                    Position = room.GetPlayerSpawnPosition()
                });
            }
        }

        public static TileType GetTileType(char input)
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

                case 's':
                    return TileType.Save;

                default:
                    throw new NotImplementedException($"char {input} is not supported");
            }
        }
    }
}
