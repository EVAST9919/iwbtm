using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System.Collections.Generic;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableRoom : Container<DrawableTile>
    {
        public Vector2 PlayerSpawnPosition { get; private set; }

        protected Room Room { get; set; }

        private readonly bool showPlayerSpawn;
        private readonly bool showBulletBlocker;
        private readonly bool animatedCherry;
        private readonly bool executeActions;

        public DrawableRoom(Room room, bool showPlayerSpawn, bool showBulletBlocker, bool animatedCherry, bool executeActions)
        {
            Room = room;
            this.showPlayerSpawn = showPlayerSpawn;
            this.showBulletBlocker = showBulletBlocker;
            this.animatedCherry = animatedCherry;
            this.executeActions = executeActions;

            Size = new Vector2(room.SizeX, room.SizeY) * DrawableTile.SIZE;

            if (room != null)
            {
                foreach (var t in room.Tiles)
                    AddTile(t);
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            RestartAnimations();
        }

        public void RestartAnimations()
        {
            if (executeActions)
            {
                foreach (var child in Children)
                {
                    var action = child.Tile.Action;

                    if (action != null)
                    {
                        child.ClearTransforms();
                        child.Position = new Vector2(child.Tile.PositionX, child.Tile.PositionY);
                        child.MoveTo(new Vector2(action.EndX, action.EndY), action.Time).Then().MoveTo(new Vector2(child.Tile.PositionX, child.Tile.PositionY), action.Time).Loop();
                    }
                }
            }
        }

        public bool HasTileOfGroupAt(Vector2 pixelPosition, TileGroup group) => GetTileOfGroupAt(pixelPosition, group) != null;

        public bool HasTileAtPixel(Vector2 pixelPosition, TileType type) => GetTileAtPixel(pixelPosition, type) != null;

        public bool HasTileAt(Vector2 position, TileType type) => GetTileAt(position, type) != null;

        public bool HasAnyTileAt(Vector2 pixelPosition) => GetAnyTileAt(pixelPosition) != null;

        public bool HasTile(TileType type) => GetTile(type) != null;

        public DrawableTile GetTile(TileType type)
        {
            foreach (var child in Children)
            {
                if (child.Tile.Type == type)
                    return child;
            }

            return null;
        }

        public List<DrawableTile> GetAllTiles(TileType type)
        {
            var tiles = new List<DrawableTile>();

            foreach (var child in Children)
            {
                if (child.Tile.Type == type)
                    tiles.Add(child);
            }

            return tiles;
        }

        public DrawableTile GetTileOfGroupAt(Vector2 pixelPosition, TileGroup group)
        {
            foreach (var child in Children)
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
            foreach (var child in Children)
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
            foreach (var child in Children)
            {
                if (child.Position == position && child.Tile.Type == type)
                    return child;
            }

            return null;
        }

        public DrawableTile GetAnyTileAt(Vector2 pixelPosition)
        {
            foreach (var child in Children)
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
                case TileType.Save:
                    Add(new SaveTile(t));
                    break;

                case TileType.BulletBlocker:
                    Add(new DrawableBulletBlocker(t, showBulletBlocker));
                    break;

                case TileType.Cherry:
                    Add(new DrawableCherry(t, animatedCherry));
                    break;

                case TileType.PlayerStart:
                    PlayerSpawnPosition = new Vector2(t.PositionX, t.PositionY);

                    if (showPlayerSpawn)
                        Add(new DrawableTile(t));

                    break;

                default:
                    Add(new DrawableTile(t));
                    break;
            }
        }
    }
}
