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
                content = new Container<DrawableTile>
                {
                    RelativeSizeAxes = Axes.Both
                }
            });

            foreach (var t in Room.Tiles)
                AddTile(t);
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            bg.Texture = textures.Get($"Tiles/{Room.Skin}/bg") ?? textures.Get("Tiles/Default/bg");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            RestartAnimations();
        }

        public void RestartAnimations()
        {
            foreach (var tile in Tiles)
            {
                if (executeActions)
                {
                    var action = tile.Tile.Action;

                    if (action != null)
                    {
                        tile.ClearTransforms();
                        tile.Position = new Vector2(tile.Tile.PositionX, tile.Tile.PositionY);
                        tile.MoveTo(new Vector2(action.EndX, action.EndY), action.Time).Then().MoveTo(new Vector2(tile.Tile.PositionX, tile.Tile.PositionY), action.Time).Loop();
                    }
                }

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

        protected void AddTile(Tile t)
        {
            switch (t.Type)
            {
                case TileType.Water3:
                    content.Add(new DrawableTile(t, Room.Skin, allowEdit)
                    {
                        Alpha = showWater ? 1 : 0
                    });
                    return;

                case TileType.Jumprefresher:
                    content.Add(new DrawableJumpRefresher(t, Room.Skin, allowEdit));
                    return;

                case TileType.Save:
                    content.Add(new SaveTile(t, Room.Skin, allowEdit));
                    return;

                case TileType.BulletBlocker:
                    content.Add(new DrawableBulletBlocker(t, Room.Skin, showBulletBlocker, allowEdit));
                    return;

                case TileType.Cherry:
                    content.Add(new DrawableCherry(t, Room.Skin, animatedCherry, allowEdit));
                    return;

                case TileType.PlayerStart:
                    PlayerSpawnPosition = new Vector2(t.PositionX, t.PositionY);

                    if (showPlayerSpawn)
                        content.Add(new DrawableTile(t, Room.Skin, allowEdit));

                    return;

                default:
                    content.Add(new DrawableTile(t, Room.Skin, allowEdit));
                    return;
            }
        }
    }
}
