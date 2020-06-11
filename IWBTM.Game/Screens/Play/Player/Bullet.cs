using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using System;
using IWBTM.Game.Rooms.Drawables;
using System.Linq;

namespace IWBTM.Game.Screens.Play.Player
{
    public class Bullet : CompositeDrawable
    {
        public Action OnSave;

        private const double speed = 16.0;

        private readonly Sprite sprite;

        private readonly bool right;
        private readonly Vector2 roomSize;

        [Resolved]
        private DrawableRoom drawableRoom { get; set; }

        public Bullet(bool right, Vector2 roomSize)
        {
            this.right = right;
            this.roomSize = roomSize;

            Size = new Vector2(3);
            Origin = Anchor.Centre;
            InternalChild = sprite = new Sprite
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.Both
            };
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            sprite.Texture = textures.Get("bullet");
        }

        protected override void Update()
        {
            base.Update();

            var position = new Vector2(Position.X + (1.5f * (right ? 1 : -1)), Position.Y);

            if (position.X >= roomSize.X || position.X <= 0)
            {
                Expire();
                return;
            }

            var tiles = drawableRoom.GetTilesAt(position);

            if (tiles.Any())
            {
                if (DrawableRoom.ContainsTileOfGroup(tiles, TileGroup.Solid))
                {
                    Expire();
                    return;
                }

                var blocker = DrawableRoom.GetTileOfGroup(tiles, TileGroup.BulletBlocker);

                if (blocker != null)
                {
                    ((DrawableBulletBlocker)blocker).Activate();
                    Expire();
                    return;
                }

                var save = DrawableRoom.GetTileOfGroup(tiles, TileGroup.Save);

                if (save != null)
                {
                    ((SaveTile)save).Activate();
                    OnSave?.Invoke();
                    Expire();
                    return;
                }
            }

            var delta = (right ? 1 : -1) * (float)(speed / 20 * Clock.ElapsedFrameTime);
            X += delta;
        }
    }
}
