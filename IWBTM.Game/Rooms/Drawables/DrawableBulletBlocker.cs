using osu.Framework.Graphics;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableBulletBlocker : DrawableTile
    {
        private readonly bool show;

        public DrawableBulletBlocker(Tile tile, string skin, bool show, bool allowEdit)
            : base(tile, skin, allowEdit)
        {
            this.show = show;

            if (!show)
            {
                Alpha = 0;
                AlwaysPresent = true;
            }
        }

        public void Activate()
        {
            if (!show)
                this.FadeIn().Then().FadeOut(400, Easing.Out);
        }
    }
}
