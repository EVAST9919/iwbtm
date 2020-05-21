using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using IWBTM.Game.Rooms;
using System;
using IWBTM.Game.Screens.Play.Playfield;

namespace IWBTM.Game.Screens.Play.Player
{
    public class Bullet : CompositeDrawable
    {
        public Action OnSave;

        private const double speed = 16.0;

        private readonly Sprite sprite;

        private readonly bool right;
        private readonly Room room;

        [Resolved]
        private DrawableRoom drawableRoom { get; set; }

        public Bullet(Room room, bool right)
        {
            this.right = right;
            this.room = room;

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

            // check borders
            if (Position.X <= 0 || Position.X >= DefaultPlayfield.BASE_SIZE.X)
            {
                Expire();
                return;
            }

            var xPos = (int)(Position.X / DefaultPlayfield.BASE_SIZE.X * DefaultPlayfield.TILES_WIDTH);
            var yPos = (int)(Position.Y / DefaultPlayfield.BASE_SIZE.Y * DefaultPlayfield.TILES_HEIGHT);

            // check solid tiles
            var tile = room.GetTileAt(xPos, yPos);
            if (Room.TileIsSolid(tile))
            {
                Expire();
                return;
            }

            if (Room.TileIsSave(tile))
            {
                ((SaveTile)drawableRoom.GetTileAt(xPos, yPos)).Activate();
                OnSave?.Invoke();
                Expire();
                return;
            }

            var delta = (right ? 1 : -1) * (float)(speed / 20 * Clock.ElapsedFrameTime);
            X += delta;
        }
    }
}
