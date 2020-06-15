using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Allocation;
using System;
using osuTK;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using IWBTM.Game.UserInterface;
using osu.Framework.Graphics.Shapes;

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
        private readonly Container selectContainer;

        public DrawableTile(Tile tile, string skin, bool allowEdit)
        {
            Tile = tile;
            Skin = skin;

            Size = GetSize(tile.Type);
            Position = new Vector2(tile.PositionX, tile.PositionY);
            AddInternal(MainSprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both
            });

            if (allowEdit)
            {
                AddInternal(selectContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    BorderColour = IWannaColour.Blue,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        AlwaysPresent = true
                    }
                });
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            MainSprite.Texture = getTexture();
        }

        public void Select() => selectContainer.BorderThickness = 5;
        public void Deselect() => selectContainer.BorderThickness = 0;

        private Texture getTexture()
        {
            switch (Tile.Type)
            {
                case TileType.PlatformCorner:
                    return PixelTextures.Get($"Tiles/{Skin}/block") ?? PixelTextures.Get($"Tiles/Default/block");

                case TileType.Miniblock:
                    return PixelTextures.Get($"Tiles/{Skin}/miniblock") ?? PixelTextures.Get($"Tiles/{Skin}/block") ?? PixelTextures.Get($"Tiles/Default/miniblock");

                case TileType.PlatformMiddle:
                    return PixelTextures.Get($"Tiles/{Skin}/platform-middle") ?? PixelTextures.Get($"Tiles/{Skin}/block") ?? PixelTextures.Get("Tiles/Default/platform-middle");

                case TileType.PlatformMiddleRotated:
                    return PixelTextures.Get($"Tiles/{Skin}/platform-middle-rotated") ?? PixelTextures.Get($"Tiles/{Skin}/block") ?? PixelTextures.Get("Tiles/Default/platform-middle-rotated");

                case TileType.PlayerStart:
                    return Textures.Get($"Tiles/{Skin}/playerstart") ?? Textures.Get("Tiles/Default/playerstart");

                case TileType.SmallSpikeBottom:
                    return Textures.Get($"Tiles/{Skin}/minidown") ?? Textures.Get($"Tiles/{Skin}/spikedown") ?? Textures.Get("Tiles/Default/spikedown");

                case TileType.SpikeBottom:
                    return Textures.Get($"Tiles/{Skin}/spikedown") ?? Textures.Get("Tiles/Default/spikedown");

                case TileType.SmallSpikeTop:
                    return Textures.Get($"Tiles/{Skin}/miniup") ?? Textures.Get($"Tiles/{Skin}/spikeup") ?? Textures.Get("Tiles/Default/spikeup");

                case TileType.SpikeTop:
                    return Textures.Get($"Tiles/{Skin}/spikeup") ?? Textures.Get("Tiles/Default/spikeup");

                case TileType.SmallSpikeLeft:
                    return Textures.Get($"Tiles/{Skin}/minileft") ?? Textures.Get($"Tiles/{Skin}/spikeleft") ?? Textures.Get("Tiles/Default/spikeleft");

                case TileType.SpikeLeft:
                    return Textures.Get($"Tiles/{Skin}/spikeleft") ?? Textures.Get("Tiles/Default/spikeleft");

                case TileType.SmallSpikeRight:
                    return Textures.Get($"Tiles/{Skin}/miniright") ?? Textures.Get($"Tiles/{Skin}/spikeright") ?? Textures.Get("Tiles/Default/spikeright");

                case TileType.SpikeRight:
                    return Textures.Get($"Tiles/{Skin}/spikeright") ?? Textures.Get("Tiles/Default/spikeright");

                case TileType.Save:
                    return Textures.Get($"Tiles/{Skin}/save") ?? Textures.Get("Tiles/Default/save");

                case TileType.Warp:
                    return Textures.Get($"Tiles/{Skin}/warp") ?? Textures.Get("Tiles/Default/warp");

                case TileType.Cherry:
                    return Textures.Get($"Tiles/{Skin}/cherry-1") ?? Textures.Get("Tiles/Default/cherry-1");

                case TileType.KillerBlock:
                    return Textures.Get($"Tiles/{Skin}/killerblock") ?? Textures.Get("Tiles/Default/killerblock");

                case TileType.BulletBlocker:
                    return Textures.Get($"Tiles/{Skin}/bulletblocker") ?? Textures.Get("Tiles/Default/bulletblocker");

                case TileType.Jumprefresher:
                    return Textures.Get($"Tiles/{Skin}/jumprefresher") ?? Textures.Get("Tiles/Default/jumprefresher");

                case TileType.Water:
                    return PixelTextures.Get($"Tiles/{Skin}/water3") ?? PixelTextures.Get("Tiles/Default/water3");
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
                case TileType.Miniblock:
                    return new Vector2(SIZE / 2);

                case TileType.Cherry:
                    return new Vector2(21, 22);

                case TileType.Jumprefresher:
                    return new Vector2(13);

                default:
                    return new Vector2(SIZE);
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
            switch (type)
            {
                case TileType.PlatformCorner:
                case TileType.PlatformMiddle:
                case TileType.PlatformMiddleRotated:
                case TileType.Miniblock:
                    return TileGroup.Solid;

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
        Cherry,
        KillerBlock,
        BulletBlocker,
        Miniblock,
        Jumprefresher,
        Water
    }

    public enum TileGroup
    {
        Solid,
        Spike,
        Ungrouped
    }
}
