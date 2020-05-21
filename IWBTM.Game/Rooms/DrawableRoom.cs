using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace IWBTM.Game.Rooms
{
    public class DrawableRoom : Container<Tile>
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
                        var type = GetTileType(tile);
                        var position = new Vector2(i * Tile.SIZE, j * Tile.SIZE);

                        if (type == TileType.Save)
                        {
                            Add(new SaveTile
                            {
                                Position = position
                            });
                        }
                        else
                        {
                            Add(new Tile(type)
                            {
                                Position = position
                            });
                        }
                    }
                }
            }

            if (showPlayerSpawn)
            {
                Add(new Tile(TileType.PlayerStart)
                {
                    Position = room.GetPlayerSpawnPosition()
                });
            }
        }

        public Tile GetTileAt(int x, int y)
        {
            foreach (var child in Children)
            {
                if (child.Position == new Vector2(x * Tile.SIZE, y * Tile.SIZE))
                    return child;
            }

            return null;
        }

        public Tile GetTileAtPixel(Vector2 pixelPosition)
        {
            foreach(var child in Children)
            {
                var tilePosition = child.Position;

                if (pixelPosition.X >= tilePosition.X && pixelPosition.X <= tilePosition.X + Tile.SIZE - 1)
                {
                    if (pixelPosition.Y >= tilePosition.Y && pixelPosition.Y <= tilePosition.Y + Tile.SIZE - 1)
                        return child;
                }
            }

            return null;
        }

        public Vector2 GetPlayerSpawnPosition() => room.GetPlayerSpawnPosition();

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
