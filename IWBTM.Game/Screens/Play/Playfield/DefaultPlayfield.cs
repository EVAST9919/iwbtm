using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Audio.Track;
using osu.Framework.Audio;
using osu.Framework.Input.Events;
using osuTK.Input;
using osuTK;
using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Play.Player;
using IWBTM.Game.Screens.Play.Death;
using IWBTM.Game.Rooms.Drawables;
using osu.Framework.Graphics.Audio;
using System;
using IWBTM.Game.Helpers;

namespace IWBTM.Game.Screens.Play.Playfield
{
    public class DefaultPlayfield : CompositeDrawable
    {
        public Action Completed;
        public Action<Vector2> OnDeath;
        public Action OnRespawn;

        public DefaultPlayer Player;
        private PlayerParticlesContainer deathOverlay;
        private Track track;
        private DrawableSample roomEntering;

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        private readonly Room room;
        private readonly string name;
        protected DrawableRoom DrawableRoom;

        public DefaultPlayfield(Room room, string name)
        {
            this.room = room;
            this.name = name;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, LevelAudioManager levelAudio)
        {
            DrawableRoom = new DrawableRoom(room);
            dependencies.Cache(DrawableRoom);

            Size = new Vector2(room.SizeX, room.SizeY) * DrawableTile.SIZE;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                DrawableRoom,
                CreateLayerBehindPlayer(),
                Player = new DefaultPlayer
                {
                    Died = onDeath,
                    Completed = onCompletion
                },
                deathOverlay = new PlayerParticlesContainer(),
                roomEntering = new DrawableSample(audio.Samples.Get("room-entering"))
            };

            if (LevelStorage.LevelHasCustomAudio(name))
            {
                track = levelAudio.Tracks.Get(name);
                track.Looping = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(room.Music))
                {
                    if (room.Music != "none")
                    {
                        track = audio.Tracks.Get($"{room.Music}");
                        track.Looping = true;
                    }
                }
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            restart();
        }

        protected virtual Drawable CreateLayerBehindPlayer() => Empty();

        private void onDeath(Vector2 position, Vector2 speed)
        {
            track?.Stop();
            deathOverlay.Play(position, speed);
            OnDeath?.Invoke(position);
        }

        private void onCompletion()
        {
            track?.Stop();
            Completed?.Invoke();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
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
            Player.Revive();
            deathOverlay.Restore();
            roomEntering.Stop();
            roomEntering.Play();
            track?.Start();
            OnRespawn?.Invoke();
        }

        protected override void Dispose(bool isDisposing)
        {
            track?.Stop();
            base.Dispose(isDisposing);
        }
    }
}
