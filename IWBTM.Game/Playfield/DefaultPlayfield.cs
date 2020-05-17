﻿using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Shapes;
using IWBTM.Game.Player;
using osu.Framework.Audio.Track;
using osu.Framework.Audio;
using osu.Framework.Input.Events;
using osuTK.Input;
using osuTK;
using osu.Framework.Bindables;
using IWBTM.Game.Rooms;

namespace IWBTM.Game.Playfield
{
    public class DefaultPlayfield : CompositeDrawable
    {
        public static readonly Vector2 BASE_SIZE = new Vector2(768, 608);
        public static readonly int TILES_WIDTH = 24;
        public static readonly int TILES_HEIGHT = 19;

        public BindableBool ShowHitbox => player.ShowHitbox;

        private readonly DefaultPlayer player;
        private Track track;

        public DefaultPlayfield()
        {
            Size = BASE_SIZE;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            //Scale = new Vector2(1.5f);

            var room = new BossRoom();

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                new DrawableRoom(room)
            });

            AddInternal(player = new DefaultPlayer(room));
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            track = audio.Tracks.Get("Ghost Rule");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            restart();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch(e.Key)
                {
                    case Key.R:
                        restart();
                        return true;
                }
            }

            return base.OnKeyDown(e);
        }

        private void restart()
        {
            //track.Restart();
            player.SetDefaultPosition();
        }
    }
}
