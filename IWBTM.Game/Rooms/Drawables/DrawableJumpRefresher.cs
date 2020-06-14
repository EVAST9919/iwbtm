using osu.Framework.Graphics;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableJumpRefresher : DrawableTile
    {
        public bool IsActive { get; set; }

        public DrawableJumpRefresher(Tile t, string skin, bool allowEdit)
            : base(t, skin, allowEdit)
        {
            AlwaysPresent = true;
        }

        public void Deactivate()
        {
            Scheduler.CancelDelayedTasks();
            IsActive = false;
            this.FadeOut();
            Scheduler.AddDelayed(() =>
            {
                IsActive = true;
                this.FadeIn();
            }, 2000);
        }

        public void Activate()
        {
            Scheduler.CancelDelayedTasks();
            IsActive = true;
            this.FadeIn();
        }
    }
}
