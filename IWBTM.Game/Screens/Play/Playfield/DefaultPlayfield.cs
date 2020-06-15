using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osuTK;
using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Play.Player;
using IWBTM.Game.Screens.Play.Death;
using IWBTM.Game.Rooms.Drawables;
using osu.Framework.Graphics.Audio;
using System;

namespace IWBTM.Game.Screens.Play.Playfield
{
    public class DefaultPlayfield : CompositeDrawable
    {
        public Action Completed;
        public Action<Vector2> OnDeath;
        public Action<Vector2, bool> OnSave;

        public DefaultPlayer Player;
        private PlayerParticlesContainer deathOverlay;
        private DrawableSample roomEntering;

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        private readonly Room room;
        protected DrawableRoom DrawableRoom;

        public DefaultPlayfield(Room room)
        {
            this.room = room;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            DrawableRoom = new DrawableRoom(room, false, false, true, true, false, false);
            dependencies.Cache(DrawableRoom);

            Size = new Vector2(room.SizeX, room.SizeY) * DrawableTile.SIZE;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            InternalChildren = new Drawable[]
            {
                DrawableRoom,
                CreateLayerBehindPlayer(),
                Player = new DefaultPlayer
                {
                    Died = onDeath,
                    Completed = onCompletion,
                    Saved = onSave,
                },
                new WaterRenderer(room),
                deathOverlay = new PlayerParticlesContainer(),
                roomEntering = new DrawableSample(audio.Samples.Get("room-entering"))
            };
        }

        protected virtual Drawable CreateLayerBehindPlayer() => Empty();

        private void onDeath(Vector2 position, Vector2 speed)
        {
            deathOverlay.Play(position, speed);
            OnDeath?.Invoke(position);
        }

        private void onCompletion()
        {
            Completed?.Invoke();
        }

        private void onSave(Vector2 position, bool rightwards)
        {
            OnSave?.Invoke(position, rightwards);
        }

        public void Restart(Vector2 position, bool rightwards)
        {
            Player.Revive(position, rightwards);
            DrawableRoom.RestartAnimations();
            deathOverlay.Restore();
            roomEntering.Stop();
            roomEntering.Play();
        }
    }
}
