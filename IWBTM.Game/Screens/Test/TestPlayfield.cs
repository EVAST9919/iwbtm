﻿using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Play.Player;
using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.Screens.Test
{
    public class TestPlayfield : DefaultPlayfield
    {
        public readonly Bindable<bool> ShowDeath = new Bindable<bool>();

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
        }

        private void onDeath(Vector2 position)
        {
            if (ShowDeath.Value)
            {
                trailLayer.Add(new Box
                {
                    Position = position,
                    Size = DefaultPlayer.SIZE,
                    Origin = Anchor.Centre,
                    Colour = Color4.Red,
                    Alpha = 0.5f
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