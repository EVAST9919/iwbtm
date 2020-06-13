using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Allocation;
using System;
using osuTK;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using IWBTM.Game.UserInterface;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableTile : CompositeDrawable
    {
        public const int SIZE = 32;

        [Resolved]
        protected PixelTextureStore PixelTextures { get; private set; }

        [Resolved]
        protected TextureStore Textures { get; private set; }

        public Tile Tile { get; private set; }

        protected readonly Sprite MainSprite;
        protected readonly string Skin;

        public DrawableTile(Tile tile, string skin)
        {
            Tile = tile;
            Skin = skin;

            Size = GetSize(tile.Type);
            Position = new Vector2(tile.PositionX, tile.PositionY);
            Masking = true;
            BorderColour = IWannaColour.Blue;
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

        public void Select() => BorderThickness = 5;
        public void Deselect() => BorderThickness = 0;

        private Texture getTexture()
        {
            switch (Tile.Type)
            {
                case TileType.PlatformCorner:
                    return PixelTextures.Get($"Tiles/{Skin}/block") ?? PixelTextures.Get($"Tiles/Default/block");

                case TileType.PlatformMiddle:
                    return PixelTextures.Get($"Tiles/{Skin}/platform-middle") ?? PixelTextures.Get($"Tiles/{Skin}/block") ?? PixelTextures.Get("Tiles/Default/platform-middle");

                case TileType.PlatformMiddleRotated:
                    return PixelTextures.Get($"Tiles/{Skin}/platform-middle-rotated") ?? PixelTextures.Get($"Tiles/{Skin}/block") ?? PixelTextures.Get("Tiles/Default/platform-middle-rotated");

                case TileType.PlayerStart:
                    return Textures.Get($"Tiles/{Skin}/playerstart") ?? Textures.Get("Tiles/Default/playerstart");

                case TileType.SmallSpikeBottom:
                case TileType.SpikeBottom:
                    return Textures.Get($"Tiles/{Skin}/spikedown") ?? Textures.Get("Tiles/Default/spikedown");

                case TileType.SmallSpikeTop:
                case TileType.SpikeTop:
                    return Textures.Get($"Tiles/{Skin}/spikeup") ?? Textures.Get("Tiles/Default/spikeup");

                case TileType.SmallSpikeLeft:
                case TileType.SpikeLeft:
                    return Textures.Get($"Tiles/{Skin}/spikeleft") ?? Textures.Get("Tiles/Default/spikeleft");

                case TileType.SmallSpikeRight:
                case TileType.SpikeRight:
                    return Textures.Get($"Tiles/{Skin}/spikeright") ?? Textures.Get("Tiles/Default/spikeright");

                case TileType.Save:
                    return Textures.Get($"Tiles/{Skin}/save") ?? Textures.Get("Tiles/Default/save");

                case TileType.Warp:
                    return Textures.Get($"Tiles/{Skin}/warp") ?? Textures.Get("Tiles/Default/warp");

                case TileType.Cherry:
                    return PixelTextures.Get("Objects/Cherry/cherry-1");

                case TileType.KillerBlock:
                    return Textures.Get($"Tiles/{Skin}/killerblock") ?? Textures.Get("Tiles/Default/killerblock");

                case TileType.BulletBlocker:
                    return Textures.Get($"Tiles/{Skin}/bulletblocker") ?? Textures.Get("Tiles/Default/bulletblocker");
            }

            throw new NotImplementedException("Tile is not implemented");
        }

        public static Vector2 GetSize(TileType type)
        {
            switch (type)
            {
                case TileType.SmallSpikeBottom:
                case TileType.SmallSpikeTop:
                case TileType.SmallSpikeLeft:
                case TileType.SmallSpikeRight:
                    return new Vector2(SIZE / 2);

                case TileType.Cherry:
                    return new Vector2(21, 22);

                default:
                    return new Vector2(SIZE);
            }
        }

        public static bool IsGroup(DrawableTile tile, TileGroup group)
        {
            var type = tile?.Tile.Type ?? null;

            if (GetGroup(type) == group)
                return true;

            return false;
        }

        public static TileGroup GetGroup(TileType? type)
        {
            switch (type)
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

                case TileType.KillerBlock:
                    return TileGroup.KillerBlock;

                case TileType.BulletBlocker:
                    return TileGroup.BulletBlocker;
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
        Cherry,
        KillerBlock,
        BulletBlocker
    }

    public enum TileGroup
    {
        Solid,
        Spike,
        Warp,
        Save,
        Start,
        Cherry,
        KillerBlock,
        BulletBlocker,
        Ungrouped
    }
}
