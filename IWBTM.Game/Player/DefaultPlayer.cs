using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK.Input;
using osu.Framework.Graphics;
using osuTK;
using IWBTM.Game.Playfield;
using System;
using osu.Framework.Bindables;
using IWBTM.Game.Rooms;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Utils;
using IWBTM.Game.Helpers;

namespace IWBTM.Game.Player
{
    public class DefaultPlayer : CompositeDrawable
    {
        // Legacy constants
        private const double max_horizontal_speed = 0.15;
        private const double vertical_stop_speed_multiplier = 0.45;
        private const double jump_speed = 8.5;
        private const double jump2_speed = 7;
        private const double gravity = 0.45; // 0.4 is legacy, but this one matches better for some reason
        private const double max_vertical_speed = 9;

        private readonly Bindable<PlayerState> state = new Bindable<PlayerState>(PlayerState.Idle);
        public readonly BindableBool ShowHitbox = new BindableBool();

        private DrawableSample jump;
        private DrawableSample doubleJump;
        private DrawableSample shoot;

        private int horizontalDirection;
        private int availableJumpCount = 2;
        private double verticalSpeed;
        private bool midAir;

        public readonly Container Player;
        private readonly Container bulletsContainer;
        private readonly Container animationContainer;
        private readonly Container hitbox;
        private readonly Room room;

