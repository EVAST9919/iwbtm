using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Allocation;
using System;
using osuTK;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableTile : Sprite
    {
        public const int SIZE = 32;

        [Resolved]
        private PixelTextureStore pixelTextures { get; set; }

        [Resolved]
        protected TextureStore Textures { get; private set; }

        public Tile Tile { get; private set; }

        public DrawableTile(Tile tile)
        {
            Tile = tile;
            Size = new Vector2(getSize(tile.Type));
            Position = new Vector2(tile.PositionX, tile.PositionY);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Texture = getTexture();
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
                    return SIZE / 2;

                default:
                    return SIZE;
            }
        }

        public static bool IsSolid(TileType? type)
        {
            if (type == null)
                return false;

            if (type == TileType.PlatformCorner || type == TileType.PlatformMiddle || type == TileType.PlatformMiddleRotated)
                return true;

            return false;
        }

        public static bool IsSpike(TileType? type)
        {
            if (type == null)
                return false;

            if (type == TileType.SpikeBottom || type == TileType.SpikeTop || type == TileType.SpikeLeft || type == TileType.SpikeRight
                || type == TileType.SmallSpikeBottom || type == TileType.SmallSpikeTop || type == TileType.SmallSpikeLeft || type == TileType.SmallSpikeRight)
                return true;

            return false;
        }

        public static bool IsSave(TileType? type)
        {
            if (type == null)
                return false;

            if (type == TileType.Save)
                return true;

            return false;
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
    }
}
