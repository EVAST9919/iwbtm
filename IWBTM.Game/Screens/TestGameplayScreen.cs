﻿using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.Screens.Test;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.Screens
{
    public class TestGameplayScreen : GameplayScreen
    {
        private SpriteText xPosition;
        private SpriteText yPosition;
        private SpriteText state;
        private SpriteText xSpeed;
        private SpriteText ySpeed;

        private IWannaCheckbox hitboxCheckbox;
        private IWannaCheckbox deathSpotsCheckbox;
        private IWannaCheckbox godCheckbox;

        public TestGameplayScreen(Level level, string name)
            : base(level, name)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(new Container
            {
                Margin = new MarginPadding(10),
                AutoSizeAxes = Axes.Both,
                Masking = true,
                CornerRadius = 8,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black.Opacity(0.5f)
                    },
                    new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Padding = new MarginPadding(10),
                        Child = new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 5),
                            Children = new Drawable[]
                            {
                                hitboxCheckbox = new IWannaCheckbox
                                {
                                    LabelText = "Show hitbox",
                                },
                                deathSpotsCheckbox = new IWannaCheckbox
                                {
                                    LabelText = "Show death spots",
                                },
                                godCheckbox = new IWannaCheckbox
                                {
                                    LabelText = "God mode"
                                },
                                xPosition = new SpriteText(),
                                yPosition = new SpriteText(),
                                state = new SpriteText(),
                                xSpeed = new SpriteText(),
                                ySpeed = new SpriteText(),
                            }
                        }
                    }
                }
            });
        }

        private Vector2 lastPlayerPosition;

        protected override void NewPlayfieldLoaded(DefaultPlayfield playfield)
        {
            base.NewPlayfieldLoaded(playfield);

            hitboxCheckbox.Current = ((TestPlayfield)playfield).ShowHitbox;
            deathSpotsCheckbox.Current = ((TestPlayfield)playfield).ShowDeath;
            godCheckbox.Current = ((TestPlayfield)playfield).GodMode;
        }

        protected override void Update()
        {
            base.Update();

            var playfield = getPlayfield();

            if (playfield != null)
            {
                var player = playfield.Player;
                var playerPosition = player.PlayerPosition();

                if (playerPosition != lastPlayerPosition)
                {
                    lastPlayerPosition = playerPosition;
                    xPosition.Text = $"X: {playerPosition.X}";
                    yPosition.Text = $"Y: {playerPosition.Y}";
                }

                state.Text = $"State: {player.State.Value.ToString()}";

                if (!player.IsDead)
                {
                    var speed = player.PlayerSpeed();

                    xSpeed.Text = $"Horizontal speed: {speed.X}";
                    ySpeed.Text = $"Vertical speed: {speed.Y.ToString("0.000")}";
                }
            }
        }

        protected override DefaultPlayfield CreatePlayfield(Room room) => new TestPlayfield(room);

        private TestPlayfield getPlayfield() => (TestPlayfield)Playfield;

        protected override void OnCompletion() => this.Exit();
    }
}
