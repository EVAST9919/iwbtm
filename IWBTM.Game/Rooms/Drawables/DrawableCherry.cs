using osu.Framework.Graphics;
using osuTK;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableCherry : DrawableTile
    {
        private readonly bool animated;

        public DrawableCherry(Tile tile, string skin, bool animated)
            : base(tile, skin)
        {
            this.animated = animated;
            MainSprite.RelativeSizeAxes = Axes.None;
            MainSprite.Size = new Vector2(21, 24);
            MainSprite.Anchor = Anchor.BottomCentre;
            MainSprite.Origin = Anchor.BottomCentre;
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
            MainSprite.Texture = Textures.Get($"Tiles/{Skin}/cherry-{(secondFrame ? 2 : 1)}") ?? Textures.Get($"Tiles/Default/cherry-{(secondFrame ? 2 : 1)}");
        }
    }
}
