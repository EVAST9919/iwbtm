using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableRoom : Container<DrawableTile>
    {
        public Room Room { get; private set; }

        private readonly bool showPlayerSpawn;

        public DrawableRoom(Room room, bool showPlayerSpawn = false)
        {
            Room = room;
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
                    var tile = Room.GetTileAt(i, j);

                    if (!Room.TileIsEmpty(tile))
                    {
                        var type = GetTileType(tile);
                        var position = new Vector2(i * DrawableTile.SIZE, j * DrawableTile.SIZE);

                        if (type == TileType.Save)
                        {
                            Add(new SaveTile
                            {
                                Position = position
                            });
                        }
                        else
                        {
                            Add(new DrawableTile(type)
                            {
                                Position = position
                            });
                        }
                    }
                }
            }

            if (showPlayerSpawn)
            {
                Add(new DrawableTile(TileType.PlayerStart)
                {
                    Position = Room.PlayerSpawnPosition
                });
            }
        }

        public DrawableTile GetTileAtPixel(Vector2 pixelPosition)
        {
            foreach (var child in Children)
            {
                var tilePosition = child.Position;

                if (pixelPosition.X >= tilePosition.X && pixelPosition.X <= tilePosition.X + DrawableTile.SIZE - 1)
                {
                    if (pixelPosition.Y >= tilePosition.Y && pixelPosition.Y <= tilePosition.Y + DrawableTile.SIZE - 1)
                        return child;
                }
            }

            return null;
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
