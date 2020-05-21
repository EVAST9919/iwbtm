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

namespace IWBTM.Game.Screens.Play.Playfield
{
    public class DefaultPlayfield : CompositeDrawable
    {
        public static readonly Vector2 BASE_SIZE = new Vector2(768, 608);
        public static readonly int TILES_WIDTH = 24;
        public static readonly int TILES_HEIGHT = 19;

        public DefaultPlayer Player;
        private DeathOverlay deathOverlay;
        private Track track;

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        private readonly Room room;

        public DefaultPlayfield(Room room)
        {
            this.room = room;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            DrawableRoom drawableRoom = new DrawableRoom(room);
            dependencies.Cache(drawableRoom);

            Size = BASE_SIZE;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                drawableRoom,
                Player = new DefaultPlayer
                {
                    OnDeath = onDeath,
                    OnRespawn = onRespawn
                },
                deathOverlay = new DeathOverlay()
            });

            track = audio.Tracks.Get("Ghost Rule");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            restart();
        }

        private void onDeath(Vector2 position, Vector2 speed)
        {
            deathOverlay.Play(position, speed);
        }

        private void onRespawn()
        {
            deathOverlay.Restore();
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
            //track.Restart();
            Player.SetSavedPosition();
        }
    }
}
