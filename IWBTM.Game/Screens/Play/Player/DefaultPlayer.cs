using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK.Input;
using osu.Framework.Graphics;
using osuTK;
using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Utils;
using IWBTM.Game.Helpers;
using IWBTM.Game.Rooms.Drawables;

namespace IWBTM.Game.Screens.Play.Player
{
    public class DefaultPlayer : CompositeDrawable
    {
        public static Vector2 SIZE = new Vector2(11, 21);

        // Legacy constants
        private const double max_horizontal_speed = 0.15;
        private const double vertical_stop_speed_multiplier = 0.45;
        private const double jump_speed = 8.5;
        private const double jump2_speed = 7;
        private const double gravity = 0.426; // 0.4 is legacy, but this one matches better for some reason
        private const double max_vertical_speed = 9;

        public readonly Bindable<PlayerState> State = new Bindable<PlayerState>(PlayerState.Idle);
        public readonly Bindable<bool> ShowHitbox = new Bindable<bool>();

        [Resolved]
        private DrawableRoom drawableRoom { get; set; }

        public Action<Vector2, Vector2> Died;
        public Action Completed;
        public Action<Vector2, bool> Saved;

        private bool died;
        private bool completed;

        private DrawableSample jump;
        private DrawableSample doubleJump;
        private DrawableSample shoot;

        private int availableJumpCount = 2;
        private double verticalSpeed;
        private double horizontalSpeed;
        private bool midAir;
        private bool rightwards = true;

