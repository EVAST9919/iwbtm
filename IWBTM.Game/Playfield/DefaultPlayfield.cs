using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Shapes;

namespace IWBTM.Game.Playfield
{
    public class DefaultPlayfield : CompositeDrawable
    {
        private readonly int height;
        private readonly int width;

        public DefaultPlayfield(int width, int height)
        {
            this.width = width;
            this.height = height;

            Width = width * Tile.SIZE;
            Height = height * Tile.SIZE;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                new Tile(TileType.PlatformMiddle)
                {
                    X = Tile.SIZE,
                    Width = (width - 2) * Tile.SIZE,
                    Height = Tile.SIZE,
                },
                new Tile(TileType.PlatformMiddle)
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    X = Tile.SIZE,
                    Width = (width - 2) * Tile.SIZE,
                    Height = Tile.SIZE,
                },
                new Tile(TileType.PlatformMiddleRotated)
                {
                    Y = Tile.SIZE,
                    Height = (height - 2) * Tile.SIZE,
                    Width = Tile.SIZE
                },
                new Tile(TileType.PlatformMiddleRotated)
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Y = Tile.SIZE,
                    Height = (height - 2) * Tile.SIZE,
                    Width = Tile.SIZE
                },
                new BasicTile(TileType.PlatformCorner),
                new BasicTile(TileType.PlatformCorner)
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight
                },
                new BasicTile(TileType.PlatformCorner)
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft
                },
                new BasicTile(TileType.PlatformCorner)
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight
                }
            });
        }
    }
}
