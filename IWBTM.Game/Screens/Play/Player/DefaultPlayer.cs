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
using IWBTM.Game.Screens.Create;

namespace IWBTM.Game.Screens.Play.Player
{
    public class DefaultPlayer : CompositeDrawable
    {
        public static Vector2 SIZE = new Vector2(11, 21);
        private const double half_width = 5.5;
        private const double half_height = 10.5;

        // Legacy constants
        private const double max_horizontal_speed = 0.15;
        private const double vertical_stop_speed_multiplier = 0.45;
        private const double jump_speed = 8.5;
        private const double jump2_speed = 7;
        private const double gravity = 0.426; // 0.4 is legacy, but this one matches better for some reason
        private const double max_vertical_speed = 9;
        private const double max_water_vertical_speed = 2;

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

        private float outherBottomBorderPosition() => (float)(player.Y + half_height);
        private float innerBottomBorderPosition() => outherBottomBorderPosition() - 1;
        private float innerTopBorderPosition() => (float)(player.Y - half_height);
        private float outherTopBorderPosition() => innerTopBorderPosition() - 1;

        private float outherRightBorderPosition() => (float)(player.X + half_width);
        private float innerRightBorderPosition() => outherRightBorderPosition() - 1;
        private float innerLeftBorderPosition() => (float)(player.X - half_width);
        private float outherLeftBorderPosition() => innerLeftBorderPosition() - 1;

        protected override void Update()
        {
            base.Update();

            if (died || completed)
                return;

            var elapsedFrameTime = Clock.ElapsedFrameTime;

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

            bool inWater = checkInWater();

            if (Precision.AlmostEquals(verticalSpeed, 0, 0.0001))
                verticalSpeed = 0;

            if (verticalSpeed < 0)
            {
                var maxVerticalSpeed = inWater ? max_water_vertical_speed : max_vertical_speed;

                if (verticalSpeed < -maxVerticalSpeed)
                    verticalSpeed = -maxVerticalSpeed;
            }

            if (inWater && State.Value == PlayerState.Fall)
                availableJumpCount = 1;

            if (midAir)
            {
                var timeDifference = elapsedFrameTime / 20;
                player.Y -= (float)(verticalSpeed * timeDifference);
                verticalSpeed -= gravity * timeDifference;
            }

            checkBorders();

            if (died || completed)
                return;

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

            checkSpikes();
            checkCherries();
            checkJumpRefresher();
            updatePlayerState();
            checkCompletion();
        }

        private void checkBorders()
        {
            if (drawableRoom.Room.RoomCompletionType == RoomCompletionType.Warp)
            {
                if (player.X - half_width <= 0 || player.X + half_width >= drawableRoom.Size.X
                    || player.Y - half_height <= 0 || player.Y + half_height >= drawableRoom.Size.Y)
                    onDeath();
            }
            else
            {
                if (player.X <= 0 || player.X >= drawableRoom.Size.X || player.Y <= 0 || player.Y >= drawableRoom.Size.Y)
                {
                    completed = true;
                    Completed?.Invoke();
                }
            }
        }

