using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.UserInterface;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;

namespace IWBTM.Game.Screens
{
    public class GameplayScreen : IWannaScreen
    {
        protected readonly DefaultPlayfield Playfield;

        private readonly Room room;
        private readonly SpriteText deathCountText;

        private readonly Bindable<int> deathCount = new Bindable<int>();

        private readonly float roomXBorder;
        private readonly float roomYBorder;

        public GameplayScreen(Room room, string name)
        {
            this.room = room;
            roomXBorder = (room.SizeX - 25) / 2 * DrawableTile.SIZE;
            roomYBorder = (room.SizeY - 19) / 2 * DrawableTile.SIZE;

            ValidForResume = false;

            AddInternal(new PlayfieldCameraContainer()
            {
                Children = new Drawable[]
                {
                    Playfield = CreatePlayfield(room, name).With(p => p.Completed = OnCompletion),
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
                }
            });

            Playfield.OnDeath += () => deathCount.Value++;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            deathCount.BindValueChanged(count => deathCountText.Text = $"deaths: {count.NewValue}", true);
        }

        protected override void Update()
        {
            base.Update();

            var playerPosition = Playfield.Player.PlayerPosition();

            if (room.SizeX > 25)
                Playfield.X = Math.Clamp(room.SizeX / 2 * DrawableTile.SIZE - playerPosition.X, -roomXBorder, roomXBorder);

            if (room.SizeY > 19)
                Playfield.Y = Math.Clamp(room.SizeY / 2 * DrawableTile.SIZE - playerPosition.Y, -roomYBorder, roomYBorder);
        }

        protected virtual void OnCompletion(List<Vector2> deathSpots)
        {
            this.Push(new ResultsScreen(deathSpots, room));
        }

        protected virtual DefaultPlayfield CreatePlayfield(Room room, string name) => new DefaultPlayfield(room, name);
    }
}
