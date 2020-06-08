using IWBTM.Game.Helpers;
using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Death;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IWBTM.Game.Screens
{
    public class GameplayScreen : IWannaScreen
    {
        protected DefaultPlayfield Playfield
        {
            get
            {
                if (!roomPlaceholder.Any())
                    return null;

                return (DefaultPlayfield)roomPlaceholder.Child;
            }
        }

        [Resolved]
        private AudioManager audio { get; set; }

        private Track track;
        private readonly Level level;
        private readonly string name;
        private SpriteText deathCountText;
        private DeathOverlay deathOverlay;
        private Container roomPlaceholder;

        private readonly Bindable<int> deathCount = new Bindable<int>();
        private readonly List<(Vector2, int)> deathSpots = new List<(Vector2, int)>();

        public GameplayScreen(Level level, string name)
        {
            this.level = level;
            this.name = name;

            ValidForResume = false;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(new PlayfieldCameraContainer
            {
                Children = new Drawable[]
                {
                    roomPlaceholder = new Container()
                    {
                        RelativeSizeAxes = Axes.Both,
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
                                Colour = IWannaColour.Blue,
                                Font = FontUsage.Default.With(size: 14)
                            }
                        }
                    },
                    deathOverlay = new DeathOverlay()
                }
            });

            savedPoint = (RoomHelper.PlayerSpawnPosition(level.Rooms.First()), true, 0);
            loadRoom(currentRoomIndex, (savedPoint.Item1, savedPoint.Item2));
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            deathCount.BindValueChanged(count => deathCountText.Text = $"deaths: {count.NewValue}", true);
        }

        private float roomXBorder;
        private float roomYBorder;
        private Room currentRoom;
        private int currentRoomIndex;

        private string lastRoomAudio;

        private (Vector2, bool, int) savedPoint;

        private void loadRoom(int index, (Vector2, bool)? restartPoint)
        {
            lastRoomAudio = currentRoom?.Music;

            currentRoomIndex = index;
            currentRoom = level.Rooms[index];

            roomXBorder = (currentRoom.SizeX - 25) / 2 * DrawableTile.SIZE;
            roomYBorder = (currentRoom.SizeY - 19) / 2 * DrawableTile.SIZE;

            LoadComponentAsync(CreatePlayfield(currentRoom), loaded =>
            {
                NewPlayfieldLoaded(loaded);

                deathOverlay.Restore();

                if (restartPoint.HasValue)
                {
                    Playfield.Restart(restartPoint.Value.Item1, restartPoint.Value.Item2);
                }
                else
                {
                    Playfield.Restart(RoomHelper.PlayerSpawnPosition(currentRoom), true);
                }

                var currentRoomAudio = currentRoom.Music;

                if (currentRoomAudio != lastRoomAudio)
                {
                    track?.Stop();
                    if (currentRoomAudio != "none")
                    {
                        track = audio.Tracks.Get(currentRoomAudio);
                        track.Looping = true;
                    }
                    else
                        track = null;
                }

                track?.Start();
            });
        }

        protected virtual void NewPlayfieldLoaded(DefaultPlayfield playfield)
        {
            playfield.OnDeath += onDeath;
            playfield.Completed += OnCompletion;
            playfield.OnSave += onSave;
            roomPlaceholder.Child = playfield;
        }

        private void onDeath(Vector2 position)
        {
            track?.Stop();
            deathCount.Value++;
            deathOverlay.Play();
            deathSpots.Add((position, currentRoomIndex));
        }

        private void onSave(Vector2 position, bool rightwards)
        {
            savedPoint = (position, rightwards, currentRoomIndex);
        }

        protected override void Update()
        {
            base.Update();

            if (Playfield != null)
            {
                var playerPosition = Playfield.Player.PlayerPosition();

                if (currentRoom.SizeX > 25)
                    Playfield.X = Math.Clamp(currentRoom.SizeX / 2 * DrawableTile.SIZE - playerPosition.X, -roomXBorder, roomXBorder);

                if (currentRoom.SizeY > 19)
                    Playfield.Y = Math.Clamp(currentRoom.SizeY / 2 * DrawableTile.SIZE - playerPosition.Y, -roomYBorder, roomYBorder);
            }
        }

        protected virtual void OnCompletion()
        {
            if (level.Rooms.Last() == level.Rooms[currentRoomIndex])
            {
                track?.Stop();
                this.Push(new ResultsScreen(deathSpots, level));
                return;
            }

            currentRoomIndex++;
            loadRoom(currentRoomIndex, null);
        }

        private void restart()
        {
            if (savedPoint.Item3 == currentRoomIndex)
            {
                deathOverlay.Restore();
                Playfield.Restart(savedPoint.Item1, savedPoint.Item2);
                track?.Start();
            }
            else
                loadRoom(savedPoint.Item3, (savedPoint.Item1, savedPoint.Item2));
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

        protected virtual DefaultPlayfield CreatePlayfield(Room room) => new DefaultPlayfield(room);

        protected override void OnExit()
        {
            track?.Stop();
            base.OnExit();
        }

        protected override void Dispose(bool isDisposing)
        {
            track?.Stop();
            base.Dispose(isDisposing);
        }
    }
}
