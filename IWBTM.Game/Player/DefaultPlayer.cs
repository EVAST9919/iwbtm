﻿using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK.Input;
using osu.Framework.Graphics;
using osuTK;
using IWBTM.Game.Playfield;
using System.Collections.Generic;
using System;
using osu.Framework.Bindables;

namespace IWBTM.Game.Player
{
    public class DefaultPlayer : CompositeDrawable
    {
        private const double base_speed = 1.0 / 6.5;

        private readonly Bindable<PlayerState> state = new Bindable<PlayerState>(PlayerState.Idle);

        private int horizontalDirection;
        private int availableJumpCount = 2;
        private float verticalSpeed;
        private bool midAir;

        private DrawableSample jump;
        private DrawableSample doubleJump;

        private readonly List<Tile> tiles;
        private readonly Container spritesContainer;

        public DefaultPlayer(List<Tile> tiles)
        {
            this.tiles = tiles;

            Origin = Anchor.Centre;
            Size = new Vector2(15);
            AddInternal(spritesContainer = new Container
            {
                RelativeSizeAxes = Axes.Both
            });
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            AddRangeInternal(new[]
            {
                jump = new DrawableSample(audio.Samples.Get("jump")),
                doubleJump = new DrawableSample(audio.Samples.Get("double-jump"))
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            SetDefaultPosition();
            state.BindValueChanged(onStateChanged, true);
        }

        public void SetDefaultPosition()
        {
            Position = new Vector2(260, 360 - 7.5f);
            Scale = Vector2.One;
        }

        protected override void Update()
        {
            base.Update();

            if (Y > (DefaultPlayfield.HEIGHT - 1) * Tile.SIZE - DrawHeight / 2f)
            {
                resetJumpLogic();
                Y = (DefaultPlayfield.HEIGHT - 1) * Tile.SIZE - DrawHeight / 2f;
            }

            if (midAir)
            {
                verticalSpeed -= (float)Clock.ElapsedFrameTime / 3.5f;

                // Limit maximum falling speed
                if (verticalSpeed < -100)
                    verticalSpeed = -100;

                Y -= (float)(Clock.ElapsedFrameTime * verticalSpeed * 0.0045);
            }


            // Horizontal movement
            if (horizontalDirection != 0)
            {
                Scale = new Vector2(horizontalDirection > 0 ? 1 : -1, 1);

                var newX = X + Math.Sign(horizontalDirection) * Clock.ElapsedFrameTime * base_speed;

                // Moving left
                if (horizontalDirection < 0)
                {
                    if (newX < Tile.SIZE + DrawWidth / 2f)
                    {
                        newX = Tile.SIZE + DrawWidth / 2f;
                    }
                }
                else // Moving right
                {
                    if (newX > DefaultPlayfield.WIDTH * Tile.SIZE - Tile.SIZE - DrawWidth / 2f)
                    {
                        newX = DefaultPlayfield.WIDTH * Tile.SIZE - Tile.SIZE - DrawWidth / 2f;
                    }
                }
                
                X = (float)newX;
            }

            updatePlayerState();
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
                    verticalSpeed = 90;
                    break;

                case 0:
                    doubleJump.Play();
                    verticalSpeed = 80;
                    break;
            }
        }

        private void onJumpReleased()
        {
            if (verticalSpeed < 0)
                return;

            verticalSpeed /= 2;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Right:
                        horizontalDirection++;
                        return true;

                    case Key.Left:
                        horizontalDirection--;
                        return true;

                    case Key.ShiftLeft:
                        onJumpPressed();
                        return true;
                }
            };

            return base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    horizontalDirection++;
                    return;

                case Key.Right:
                    horizontalDirection--;
                    return;

                case Key.ShiftLeft:
                    onJumpReleased();
                    return;
            }

            base.OnKeyUp(e);
        }

        private void onStateChanged(ValueChangedEvent<PlayerState> s) => spritesContainer.Child = new PlayerAnimation(s.NewValue);

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