        public DefaultPlayer(Room room)
        {
            this.room = room;

            RelativeSizeAxes = Axes.Both;
            AddRangeInternal(new Drawable[]
            {
                bulletsContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
                Player = new Container
                {
                    Origin = Anchor.Centre,
                    Size = new Vector2(11, 21),
                    Children = new Drawable[]
                    {
                        animationContainer = new Container
                        {
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            X = -1.5f,
                            Size = new Vector2(Tile.SIZE)
                        },
                        hitbox = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.Red,
                                Alpha = 0.5f,
                            }
                        }
                    }
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            AddRangeInternal(new[]
            {
                jump = new DrawableSample(audio.Samples.Get("jump")),
                doubleJump = new DrawableSample(audio.Samples.Get("double-jump")),
                shoot = new DrawableSample(audio.Samples.Get("shoot")),
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            SetDefaultPosition();
            state.BindValueChanged(onStateChanged, true);
            ShowHitbox.BindValueChanged(value => hitbox.Alpha = value.NewValue ? 1 : 0, true);
        }

        public Vector2 PlayerPosition() => Player.Position;

        public Vector2 PlayerSize() => Player.Size;

        public PlayerState GetCurrentState() => state.Value;

        public void SetDefaultPosition()
        {
            var position = room.GetPlayerSpawnPosition();
            Player.Position = new Vector2(position.X + 16, position.Y + 16);
            verticalSpeed = 0;
            midAir = true;
            rightwards = true;
            updateVisual();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.ShiftLeft:
                        onJumpPressed();
                        return true;

                    case Key.Z:
                        onShoot();
                        return true;
                }
            };

            return base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            switch (e.Key)
            {
                case Key.ShiftLeft:
                    onJumpReleased();
                    return;
            }

            base.OnKeyUp(e);
        }

        private bool rightwards = true;

        public bool Rightwards() => rightwards;

        protected override void Update()
        {
            base.Update();

            var elapsedFrameTime = Clock.ElapsedFrameTime;

            // Limit vertical speed
            if (Math.Abs(verticalSpeed) > max_vertical_speed)
                verticalSpeed = Math.Sign(verticalSpeed) * max_vertical_speed;

            if (Precision.AlmostEquals(verticalSpeed, 0, 0.0001))
                verticalSpeed = 0;

            if (verticalSpeed < 0)
                checkBottomCollision();

            if (verticalSpeed > 0)
                checkTopCollision();

            horizontalDirection = 0;

            var keys = GetContainingInputManager().CurrentState.Keyboard.Keys;

            if (keys.IsPressed(Key.Right))
                horizontalDirection = 1;
            else if (keys.IsPressed(Key.Left))
                horizontalDirection = -1;

            if (horizontalDirection != 0)
            {
                rightwards = horizontalDirection > 0;
                updateVisual();

                if (rightwards)
                    checkRightCollision(elapsedFrameTime);
                else
                    checkLeftCollision(elapsedFrameTime);
            }

            if (midAir)
            {
                var legacyDistance = verticalSpeed;
                var adjustedDistance = legacyDistance * (elapsedFrameTime / 20);

                Player.Y -= (float)adjustedDistance;

                verticalSpeed -= gravity * (elapsedFrameTime / 20);
            }

            checkSpikes();
            updatePlayerState();
        }

        private void checkSpikes()
        {
            var playerLeftBorderPosition = (int)((Player.X - PlayerSize().X / 2) / Tile.SIZE);
            var playerRightBorderPosition = (int)((Player.X + PlayerSize().X / 2 - 1) / Tile.SIZE);
            var playerTopBorderPosition = (int)((Player.Y - PlayerSize().Y / 2) / Tile.SIZE);
            var playerBottomBorderPosition = (int)((Player.Y + PlayerSize().Y / 2 - 1) / Tile.SIZE);

            var topLeftTile = room.GetTileAt(playerLeftBorderPosition, playerTopBorderPosition);
            var topRightTile = room.GetTileAt(playerRightBorderPosition, playerTopBorderPosition);
            var bottomLeftTile = room.GetTileAt(playerLeftBorderPosition, playerBottomBorderPosition);
            var bottomRightTile = room.GetTileAt(playerRightBorderPosition, playerBottomBorderPosition);

            if (Room.TileIsSpike(topLeftTile))
            {
                if (CollisionHelper.Collided(PlayerPosition(), PlayerSize(), new Vector2(playerLeftBorderPosition, playerTopBorderPosition), topLeftTile))
                {
                    SetDefaultPosition();
                    return;
                }
            }

            if (Room.TileIsSpike(topRightTile))
            {
                if (CollisionHelper.Collided(PlayerPosition(), PlayerSize(), new Vector2(playerRightBorderPosition, playerTopBorderPosition), topRightTile))
                {
                    SetDefaultPosition();
                    return;
                }
            }

            if (Room.TileIsSpike(bottomLeftTile))
            {
                if (CollisionHelper.Collided(PlayerPosition(), PlayerSize(), new Vector2(playerLeftBorderPosition, playerBottomBorderPosition), bottomLeftTile))
                {
                    SetDefaultPosition();
                    return;
                }
            }

            if (Room.TileIsSpike(bottomRightTile))
            {
                if (CollisionHelper.Collided(PlayerPosition(), PlayerSize(), new Vector2(playerRightBorderPosition, playerBottomBorderPosition), bottomRightTile))
                {
                    SetDefaultPosition();
                    return;
                }
            }
        }

        private void updateVisual()
        {
            animationContainer.Scale = new Vector2(rightwards ? 1 : -1, 1);
            animationContainer.X = rightwards ? -1.5f : 1.5f;
        }

        private void checkRightCollision(double elapsedFrameTime)
        {
            var playerRightBorderPosition = (int)((Player.X + PlayerSize().X / 2 + 1) / Tile.SIZE);
            var playerLeftBorderPosition = (int)((Player.X - PlayerSize().X / 2) / Tile.SIZE);

            var playerTopBorderPosition = (int)((Player.Y - PlayerSize().Y / 2) / Tile.SIZE);
            var playerMiddleBorderPosition = (int)((Player.Y + PlayerSize().Y / 2 - 1) / Tile.SIZE);
            var playerBottomBorderPosition = (int)((Player.Y + PlayerSize().Y / 2 + 1) / Tile.SIZE);

            var topTile = room.GetTileAt(playerRightBorderPosition, playerTopBorderPosition);
            var middleTile = room.GetTileAt(playerRightBorderPosition, playerMiddleBorderPosition);
            var bottomTile = room.GetTileAt(playerLeftBorderPosition, playerBottomBorderPosition);

            if (Room.TileIsSolid(topTile) || Room.TileIsSolid(middleTile))
            {
                Player.X = playerRightBorderPosition * Tile.SIZE - PlayerSize().X / 2;
            }
            else
            {
                Player.X += (float)(max_horizontal_speed * elapsedFrameTime);

                if (!midAir && (Room.TileIsEmpty(bottomTile) || Room.TileIsSpike(bottomTile)))
                {
                    midAir = true;
                    availableJumpCount = 1;
                }
            }
        }

        private void checkLeftCollision(double elapsedFrameTime)
        {
            var playerLeftBorderPosition = (int)((Player.X - PlayerSize().X / 2 - 1) / Tile.SIZE);
            var playerRightBorderPosition = (int)((Player.X + PlayerSize().X / 2) / Tile.SIZE);

            var playerTopBorderPosition = (int)((Player.Y - PlayerSize().Y / 2) / Tile.SIZE);
            var playerMiddleBorderPosition = (int)((Player.Y + PlayerSize().Y / 2 - 1) / Tile.SIZE);
            var playerBottomBorderPosition = (int)((Player.Y + PlayerSize().Y / 2 + 1) / Tile.SIZE);

            var topTile = room.GetTileAt(playerLeftBorderPosition, playerTopBorderPosition);
            var middleTile = room.GetTileAt(playerLeftBorderPosition, playerMiddleBorderPosition);
            var bottomTile = room.GetTileAt(playerRightBorderPosition, playerBottomBorderPosition);

            if (Room.TileIsSolid(topTile) || Room.TileIsSolid(middleTile))
            {
                Player.X = (playerLeftBorderPosition + 1) * Tile.SIZE + PlayerSize().X / 2;
            }
            else
            {
                Player.X -= (float)(max_horizontal_speed * elapsedFrameTime);

                if (!midAir && (Room.TileIsEmpty(bottomTile) || Room.TileIsSpike(bottomTile)))
                {
                    midAir = true;
                    availableJumpCount = 1;
                }
            }
        }

        private void checkTopCollision()
        {
            var playerTopBorderPosition = (int)((Player.Y - PlayerSize().Y / 2 - 1) / Tile.SIZE);
            var playerLeftBorderPosition = (int)((Player.X - PlayerSize().X / 2 + 1) / Tile.SIZE);
            var playerRightBorderPosition = (int)((Player.X + PlayerSize().X / 2 - 1) / Tile.SIZE);

            var leftTile = room.GetTileAt(playerLeftBorderPosition, playerTopBorderPosition);
            var rightTile = room.GetTileAt(playerRightBorderPosition, playerTopBorderPosition);

            if (Room.TileIsSolid(leftTile) || Room.TileIsSolid(rightTile))
            {
                Player.Y = (playerTopBorderPosition + 1) * Tile.SIZE + PlayerSize().Y / 2;
                verticalSpeed = 0;
            }
        }

        private void checkBottomCollision()
        {
            var playerBottomBorderPosition = (int)((Player.Y + PlayerSize().Y / 2 + 1) / Tile.SIZE);
            var playerLeftBorderPosition = (int)((Player.X - PlayerSize().X / 2 + 1) / Tile.SIZE);
            var playerRightBorderPosition = (int)((Player.X + PlayerSize().X / 2 - 1) / Tile.SIZE);

            var leftTile = room.GetTileAt(playerLeftBorderPosition, playerBottomBorderPosition);
            var rightTile = room.GetTileAt(playerRightBorderPosition, playerBottomBorderPosition);

            if (Room.TileIsSolid(leftTile) || Room.TileIsSolid(rightTile))
            {
                resetJumpLogic();
                Player.Y = playerBottomBorderPosition * Tile.SIZE - PlayerSize().Y / 2;
            }
        }

        private void resetJumpLogic()
        {
            availableJumpCount = 2;
            verticalSpeed = 0;
            midAir = false;
        }

        private void onJumpPressed()
        {
            if (availableJumpCount == 0)
                return;

            midAir = true;

            availableJumpCount--;

            switch (availableJumpCount)
            {
                case 1:
                    jump.Play();
                    verticalSpeed = jump_speed;
                    break;

                case 0:
                    doubleJump.Play();
                    verticalSpeed = jump2_speed;
                    break;
            }
        }

        private void onJumpReleased()
        {
            if (verticalSpeed < 0)
                return;

            verticalSpeed *= vertical_stop_speed_multiplier;
        }

        private void onShoot()
        {
            shoot.Play();
            bulletsContainer.Add(new Bullet(Rightwards())
            {
                Position = PlayerPosition()
            });
        }

        private void onStateChanged(ValueChangedEvent<PlayerState> s)
        {
            animationContainer.Child = new PlayerAnimation(s.NewValue);
        }

        private void updatePlayerState()
        {
            if (verticalSpeed < 0)
            {
                state.Value = PlayerState.Fall;
                return;
            }

            if (verticalSpeed > 0)
            {
                state.Value = PlayerState.Jump;
                return;
            }

            if (horizontalDirection != 0)
            {
                state.Value = PlayerState.Run;
                return;
            }

            state.Value = PlayerState.Idle;
        }
    }
}
