using IWBTM.Game.Playfield;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;
using System.Linq;

namespace IWBTM.Game.Screens.Edit
{
    public class ObjectsLayer : Container<Tile>
    {
        public ObjectsLayer()
        {
            Size = DefaultPlayfield.BASE_SIZE;
        }

        public void TryPlace(TileType tile, Vector2 position)
        {
            var snappedPosition = BluePrint.GetSnappedPosition(position);

            if (!this.Any())
            {
                addTile(tile, snappedPosition);
                return;
            }

            Tile placed = null;

            foreach (Tile child in Children)
            {
                if (child.Position == snappedPosition)
                {
                    placed = child;
                    break;
                }
            }

            if (placed == null)
            {
                addTile(tile, snappedPosition);
                return;
            }

            if (placed.Type == tile)
                return;

            placed.Expire();
            addTile(tile, snappedPosition);
        }

        public void TryRemove(Vector2 position)
        {
            var snappedPosition = BluePrint.GetSnappedPosition(position);

            foreach (Tile child in Children)
            {
                if (child.Position == snappedPosition)
                {
                    child.Expire();
                    break;
                }
            }
        }

        private void addTile(TileType tile, Vector2 position)
        {
            Add(new Tile(tile)
            {
                Position = position
            });
        }

        public string CreateLayout()
        {
            string s = string.Empty;

            for (int j = 0; j < DefaultPlayfield.TILES_HEIGHT; j++)
            {
                for (int i = 0; i < DefaultPlayfield.TILES_WIDTH; i++)
                {
                    Tile tile = null;

                    foreach (Tile child in Children)
                    {
                        if (child.Position == new Vector2(i * Tile.SIZE, j * Tile.SIZE))
                        {
                            tile = child;
                            break;
                        }
                    }

                    s += getChar(tile?.Type);
                }
            }

            return s;
        }

        private static char getChar(TileType? type)
        {
            switch (type)
            {
                case TileType.PlatformCorner:
                    return '+';

                case TileType.PlatformMiddle:
                    return 'X';

                case TileType.PlatformMiddleRotated:
                    return '-';

                case null:
                    return ' ';
            }

            throw new NotImplementedException("Conversion failed");
        }
    }
}
