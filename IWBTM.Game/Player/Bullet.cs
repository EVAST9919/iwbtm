﻿using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using IWBTM.Game.Playfield;

namespace IWBTM.Game.Player
{
    public class Bullet : CompositeDrawable
    {
        private const double speed = 16.0;

        private readonly Sprite sprite;

        private readonly bool right;

        public Bullet(bool right)
        {
            this.right = right;

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

            if (Position.X > DefaultPlayfield.BASE_SIZE.X || Position.X < 0)
            {
                Expire();
                return;
            }

            var delta = (right ? 1 : -1) * (float)(speed / 20 * Clock.ElapsedFrameTime);

            X += delta;
        }
    }
}