        private Container player;
        private BulletsContainer bulletsContainer;
        private Container animationContainer;
        private Container hitbox;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                bulletsContainer = new BulletsContainer(drawableRoom.Size)
                {
                    OnSave = () => Saved?.Invoke(PlayerPosition(), rightwards)
                },
                player = new Container
                {
                    Origin = Anchor.Centre,
                    Size = SIZE,
                    Children = new Drawable[]
                    {
                        animationContainer = new Container
                        {
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            X = -1.5f,
                            Size = new Vector2(DrawableTile.SIZE)
                        },
                        hitbox = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.DeepPink,
                                Alpha = 0.5f,
                            }
                        }
                    }
                },
                jump = new DrawableSample(audio.Samples.Get("jump")),
                doubleJump = new DrawableSample(audio.Samples.Get("double-jump")),
                shoot = new DrawableSample(audio.Samples.Get("shoot")),
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            State.BindValueChanged(onStateChanged, true);
            ShowHitbox.BindValueChanged(show => hitbox.Alpha = show.NewValue ? 1 : 0, true);
        }

        public Vector2 PlayerPosition() => player.Position;

        public void Revive(Vector2 position, bool rightwards)
        {
            if (completed)
                return;

            player.Position = position;
            this.rightwards = rightwards;

            verticalSpeed = 0;
            midAir = true;
            availableJumpCount = 1;

            died = false;
            updateAnimationDirection();
            player.Show();
        }

        private void onDeath()
        {
            died = true;
            player.Hide();
            Died?.Invoke(PlayerPosition(), new Vector2((float)horizontalSpeed, (float)verticalSpeed));
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (died || completed)
                return false;

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
            if (died || completed)
                return;

            switch (e.Key)
            {
                case Key.ShiftLeft:
                    onJumpReleased();
                    return;
            }

            base.OnKeyUp(e);
        }

        protected override void Update()
        {
            base.Update();

            if (died || completed)
                return;

            checkBorders();

            if (died)
                return;

            var elapsedFrameTime = Clock.ElapsedFrameTime;

            // Limit vertical speed
            if (Math.Abs(verticalSpeed) > max_vertical_speed)
                verticalSpeed = Math.Sign(verticalSpeed) * max_vertical_speed;

            if (Precision.AlmostEquals(verticalSpeed, 0, 0.0001))
                verticalSpeed = 0;

            if (verticalSpeed <= 0)
            {
                checkBottomCollision();
                checkBottomKillerBlock();
            }
            else
            {
                checkTopCollision();
                checkTopKillerBlock();
            }

            if (died)
                return;

            horizontalSpeed = 0;

            var keys = GetContainingInputManager().CurrentState.Keyboard.Keys;

            if (keys.IsPressed(Key.Right))
                horizontalSpeed = 3;
            else if (keys.IsPressed(Key.Left))
                horizontalSpeed = -3;

            if (horizontalSpeed != 0)
            {
                rightwards = horizontalSpeed > 0;
                updateAnimationDirection();

                if (rightwards)
                {
                    checkRightCollision(elapsedFrameTime);
                    checkRightKillerBlock();
                }
                else
                {
                    checkLeftCollision(elapsedFrameTime);
                    checkLeftKillerBlock();
                }
            }

            if (died)
                return;

            if (midAir)
            {
                var legacyDistance = verticalSpeed;
                var adjustedDistance = legacyDistance * (elapsedFrameTime / 20);

                player.Y -= (float)adjustedDistance;

                verticalSpeed -= gravity * (elapsedFrameTime / 20);
            }

            checkSpikes();
            checkCherries();
            checkJumpRefresher();
            updatePlayerState();
            checkCompletion();
        }

        private void checkBorders()
        {
            if (PlayerPosition().X - SIZE.X / 2f <= 0 || PlayerPosition().X + SIZE.X / 2f >= drawableRoom.Size.X
                || PlayerPosition().Y - SIZE.Y / 2f <= 0 || PlayerPosition().Y + SIZE.Y / 2f + 1 >= drawableRoom.Size.Y)
                onDeath();
        }

        private void checkSpikes()
        {
            foreach (var t in drawableRoom.Tiles)
            {
                if (MathExtensions.Distance(PlayerPosition(), t.Position) > 64)
                    continue;

                if (!DrawableTile.IsGroup(t, TileGroup.Spike))
                    continue;

                if (CollisionHelper.Collided(PlayerPosition(), t))
                {
                    onDeath();
                    return;
                }
            }
        }

        private void checkCompletion()
        {
            if (died || completed)
                return;

            if (drawableRoom.HasTileAtPixel(PlayerPosition(), TileType.Warp))
            {
                completed = true;
                Completed?.Invoke();
            }
        }

        private void checkCherries()
        {
            if (died || completed)
                return;

            foreach (var t in drawableRoom.Tiles)
            {
                if (MathExtensions.Distance(PlayerPosition(), t.Position) < 64)
                {
                    if (t.Tile.Type == TileType.Cherry)
                    {
                        if (CollisionHelper.CollidedWithCircle(PlayerPosition(), t))
                        {
                            onDeath();
                            return;
                        }
                    }
                }
            }
        }

        private void updateAnimationDirection()
        {
            animationContainer.Scale = new Vector2(rightwards ? 1 : -1, 1);
            animationContainer.X = rightwards ? -1.5f : 1.5f;
        }

        private void checkRightCollision(double elapsedFrameTime)
        {
            var playerRightBorderPosition = player.X + SIZE.X / 2;

            var playerTopBorderPosition = player.Y - SIZE.Y / 2;
            var playerBottomBorderPosition = player.Y + SIZE.Y / 2 - 1;

            var hasTopCollision = drawableRoom.HasTileOfGroupAt(new Vector2(playerRightBorderPosition, playerTopBorderPosition), TileGroup.Solid);
            var hasMiddleCollision = drawableRoom.HasTileOfGroupAt(new Vector2(playerRightBorderPosition, player.Y), TileGroup.Solid);
            var hasBottomCollision = drawableRoom.HasTileOfGroupAt(new Vector2(playerRightBorderPosition, playerBottomBorderPosition), TileGroup.Solid);

            if (hasTopCollision || hasMiddleCollision || hasBottomCollision)
                player.X = (int)playerRightBorderPosition - SIZE.X / 2;
            else
                player.X += (float)(max_horizontal_speed * elapsedFrameTime);
        }

        private void checkRightKillerBlock()
        {
            var playerRightBorderPosition = player.X + SIZE.X / 2 - 1;

            var playerTopBorderPosition = player.Y - SIZE.Y / 2;
            var playerMiddleBorderPosition = player.Y + SIZE.Y / 2 - 1;

            var hasTopCollision = drawableRoom.HasTileAtPixel(new Vector2(playerRightBorderPosition, playerTopBorderPosition), TileType.KillerBlock);
            var hasBottomCollision = drawableRoom.HasTileAtPixel(new Vector2(playerRightBorderPosition, playerMiddleBorderPosition), TileType.KillerBlock);

            if (hasTopCollision || hasBottomCollision)
                onDeath();
        }

        private void checkLeftCollision(double elapsedFrameTime)
        {
            var playerLeftBorderPosition = player.X - SIZE.X / 2 - 1;

            var playerTopBorderPosition = player.Y - SIZE.Y / 2;
            var playerBottomBorderPosition = player.Y + SIZE.Y / 2 - 1;

            var hasTopCollision = drawableRoom.HasTileOfGroupAt(new Vector2(playerLeftBorderPosition, playerTopBorderPosition), TileGroup.Solid);
            var hasMiddleCollision = drawableRoom.HasTileOfGroupAt(new Vector2(playerLeftBorderPosition, player.Y), TileGroup.Solid);
            var hasBottomCollision = drawableRoom.HasTileOfGroupAt(new Vector2(playerLeftBorderPosition, playerBottomBorderPosition), TileGroup.Solid);

            if (hasTopCollision || hasMiddleCollision || hasBottomCollision)
                player.X = (int)playerLeftBorderPosition + 1 + SIZE.X / 2;
            else
                player.X -= (float)(max_horizontal_speed * elapsedFrameTime);
        }

        private void checkLeftKillerBlock()
        {
            var playerLeftBorderPosition = player.X - SIZE.X / 2;

            var playerTopBorderPosition = player.Y - SIZE.Y / 2;
            var playerMiddleBorderPosition = player.Y + SIZE.Y / 2 - 1;

            var hasTopCollision = drawableRoom.HasTileAtPixel(new Vector2(playerLeftBorderPosition, playerTopBorderPosition), TileType.KillerBlock);
            var hasBottomCollision = drawableRoom.HasTileAtPixel(new Vector2(playerLeftBorderPosition, playerMiddleBorderPosition), TileType.KillerBlock);

            if (hasTopCollision || hasBottomCollision)
                onDeath();
        }

        private void checkTopCollision()
        {
            var playerTopBorderPosition = player.Y - SIZE.Y / 2 - 1;
            var playerLeftBorderPosition = player.X - SIZE.X / 2;
            var playerRightBorderPosition = player.X + SIZE.X / 2 - 1;

            var leftTile = drawableRoom.GetTileOfGroupAt(new Vector2(playerLeftBorderPosition, playerTopBorderPosition), TileGroup.Solid);
            var rightTile = drawableRoom.GetTileOfGroupAt(new Vector2(playerRightBorderPosition, playerTopBorderPosition), TileGroup.Solid);

            if (leftTile != null || rightTile != null)
            {
                var closestDrawableTilePosition = Math.Max(leftTile?.Y ?? double.MinValue, rightTile?.Y ?? double.MinValue);
                var closestDrawableTileSize = Math.Max(leftTile?.Size.Y ?? double.MinValue, rightTile?.Size.Y ?? double.MinValue);
                player.Y = (float)(closestDrawableTilePosition + closestDrawableTileSize + SIZE.Y / 2);
                verticalSpeed = 0;
            }
        }

        private void checkTopKillerBlock()
        {
            var playerTopBorderPosition = player.Y - SIZE.Y / 2;
            var playerLeftBorderPosition = player.X - SIZE.X / 2;
            var playerRightBorderPosition = player.X + SIZE.X / 2 - 1;

            var hasLeftCollision = drawableRoom.HasTileAtPixel(new Vector2(playerLeftBorderPosition, playerTopBorderPosition), TileType.KillerBlock);
            var hasRightCollision = drawableRoom.HasTileAtPixel(new Vector2(playerRightBorderPosition, playerTopBorderPosition), TileType.KillerBlock);

            if (hasLeftCollision || hasRightCollision)
                onDeath();
        }

        private void checkBottomCollision()
        {
            var playerBottomBorderPosition = player.Y + SIZE.Y / 2;
            var playerLeftBorderPosition = player.X - SIZE.X / 2;
            var playerRightBorderPosition = player.X + SIZE.X / 2 - 1;

            var leftTile = drawableRoom.GetTileOfGroupAt(new Vector2(playerLeftBorderPosition, playerBottomBorderPosition), TileGroup.Solid);
            var rightTile = drawableRoom.GetTileOfGroupAt(new Vector2(playerRightBorderPosition, playerBottomBorderPosition), TileGroup.Solid);

            if (leftTile != null || rightTile != null)
            {
                var closestDrawableTilePosition = Math.Min(leftTile?.Y ?? double.MaxValue, rightTile?.Y ?? double.MaxValue);
                player.Y = (float)(closestDrawableTilePosition - SIZE.Y / 2);
                resetJumpLogic();
            }
            else
            {
                if (!midAir)
                {
                    midAir = true;
                    availableJumpCount = 1;
                }
            }
        }

        private void checkBottomKillerBlock()
        {
            var playerBottomBorderPosition = player.Y + SIZE.Y / 2 - 1;
            var playerLeftBorderPosition = player.X - SIZE.X / 2;
            var playerRightBorderPosition = player.X + SIZE.X / 2 - 1;

            var leftTileCollision = drawableRoom.HasTileAtPixel(new Vector2(playerLeftBorderPosition, playerBottomBorderPosition), TileType.KillerBlock);
            var rightTileCollision = drawableRoom.HasTileAtPixel(new Vector2(playerRightBorderPosition, playerBottomBorderPosition), TileType.KillerBlock);

            if (leftTileCollision || rightTileCollision)
                onDeath();
        }

        private void checkJumpRefresher()
        {
            foreach (var t in drawableRoom.Tiles)
            {
                if (MathExtensions.Distance(PlayerPosition(), t.Position) < 64)
                {
                    if (t.Tile.Type == TileType.Jumprefresher)
                    {
                        var refresher = (DrawableJumpRefresher)t;

                        if (refresher.IsActive)
                        {
                            if (CollisionHelper.CollidedWithCircle(PlayerPosition(), t))
                            {
                                refresher.Deactivate();
                                availableJumpCount = 1;
                                return;
                            }
                        }
                    }
                }
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
            bulletsContainer.GenerateBullet(PlayerPosition(), rightwards);
        }

        private void onStateChanged(ValueChangedEvent<PlayerState> s)
        {
            animationContainer.Child = new PlayerAnimation(s.NewValue);
        }

        private void updatePlayerState()
        {
            if (died || completed)
                return;

            if (verticalSpeed < 0)
            {
                State.Value = PlayerState.Fall;
                return;
            }

            if (verticalSpeed > 0)
            {
                State.Value = PlayerState.Jump;
                return;
            }

            if (horizontalSpeed != 0)
            {
                State.Value = PlayerState.Run;
                return;
            }

            State.Value = PlayerState.Idle;
        }
    }
}