        private void checkSpikes()
        {
            foreach (var t in drawableRoom.Tiles)
            {
                if (MathExtensions.Distance(PlayerPosition(), t.Position) > 60)
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
            if (died)
                return;

            if (drawableRoom.HasTileAtPixel(PlayerPosition(), TileType.Warp))
            {
                completed = true;
                Completed?.Invoke();
            }
        }

        private void checkCherries()
        {
            if (died)
                return;

            foreach (var t in drawableRoom.Tiles)
            {
                if (MathExtensions.Distance(PlayerPosition(), t.Position) > 55)
                    continue;

                if (t.Tile.Type != TileType.Cherry)
                    continue;

                if (CollisionHelper.CollidedWithCircle(PlayerPosition(), t))
                {
                    onDeath();
                    return;
                }
            }
        }

        private void updateAnimationDirection()
        {
            var newScale = new Vector2(rightwards ? 1 : -1, 1);
            var newX = rightwards ? -1.5f : 1.5f;

            if (animationContainer.Scale != newScale)
                animationContainer.Scale = newScale;

            if (animationContainer.X != newX)
                animationContainer.X = newX;
        }

        private bool checkInWater()
        {
            var leftBottomCornerInWater = drawableRoom.HasTileAtPixel(new Vector2(innerLeftBorderPosition(), innerBottomBorderPosition()), TileType.Water3);
            var rightBottomCornerInWater = drawableRoom.HasTileAtPixel(new Vector2(innerRightBorderPosition(), innerBottomBorderPosition()), TileType.Water3);
            var leftTopCornerInWater = drawableRoom.HasTileAtPixel(new Vector2(innerLeftBorderPosition(), innerTopBorderPosition()), TileType.Water3);
            var rightTopCornerInWater = drawableRoom.HasTileAtPixel(new Vector2(innerRightBorderPosition(), innerTopBorderPosition()), TileType.Water3);

            if (leftBottomCornerInWater || rightBottomCornerInWater || leftTopCornerInWater || rightTopCornerInWater)
                return true;

            return false;
        }

        private void checkRightCollision(double elapsedFrameTime)
        {
            var topTile = drawableRoom.GetTileOfGroupAt(new Vector2(outherRightBorderPosition(), innerTopBorderPosition()), TileGroup.Solid);
            var middleTile = drawableRoom.GetTileOfGroupAt(new Vector2(outherRightBorderPosition(), player.Y), TileGroup.Solid);
            var bottomTile = drawableRoom.GetTileOfGroupAt(new Vector2(outherRightBorderPosition(), innerBottomBorderPosition()), TileGroup.Solid);

            if (topTile != null || middleTile != null || bottomTile != null)
            {
                var closestDrawableTilePosition = Math.Max(topTile?.X ?? double.MinValue, middleTile?.X ?? double.MinValue);
                closestDrawableTilePosition = Math.Max(closestDrawableTilePosition, bottomTile?.X ?? double.MinValue);

                player.X = (float)(closestDrawableTilePosition - half_width);
            }
            else
                player.X += (float)(max_horizontal_speed * elapsedFrameTime);
        }

        private void checkRightKillerBlock()
        {
            if (drawableRoom.HasTileAtPixel(new Vector2(innerRightBorderPosition(), innerTopBorderPosition()), TileType.KillerBlock)
                || drawableRoom.HasTileAtPixel(new Vector2(innerRightBorderPosition(), innerBottomBorderPosition()), TileType.KillerBlock))
                onDeath();
        }

        private void checkLeftCollision(double elapsedFrameTime)
        {
            var topTile = drawableRoom.GetTileOfGroupAt(new Vector2(outherLeftBorderPosition(), innerTopBorderPosition()), TileGroup.Solid);
            var middleTile = drawableRoom.GetTileOfGroupAt(new Vector2(outherLeftBorderPosition(), player.Y), TileGroup.Solid);
            var bottomTile = drawableRoom.GetTileOfGroupAt(new Vector2(outherLeftBorderPosition(), innerBottomBorderPosition()), TileGroup.Solid);

            if (topTile != null || middleTile != null || bottomTile != null)
            {
                var closestDrawableTilePosition = Math.Max(topTile?.X + topTile?.Size.X ?? double.MinValue, middleTile?.X + middleTile?.Size.X ?? double.MinValue);
                closestDrawableTilePosition = Math.Max(closestDrawableTilePosition, bottomTile?.X + bottomTile?.Size.X ?? double.MinValue);

                player.X = (float)(closestDrawableTilePosition + half_width);
            }
            else
                player.X -= (float)(max_horizontal_speed * elapsedFrameTime);
        }

        private void checkLeftKillerBlock()
        {
            if (drawableRoom.HasTileAtPixel(new Vector2(innerLeftBorderPosition(), innerTopBorderPosition()), TileType.KillerBlock)
                || drawableRoom.HasTileAtPixel(new Vector2(innerLeftBorderPosition(), innerBottomBorderPosition()), TileType.KillerBlock))
                onDeath();
        }

        private void checkTopCollision()
        {
            var leftTile = drawableRoom.GetTileOfGroupAt(new Vector2(innerLeftBorderPosition(), outherTopBorderPosition()), TileGroup.Solid);
            var rightTile = drawableRoom.GetTileOfGroupAt(new Vector2(innerRightBorderPosition(), outherTopBorderPosition()), TileGroup.Solid);

            if (leftTile != null || rightTile != null)
            {
                var closestDrawableTilePosition = Math.Max(leftTile?.Y + leftTile?.Size.Y ?? double.MinValue, rightTile?.Y + rightTile?.Size.Y ?? double.MinValue);
                player.Y = (float)(closestDrawableTilePosition + half_height);
                verticalSpeed = 0;
            }
        }

        private void checkTopKillerBlock()
        {
            if (drawableRoom.HasTileAtPixel(new Vector2(innerLeftBorderPosition(), innerTopBorderPosition()), TileType.KillerBlock)
                || drawableRoom.HasTileAtPixel(new Vector2(innerRightBorderPosition(), innerTopBorderPosition()), TileType.KillerBlock))
                onDeath();
        }

        private void checkBottomCollision()
        {
            var leftTile = drawableRoom.GetTileOfGroupAt(new Vector2(innerLeftBorderPosition(), outherBottomBorderPosition()), TileGroup.Solid);
            var rightTile = drawableRoom.GetTileOfGroupAt(new Vector2(innerRightBorderPosition(), outherBottomBorderPosition()), TileGroup.Solid);

            if (leftTile != null || rightTile != null)
            {
                var closestDrawableTilePosition = Math.Min(leftTile?.Y ?? double.MaxValue, rightTile?.Y ?? double.MaxValue);
                player.Y = (float)(closestDrawableTilePosition - half_height);
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
            if (drawableRoom.HasTileAtPixel(new Vector2(innerLeftBorderPosition(), innerBottomBorderPosition()), TileType.KillerBlock)
                || drawableRoom.HasTileAtPixel(new Vector2(innerRightBorderPosition(), innerBottomBorderPosition()), TileType.KillerBlock))
                onDeath();
        }

        private void checkJumpRefresher()
        {
            if (died)
                return;

            foreach (var t in drawableRoom.Tiles)
            {
                if (MathExtensions.Distance(PlayerPosition(), t.Position) > 50)
                    continue;

                if (t.Tile.Type != TileType.Jumprefresher)
                    continue;

                var refresher = (DrawableJumpRefresher)t;

                if (!refresher.IsActive)
                    continue;

                if (CollisionHelper.CollidedWithCircle(PlayerPosition(), t))
                {
                    refresher.Deactivate();
                    availableJumpCount = 1;
                    return;
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
    }
}
