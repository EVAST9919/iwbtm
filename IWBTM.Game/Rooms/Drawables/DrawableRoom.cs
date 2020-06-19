using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableRoom : CompositeDrawable
    {
        public Vector2 PlayerSpawnPosition { get; private set; }

        public List<DrawableTile> Tiles => content.Children.ToList();

        public Room Room { get; set; }

        private readonly bool showPlayerSpawn;
        private readonly bool showBulletBlocker;
        private readonly bool animatedCherry;
        private readonly bool executeActions;
        private readonly bool allowEdit;
        private readonly bool showWater;

        private readonly Sprite bg;
        private readonly Container<DrawableTile> content;

        public DrawableRoom(Room room, bool showPlayerSpawn, bool showBulletBlocker, bool animatedCherry, bool executeActions, bool allowEdit, bool showWater)
        {
            Room = room;
            this.showPlayerSpawn = showPlayerSpawn;
            this.showBulletBlocker = showBulletBlocker;
            this.animatedCherry = animatedCherry;
            this.executeActions = executeActions;
            this.allowEdit = allowEdit;
            this.showWater = showWater;

            Size = new Vector2(Room.SizeX, Room.SizeY) * DrawableTile.SIZE;
            Masking = true;
            AddRangeInternal(new Drawable[]
            {
                bg = new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    FillMode = FillMode.Fill
                },
                CreateDrawableBelowContent(),
                content = new Container<DrawableTile>
                {
                    RelativeSizeAxes = Axes.Both
                }
            });

            foreach (var t in Room.Tiles)
                AddTile(t, false);
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            bg.Texture = textures.Get($"Tiles/{Room.Skin}/bg") ?? textures.Get("Tiles/Default/bg");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Restart();
        }

        protected virtual Drawable CreateDrawableBelowContent() => Empty();

        public void Restart()
        {
            foreach (var tile in Tiles)
            {
                if (executeActions)
                    tile.RestartAnimation();

                if (tile is DrawableJumpRefresher)
                    ((DrawableJumpRefresher)tile).Activate();
            }
        }

        public bool HasTileOfGroupAt(Vector2 pixelPosition, TileGroup group) => GetTileOfGroupAt(pixelPosition, group) != null;

        public bool HasTileAtPixel(Vector2 pixelPosition, TileType type) => GetTileAtPixel(pixelPosition, type) != null;

        public bool HasTileAt(Vector2 position, TileType type) => GetTileAt(position, type) != null;

        public bool HasAnyTileAt(Vector2 pixelPosition) => GetAnyTileAt(pixelPosition) != null;

        public bool HasTile(TileType type) => GetTile(type) != null;

        public DrawableTile GetTile(TileType type)
        {
            foreach (var child in content.Children)
            {
                if (child.Tile.Type == type)
                    return child;
            }

            return null;
        }

        public List<DrawableTile> GetAllTiles(TileType type)
        {
            var tiles = new List<DrawableTile>();

            foreach (var child in content.Children)
            {
                if (child.Tile.Type == type)
                    tiles.Add(child);
            }

            return tiles;
        }

        public DrawableTile GetTileOfGroupAt(Vector2 pixelPosition, TileGroup group)
        {
            foreach (var child in content.Children)
            {
                var tilePosition = child.Position;
                var tileSize = child.Size;

                if (pixelPosition.X >= tilePosition.X && pixelPosition.X < tilePosition.X + tileSize.X)
                {
                    if (pixelPosition.Y >= tilePosition.Y && pixelPosition.Y < tilePosition.Y + tileSize.Y)
                    {
                        if (DrawableTile.IsGroup(child, group))
                            return child;
                    }
                }
            }

            return null;
        }

        public DrawableTile GetSolidTileForVerticalCheck(Vector2 pixelPosition)
        {
            foreach (var child in content.Children)
            {
                var tilePosition = child.Position;
                var tileSize = child.Size;

                if (pixelPosition.X > tilePosition.X && pixelPosition.X < tilePosition.X + tileSize.X)
                {
                    if (pixelPosition.Y >= tilePosition.Y && pixelPosition.Y < tilePosition.Y + tileSize.Y)
                    {
                        if (DrawableTile.IsGroup(child, TileGroup.Solid))
                            return child;
                    }
                }
            }

            return null;
        }

        public DrawableTile GetSolidTileForHorizontalCheck(Vector2 pixelPosition)
        {
            foreach (var child in content.Children)
            {
                var tilePosition = child.Position;
                var tileSize = child.Size;

                if (pixelPosition.X >= tilePosition.X && pixelPosition.X < tilePosition.X + tileSize.X)
                {
                    if (pixelPosition.Y > tilePosition.Y && pixelPosition.Y < tilePosition.Y + tileSize.Y)
                    {
                        if (DrawableTile.IsGroup(child, TileGroup.Solid))
                            return child;
                    }
                }
            }

            return null;
        }

        public DrawableTile GetTileAtPixel(Vector2 pixelPosition, TileType type)
        {
            foreach (var child in content.Children)
            {
                var tilePosition = child.Position;
                var tileSize = child.Size;

                if (pixelPosition.X >= tilePosition.X && pixelPosition.X < tilePosition.X + tileSize.X)
                {
                    if (pixelPosition.Y >= tilePosition.Y && pixelPosition.Y < tilePosition.Y + tileSize.Y)
                    {
                        if (child.Tile.Type == type)
                            return child;
                    }
                }
            }

            return null;
        }

        public DrawableTile GetTileAt(Vector2 position, TileType type)
        {
            foreach (var child in content.Children)
            {
                if (child.Position == position && child.Tile.Type == type)
                    return child;
            }

            return null;
        }

        public DrawableTile GetAnyTileAt(Vector2 pixelPosition)
        {
            foreach (var child in content.Children)
            {
                var tilePosition = child.Position;
                var tileSize = child.Size;

                if (pixelPosition.X >= tilePosition.X && pixelPosition.X < tilePosition.X + tileSize.X)
                {
                    if (pixelPosition.Y >= tilePosition.Y && pixelPosition.Y < tilePosition.Y + tileSize.Y)
                        return child;
                }
            }

            return null;
        }

        protected void AddTile(Tile t, bool animated)
        {
            DrawableTile drawable = null;

            switch (t.Type)
            {
                case TileType.Water3:
                    drawable = new DrawableTile(t, Room.Skin, allowEdit)
                    {
                        Alpha = showWater ? 1 : 0
                    };
                    break;

                case TileType.Jumprefresher:
                    drawable = new DrawableJumpRefresher(t, Room.Skin, allowEdit);
                    break;

                case TileType.Save:
                    drawable = new SaveTile(t, Room.Skin, allowEdit);
                    break;

                case TileType.BulletBlocker:
                    drawable = new DrawableBulletBlocker(t, Room.Skin, showBulletBlocker, allowEdit);
                    break;

                case TileType.Cherry:
                    drawable = new DrawableCherry(t, Room.Skin, animatedCherry, allowEdit);
                    break;

                case TileType.PlayerStart:
                    PlayerSpawnPosition = new Vector2(t.PositionX, t.PositionY);

                    if (showPlayerSpawn)
                        drawable = new DrawableTile(t, Room.Skin, allowEdit);

                    break;

                default:
                    drawable = new DrawableTile(t, Room.Skin, allowEdit);
                    break;
            }

            if (drawable == null)
                return;

            content.Add(drawable);

            if (!animated)
                return;

            drawable.MainSprite.Scale = Vector2.Zero;
            drawable.MainSprite.ScaleTo(1, 300, Easing.Out);
        }
    }
}
