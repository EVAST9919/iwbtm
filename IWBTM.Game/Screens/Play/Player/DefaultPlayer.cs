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
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Test;

namespace IWBTM.Game.Screens.Play.Player
{
    public class DefaultPlayer : CompositeDrawable, IHasHitbox
    {
        public static Vector2 SIZE = new Vector2(11, 21);

        // Legacy constants
        private const double max_horizontal_speed = 0.15;
        private const double vertical_stop_speed_multiplier = 0.45;
        private const double jump_speed = 8.5;
        private const double jump2_speed = 7;
        private const double gravity = 0.45; // 0.4 is legacy, but this one matches better for some reason
        private const double max_vertical_speed = 9;

        public readonly Bindable<PlayerState> State = new Bindable<PlayerState>(PlayerState.Idle);

        [Resolved]
        private DrawableRoom drawableRoom { get; set; }

        public Action<Vector2, Vector2> Died;
        public Action Completed;

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

        public readonly Container Player;
        private readonly BulletsContainer bulletsContainer;
        private readonly Container animationContainer;
        private readonly Container hitbox;

        private (Vector2 position, bool rightwards) savedPosition;

        public DefaultPlayer()
        {
            RelativeSizeAxes = Axes.Both;
            AddRangeInternal(new Drawable[]
            {
                bulletsContainer = new BulletsContainer
                {
                    OnSave = () => savedPosition = (PlayerPosition(), rightwards)
                },
                Player = new Container
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
                            Alpha = 0,
                            RelativeSizeAxes = Axes.Both,
                            Child = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.DeepPink,
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

            State.BindValueChanged(onStateChanged, true);
        }

        public Vector2 PlayerPosition() => Player.Position;

        public void Revive()
        {
            if (savedPosition == default)
            {
                var position = drawableRoom.PlayerSpawnPosition;
                Player.Position = new Vector2(position.X + 16, position.Y + DrawableTile.SIZE - SIZE.Y / 2f);
                rightwards = true;
            }
            else
            {
                Player.Position = savedPosition.position;
                rightwards = savedPosition.rightwards;
            }

            verticalSpeed = 0;
            midAir = true;
            availableJumpCount = 1;

            died = false;
            updateAnimationDirection();
            Player.Show();
        }

        private void onDeath()
        {
            died = true;
            Player.Hide();
            Died?.Invoke(PlayerPosition(), new Vector2((float)horizontalSpeed, (float)verticalSpeed));
        }

        private void onCompletion()
        {
            completed = true;
            Completed?.Invoke();
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

            var elapsedFrameTime = Clock.ElapsedFrameTime;

            // Limit vertical speed
            if (Math.Abs(verticalSpeed) > max_vertical_speed)
                verticalSpeed = Math.Sign(verticalSpeed) * max_vertical_speed;

            if (Precision.AlmostEquals(verticalSpeed, 0, 0.0001))
                verticalSpeed = 0;

            if (verticalSpeed <= 0)
                checkBottomCollision();
            else
                checkTopCollision();

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
            checkCherries();
            checkCompletion();
            updatePlayerState();
        }

        private void checkBorders()
        {
            if (PlayerPosition().X - SIZE.X / 2f <= 0 || PlayerPosition().X + SIZE.X / 2f >= DefaultPlayfield.BASE_SIZE.X
                || PlayerPosition().Y - SIZE.Y / 2f <= 0 || PlayerPosition().Y + SIZE.Y / 2f + 1 >= DefaultPlayfield.BASE_SIZE.Y)
                onDeath();
        }

        private void checkSpikes()
        {
            foreach (var t in drawableRoom.Children)
            {
                if (MathExtensions.Distance(PlayerPosition(), t.Position) < 64)
                {
                    if (DrawableTile.IsGroup(t, TileGroup.Spike))
                    {
                        if (CollisionHelper.Collided(PlayerPosition(), t))
                        {
                            onDeath();
                            return;
                        }
                    }
                }
            }
        }

        private void checkCompletion()
        {
            if (died)
                return;

            if (drawableRoom.HasTileAt(PlayerPosition(), TileGroup.Warp))
                onCompletion();
        }

        private void checkCherries()
        {
            if (died)
                return;

            foreach (var t in drawableRoom.Children)
            {
                if (MathExtensions.Distance(PlayerPosition(), t.Position) < 64)
                {
                    if (DrawableTile.IsGroup(t, TileGroup.Cherry))
                    {
                        if (CollisionHelper.CollidedWithCherry(PlayerPosition(), t))
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
            var playerRightBorderPosition = Player.X + SIZE.X / 2;

            var playerTopBorderPosition = Player.Y - SIZE.Y / 2;
            var playerMiddleBorderPosition = Player.Y + SIZE.Y / 2 - 1;

            var topDrawableTile = drawableRoom.GetTileAt(new Vector2(playerRightBorderPosition, playerTopBorderPosition), TileGroup.Solid);
            var middleDrawableTile = drawableRoom.GetTileAt(new Vector2(playerRightBorderPosition, playerMiddleBorderPosition), TileGroup.Solid);

            if (topDrawableTile != null || middleDrawableTile != null)
            {
                var closestDrawableTilePosition = Math.Min(topDrawableTile?.X ?? double.MaxValue, middleDrawableTile?.X ?? double.MaxValue);
                Player.X = (int)closestDrawableTilePosition - SIZE.X / 2;
            }
            else
                Player.X += (float)(max_horizontal_speed * elapsedFrameTime);
        }

        private void checkLeftCollision(double elapsedFrameTime)
        {
            var playerLeftBorderPosition = Player.X - SIZE.X / 2 - 1;

            var playerTopBorderPosition = Player.Y - SIZE.Y / 2;
            var playerMiddleBorderPosition = Player.Y + SIZE.Y / 2 - 1;

            var topDrawableTile = drawableRoom.GetTileAt(new Vector2(playerLeftBorderPosition, playerTopBorderPosition), TileGroup.Solid);
            var middleDrawableTile = drawableRoom.GetTileAt(new Vector2(playerLeftBorderPosition, playerMiddleBorderPosition), TileGroup.Solid);

            if (topDrawableTile != null || middleDrawableTile != null)
            {
                var closestDrawableTilePosition = Math.Max(topDrawableTile?.X ?? double.MinValue, middleDrawableTile?.X ?? double.MinValue);
                Player.X = (float)closestDrawableTilePosition + DrawableTile.SIZE + SIZE.X / 2;
            }
            else
                Player.X -= (float)(max_horizontal_speed * elapsedFrameTime);
        }

        private void checkTopCollision()
        {
            var playerTopBorderPosition = Player.Y - SIZE.Y / 2 - 1;
            var playerLeftBorderPosition = Player.X - SIZE.X / 2;
            var playerRightBorderPosition = Player.X + SIZE.X / 2 - 1;

            var leftDrawableTile = drawableRoom.GetTileAt(new Vector2(playerLeftBorderPosition, playerTopBorderPosition), TileGroup.Solid);
            var rightDrawableTile = drawableRoom.GetTileAt(new Vector2(playerRightBorderPosition, playerTopBorderPosition), TileGroup.Solid);

            if (leftDrawableTile != null || rightDrawableTile != null)
            {
                var closestDrawableTilePosition = Math.Max(leftDrawableTile?.Y ?? double.MinValue, rightDrawableTile?.Y ?? double.MinValue);
                Player.Y = (float)closestDrawableTilePosition + DrawableTile.SIZE + SIZE.Y / 2;
                verticalSpeed = 0;
            }
        }

        private void checkBottomCollision()
        {
            var playerBottomBorderPosition = Player.Y + SIZE.Y / 2 + 1;
            var playerLeftBorderPosition = Player.X - SIZE.X / 2;
            var playerRightBorderPosition = Player.X + SIZE.X / 2 - 1;

            var leftDrawableTile = drawableRoom.GetTileAt(new Vector2(playerLeftBorderPosition, playerBottomBorderPosition), TileGroup.Solid);
            var rightDrawableTile = drawableRoom.GetTileAt(new Vector2(playerRightBorderPosition, playerBottomBorderPosition), TileGroup.Solid);

            if (leftDrawableTile != null || rightDrawableTile != null)
            {
                var closestDrawableTilePosition = Math.Min(leftDrawableTile?.Y ?? double.MaxValue, rightDrawableTile?.Y ?? double.MaxValue);
                Player.Y = (int)Math.Round(closestDrawableTilePosition) - SIZE.Y / 2;

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
            if (died)
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

        public void Toggle(bool show)
        {
            hitbox.Alpha = show ? 1 : 0;
        }
    }
}
