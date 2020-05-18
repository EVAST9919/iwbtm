using IWBTM.Game.Playfield;
using osu.Framework.Graphics.Containers;
using osuTK;
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
    }
}
