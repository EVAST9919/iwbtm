using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Allocation;
using System;
using osuTK;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableTile : CompositeDrawable
    {
        public const int SIZE = 32;

        [Resolved]
        private PixelTextureStore pixelTextures { get; set; }

        [Resolved]
        protected TextureStore Textures { get; private set; }

        public Tile Tile { get; private set; }

        protected readonly Sprite MainSprite;

        public DrawableTile(Tile tile)
        {
            Tile = tile;
            Size = new Vector2(getSize(tile.Type));
            Position = new Vector2(tile.PositionX, tile.PositionY);
            AddInternal(MainSprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            MainSprite.Texture = getTexture();
        }

        public Tile ToTile() => new Tile
        {
            PositionX = (int)X,
            PositionY = (int)Y,
            Type = Tile.Type
        };

        private Texture getTexture()
        {
            switch (Tile.Type)
            {
                case TileType.PlatformCorner:
                    return pixelTextures.Get("Tiles/platform-corner");

                case TileType.PlatformMiddle:
                    return pixelTextures.Get("Tiles/platform-middle");

                case TileType.PlatformMiddleRotated:
                    return pixelTextures.Get("Tiles/platform-middle-rotated");

                case TileType.PlayerStart:
                    return Textures.Get("Tiles/player-start");

                case TileType.SmallSpikeBottom:
                case TileType.SpikeBottom:
                    return Textures.Get("Tiles/spike-bottom");

                case TileType.SmallSpikeTop:
                case TileType.SpikeTop:
                    return Textures.Get("Tiles/spike-top");

                case TileType.SmallSpikeLeft:
                case TileType.SpikeLeft:
                    return Textures.Get("Tiles/spike-left");

                case TileType.SmallSpikeRight:
                case TileType.SpikeRight:
                    return Textures.Get("Tiles/spike-right");

                case TileType.Save:
                    return Textures.Get("Tiles/save");

                case TileType.Warp:
                    return Textures.Get("Tiles/warp");

                case TileType.Cherry:
                    return Textures.Get("Tiles/cherry");
            }

            throw new NotImplementedException("Tile is not implemented");
        }

        private int getSize(TileType type)
        {
            switch (type)
            {
                case TileType.SmallSpikeBottom:
                case TileType.SmallSpikeTop:
                case TileType.SmallSpikeLeft:
                case TileType.SmallSpikeRight:
                case TileType.Cherry:
                    return SIZE / 2;

                default:
                    return SIZE;
            }
        }

        public static bool IsGroup(DrawableTile tile, TileGroup group)
        {
            var type = tile?.Tile.Type ?? null;

            if (getGroup(type) == group)
                return true;

            return false;
        }

        private static TileGroup getGroup(TileType? type)
        {
            switch(type)
            {
                case TileType.Save:
                    return TileGroup.Save;

                case TileType.PlayerStart:
                    return TileGroup.Start;

                case TileType.Warp:
                    return TileGroup.Warp;

                case TileType.PlatformCorner:
                case TileType.PlatformMiddle:
                case TileType.PlatformMiddleRotated:
                    return TileGroup.Solid;

                case TileType.Cherry:
                    return TileGroup.Cherry;

                case TileType.SmallSpikeBottom:
                case TileType.SmallSpikeLeft:
                case TileType.SmallSpikeRight:
                case TileType.SmallSpikeTop:
                case TileType.SpikeBottom:
                case TileType.SpikeLeft:
                case TileType.SpikeRight:
                case TileType.SpikeTop:
                    return TileGroup.Spike;
            }

            return TileGroup.Ungrouped;
        }
    }

    public enum TileType
    {
        PlatformCorner,
        PlatformMiddle,
        PlatformMiddleRotated,
        PlayerStart,
        SpikeTop,
        SpikeBottom,
        SpikeLeft,
        SpikeRight,
        Save,
        SmallSpikeTop,
        SmallSpikeBottom,
        SmallSpikeLeft,
        SmallSpikeRight,
        Warp,
        Cherry
    }

    public enum TileGroup
    {
        Solid,
        Spike,
        Warp,
        Save,
        Start,
        Cherry,
        Ungrouped
    }
}
