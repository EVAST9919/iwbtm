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
using osu.Framework.Graphics.Sprites;
using osu.Framework.Bindables;
using osuTK.Graphics;
using System;
using System.Collections.Generic;

namespace IWBTM.Game.Screens.Play.Playfield
{
    public class DefaultPlayfield : CompositeDrawable
    {
        public static readonly Vector2 BASE_SIZE = new Vector2(768, 608);
        public static readonly int TILES_WIDTH = 24;
        public static readonly int TILES_HEIGHT = 19;

        public Action<List<Vector2>> Completed;

        public DefaultPlayer Player;
        private DeathOverlay deathOverlay;
        private Track track;
        private DrawableSample roomEntering;
        private SpriteText deathCountText;
        private readonly Bindable<int> deathCount = new Bindable<int>();

        private readonly List<Vector2> deathSpots = new List<Vector2>();

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
            DrawableRoom = new DrawableRoom(room);
            dependencies.Cache(DrawableRoom);

            Size = BASE_SIZE;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            AddRangeInternal(new Drawable[]
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
                new FillFlowContainer
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Margin = new MarginPadding(32 + 5),
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 5),
                    Children = new Drawable[]
                    {
                        deathCountText = new SpriteText
                        {
                            Colour = Color4.Black,
                            Font = FontUsage.Default.With(size: 14)
                        }
                    }
                },
                deathOverlay = new DeathOverlay(),
                roomEntering = new DrawableSample(audio.Samples.Get("room-entering"))
            });

            if (!string.IsNullOrEmpty(room.Music))
            {
                if (room.Music != "none")
                {
                    track = audio.Tracks.Get($"{room.Music}");
                    track.Looping = true;
                }
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            deathCount.BindValueChanged(count => deathCountText.Text = $"deaths: {count.NewValue}", true);
            restart();
        }

        protected virtual Drawable CreateLayerBehindPlayer() => Empty();

        private void onDeath(Vector2 position, Vector2 speed)
        {
            track?.Stop();
            deathOverlay.Play(position, speed);
            deathCount.Value++;
            deathSpots.Add(position);
        }

        private void onCompletion()
        {
            Completed?.Invoke(deathSpots);
            track?.Stop();
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
            Player.SetSavedPosition();
            deathOverlay.Restore();
            roomEntering.Stop();
            roomEntering.Play();
            track?.Start();
        }

        protected override void Dispose(bool isDisposing)
        {
            track?.Stop();
            base.Dispose(isDisposing);
        }
    }
}
