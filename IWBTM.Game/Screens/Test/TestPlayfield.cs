using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.UserInterface;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace IWBTM.Game.Screens.Test
{
    public class TestPlayfield : DefaultPlayfield
    {
        public readonly Bindable<bool> ShowDeath = new Bindable<bool>();
        public readonly Bindable<bool> ShowHitbox = new Bindable<bool>();
        public readonly Bindable<bool> GodMode = new Bindable<bool>();

        public TestPlayfield(Room room)
            : base(room)
        {
        }

        private Container trailLayer;

        protected override Drawable CreateLayerBehindPlayer() => trailLayer = new Container
        {
            RelativeSizeAxes = Axes.Both
        };

        protected override void LoadComplete()
        {
            base.LoadComplete();

            ShowDeath.BindValueChanged(onTrailChanged, true);
            Player.Died += (position, _) => onDeath(position);
            Player.ShowHitbox.BindTo(ShowHitbox);
            Player.God.BindTo(GodMode);
            ShowHitbox.TriggerChange();
            GodMode.TriggerChange();
        }

        private void onDeath(Vector2 position)
        {
            if (ShowDeath.Value)
            {
                trailLayer.Add(new DeathSpot
                {
                    Position = position
                });
            }
        }

        private void onTrailChanged(ValueChangedEvent<bool> trail)
        {
            if (!trail.NewValue)
            {
                trailLayer.Clear();
                return;
            }
        }
    }
}
