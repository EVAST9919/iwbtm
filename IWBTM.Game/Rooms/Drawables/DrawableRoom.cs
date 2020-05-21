using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableRoom : Container<DrawableTile>
    {
        public Room Room { get; private set; }

        public Vector2 PlayerSpawnPosition { get; private set; }

        public DrawableRoom(Room room, bool showPlayerSpawn = false)
        {
            Room = room;

            Size = DefaultPlayfield.BASE_SIZE;

            foreach (var t in Room.Tiles)
            {
                if (t.Type == TileType.Save)
                {
                    Add(new SaveTile(t));
                    continue;
                }

                if (t.Type == TileType.PlayerStart)
                {
                    PlayerSpawnPosition = new Vector2(t.PositionX, t.PositionY);

                    if (showPlayerSpawn)
                        Add(new DrawableTile(t));

                    continue;
                }

                Add(new DrawableTile(t));
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
