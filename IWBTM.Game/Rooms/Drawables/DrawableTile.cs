using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Allocation;
using System;
using osuTK;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using IWBTM.Game.UserInterface;
using IWBTM.Game.Helpers;

namespace IWBTM.Game.Rooms.Drawables
{
    public partial class DrawableTile : CompositeDrawable
    {
        public const int SIZE = 32;

        [Resolved]
        protected PixelTextureStore PixelTextures { get; private set; }

        [Resolved]
        protected TextureStore Textures { get; private set; }

        public Tile Tile { get; private set; }

        public readonly Sprite MainSprite;
        protected readonly string Skin;

        public DrawableTile(Tile tile, string skin, bool allowEdit)
        {
            Tile = tile;
            Skin = skin;

            Size = GetSize(tile.Type);
            Position = new Vector2(tile.PositionX, tile.PositionY);
            Masking = allowEdit;
            BorderColour = IWannaColour.Blue;
            AddInternal(MainSprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            MainSprite.Texture = getTexture();
        }

        public void RestartAnimation()
        {
            var action = Tile.Action;

            if (action != null)
            {
                Position = new Vector2(Tile.PositionX, Tile.PositionY);
                origin = new Vector2(action.EndX, action.EndY);

                switch (action.Type)
                {
                    case TileActionType.Movement:
                        ClearTransforms();
                        this.MoveTo(origin, action.Time).Then().MoveTo(new Vector2(Tile.PositionX, Tile.PositionY), action.Time).Loop();
                        break;

                    case TileActionType.Rotation:
                        positionOffset = Vector2.Divide(GetSize(Tile.Type), new Vector2(2));
                        var initialPosition = new Vector2(Tile.PositionX, Tile.PositionY);
                        movingAngle = MathExtensions.GetAngle(initialPosition + positionOffset, origin);
                        distance = MathExtensions.Distance(initialPosition + positionOffset, origin);
                        playAnimation = true;
                        break;
                }
            }
        }

        private bool playAnimation;

        private Vector2 origin;
        private Vector2 positionOffset;
        private float distance;
        private float movingAngle;

        protected override void Update()
        {
            base.Update();

            if (!playAnimation)
                return;

            movingAngle += (float)(Clock.ElapsedFrameTime / Tile.Action.Time * 360f);
            if (movingAngle >= 360)
                movingAngle %= 360;

            Position = MathExtensions.GetRotatedPosition(origin, distance, movingAngle) - positionOffset;
        }

        public void Select() => BorderThickness = 5;
        public void Deselect() => BorderThickness = 0;

        private Texture getTexture()
        {
            switch (Tile.Type)
            {
                case TileType.PlatformCorner:
                    return getTexture("block", Skin, true) ?? getTexture("block", "Default", true);

                case TileType.Miniblock:
                    return getTexture("miniblock", Skin, true) ?? getTexture("block", Skin, true) ?? getTexture("miniblock", "Default", true);

                case TileType.PlatformMiddle:
                    return getTexture("platform-middle", Skin, true) ?? getTexture("block", Skin, true) ?? getTexture("platform-middle", "Default", true);

                case TileType.PlatformMiddleRotated:
                    return getTexture("platform-middle-rotated", Skin, true) ?? getTexture("block", Skin, true) ?? getTexture("platform-middle-rotated", "Default", true);

                case TileType.PlayerStart:
                    return getTexture("playerstart", Skin, false) ?? getTexture("playerstart", "Default", false);

                case TileType.SmallSpikeBottom:
                    return getTexture("minidown", Skin, false) ?? getTexture("spikedown", Skin, false) ?? getTexture("spikedown", "Default", false);

                case TileType.SpikeBottom:
                    return getTexture("spikedown", Skin, false) ?? getTexture("spikedown", "Default", false);

                case TileType.SmallSpikeTop:
                    return getTexture("miniup", Skin, false) ?? getTexture("spikeup", Skin, false) ?? getTexture("spikeup", "Default", false);

                case TileType.SpikeTop:
                    return getTexture("spikeup", Skin, false) ?? getTexture("spikeup", "Default", false);

                case TileType.SmallSpikeLeft:
                    return getTexture("minileft", Skin, false) ?? getTexture("spikeleft", Skin, false) ?? getTexture("spikeleft", "Default", false);

                case TileType.SpikeLeft:
                    return getTexture("spikeleft", Skin, false) ?? getTexture("spikeleft", "Default", false);

                case TileType.SmallSpikeRight:
                    return getTexture("miniright", Skin, false) ?? getTexture("spikeright", Skin, false) ?? getTexture("spikeright", "Default", false);

                case TileType.SpikeRight:
                    return getTexture("spikeright", Skin, false) ?? getTexture("spikeright", "Default", false);

                case TileType.Save:
                    return getTexture("save", Skin, false) ?? getTexture("save", "Default", false);

                case TileType.Warp:
                    return getTexture("warp", Skin, false) ?? getTexture("warp", "Default", false);

                case TileType.Cherry:
                    return getTexture("cherry-1", Skin, true) ?? getTexture("cherry-1", "Default", true);

                case TileType.KillerBlock:
                    return getTexture("killerblock", Skin, true) ?? getTexture("killerblock", "Default", true);

                case TileType.BulletBlocker:
                    return getTexture("bulletblocker", Skin, true) ?? getTexture("bulletblocker", "Default", true);

                case TileType.Jumprefresher:
                    return getTexture("jumprefresher", Skin, true) ?? getTexture("jumprefresher", "Default", true);

                case TileType.Water3:
                    return getTexture("water3", Skin, false) ?? getTexture("water3", "Default", false);
            }

            throw new NotImplementedException("Tile is not implemented");
        }

        private Texture getTexture(string name, string skin, bool pixelated)
        {
            if (pixelated)
                return PixelTextures.Get($"Tiles/{skin}/{name}", WrapMode.ClampToEdge, WrapMode.ClampToEdge);

            return Textures.Get($"Tiles/{skin}/{name}", WrapMode.ClampToEdge, WrapMode.ClampToEdge);
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
        Water3
    }

    public enum TileGroup
    {
        Solid,
        Spike,
        Ungrouped
    }
}
