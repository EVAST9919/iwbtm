using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableCherry : DrawableTile
    {
        private readonly Sprite overlay;
        private readonly Sprite branch;

        private readonly bool animated;

        public DrawableCherry(Tile tile, bool animated)
            : base(tile)
        {
            this.animated = animated;

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
                    Position = new Vector2(0, -1)
                }
            });

            MainSprite.Colour = Color4.Red;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            overlay.Texture = PixelTextures.Get("Objects/Cherry/cherry-1-overlay");
            branch.Texture = PixelTextures.Get("Objects/Cherry/cherry-1-branch");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (animated)
                Scheduler.AddDelayed(updateFrame, 400, true);
        }

        private bool secondFrame;

        private void updateFrame()
        {
            secondFrame = !secondFrame;
            MainSprite.Texture = PixelTextures.Get($"Objects/Cherry/cherry-{(secondFrame ? 2 : 1)}");
            branch.Y = secondFrame ? -3 : -1;
            branch.Texture = PixelTextures.Get($"Objects/Cherry/cherry-{(secondFrame ? 2 : 1)}-branch");
            overlay.Texture = PixelTextures.Get($"Objects/Cherry/cherry-{(secondFrame ? 2 : 1)}-overlay");
        }
    }
}
