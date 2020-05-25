using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace IWBTM.Game.Overlays
{
    public class NotificationOverlay : CompositeDrawable
    {
        private readonly FillFlowContainer<Notification> flow;

        private int runningDepth;

        public NotificationOverlay()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            AddInternal(flow = new FillFlowContainer<Notification>
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                AutoSizeDuration = 200,
                AutoSizeEasing = Easing.OutQuint,
                Direction = FillDirection.Vertical
            });
        }

        public void Push(string text, NotificationState state)
        {
            runningDepth--;
            var notification = new Notification(text, state);
            flow.Insert(runningDepth, notification);
            notification.Delay(1500).FadeOut(500, Easing.OutQuint).Expire();
        }

        private class Notification : CompositeDrawable
        {
            public Notification(string text, NotificationState state)
            {
                RelativeSizeAxes = Axes.X;
                Height = 30;
                Y = -30;
                Anchor = Anchor.TopCentre;
                Origin = Anchor.TopCentre;
                AddRangeInternal(new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = state == NotificationState.Bad ? Color4.Red : Color4.Green
                    },
                    new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = text
                    }
                });
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                this.MoveToY(0, 200, Easing.OutQuint);
            }
        }
    }

    public enum NotificationState
    {
        Good,
        Bad
    }
}
