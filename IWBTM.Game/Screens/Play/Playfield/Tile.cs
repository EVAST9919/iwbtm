using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Allocation;
using System;
using osuTK;

namespace IWBTM.Game.Screens.Play.Playfield
{
    public class Tile : Sprite
    {
        public const int SIZE = 32;

        [Resolved]
        private PixelTextureStore pixelTextures { get; set; }

        [Resolved]
        private TextureStore textures { get; set; }

        public readonly TileType Type;

        public Tile(TileType type)
        {
            Type = type;
            Size = new Vector2(SIZE);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Texture = getTexture();
        }

        private Texture getTexture()
        {
            switch (Type)
            {
                case TileType.PlatformCorner:
                    return pixelTextures.Get("Tiles/platform-corner");

                case TileType.PlatformMiddle:
                    return pixelTextures.Get("Tiles/platform-middle");

                case TileType.PlatformMiddleRotated:
                    return pixelTextures.Get("Tiles/platform-middle-rotated");

                case TileType.PlayerStart:
                    return textures.Get("Tiles/player-start");

                case TileType.SpikeBottom:
                    return textures.Get("Tiles/spike-bottom");

                case TileType.SpikeTop:
                    return textures.Get("Tiles/spike-top");

                case TileType.SpikeLeft:
                    return textures.Get("Tiles/spike-left");

                case TileType.SpikeRight:
                    return textures.Get("Tiles/spike-right");

                case TileType.Save:
                    return textures.Get("Tiles/save");
            }

            throw new NotImplementedException($"{Type} tile is not implemented");
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
        Save
    }
}
