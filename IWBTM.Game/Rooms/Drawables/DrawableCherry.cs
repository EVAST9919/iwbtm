﻿using IWBTM.Game.Screens.Test;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableCherry : DrawableTile, IHasHitbox
    {
        private readonly Sprite overlay;
        private readonly Sprite branch;
        private readonly Circle hitbox;

        public DrawableCherry(Tile tile)
            : base(tile)
        {
            AddRangeInternal(new Drawable[]
            {
                overlay = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                },
                branch = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Position = new Vector2(1, -1)
                },
                hitbox = new Circle
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.DeepPink,
                    Alpha = 0
                }
            });

            MainSprite.Colour = Color4.Red;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            overlay.Texture = Textures.Get("Tiles/cherry-overlay");
            branch.Texture = Textures.Get("Tiles/cherry-branch-1");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Scheduler.AddDelayed(updateFrame, 200, true);
        }

        private bool secondFrame;

        private void updateFrame()
        {
            secondFrame = !secondFrame;
            MainSprite.Texture = Textures.Get($"Tiles/cherry-{(secondFrame ? 2 : 1)}");
            branch.Texture = Textures.Get($"Tiles/cherry-branch-{(secondFrame ? 2 : 1)}");
        }

        public void Toggle(bool show)
        {
            hitbox.Alpha = show ? 1 : 0;
        }
    }
}
