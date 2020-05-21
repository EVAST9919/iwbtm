﻿using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osu.Framework.Utils;
using IWBTM.Game.Helpers;

namespace IWBTM.Game.Screens.Play.Death
{
    public class DeathOverlay : CompositeDrawable
    {
        private Box tint;
        private Box blackFlash;
        private Sprite sprite;
        private LetterboxOverlay letterbox;
        private DrawableSample deathSample;
        private Container<DeathParticle> particles;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures)
        {
            RelativeSizeAxes = Axes.Both;

            AddRangeInternal(new Drawable[]
            {
                particles = new Container<DeathParticle>
                {
                    RelativeSizeAxes = Axes.Both
                },
                tint = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Red,
                    Alpha = 0
                },
                blackFlash = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0
                },
                sprite = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.8f),
                    FillMode = FillMode.Fit,
                    Texture = textures.Get("game-over"),
                    Alpha = 0
                },
                letterbox = new LetterboxOverlay(),
                deathSample = new DrawableSample(audio.Samples.Get("death")),
            });
        }

        public void Play(Vector2 position, Vector2 playerSpeed)
        {
            tint.FadeTo(0.6f, 260);
            blackFlash.FadeIn(0.8f).Then().FadeOut(180);
            sprite.Delay(200).FadeIn(600);
            letterbox.Delay(330).FadeIn(700);
            deathSample.Play();

            var time = Clock.CurrentTime;

            float[] xRandoms = MathExtensions.Get(time, 50, -5, 5);
            float[] yRandoms = MathExtensions.Get(time * 2, 50, -2, 5);

            for (int i = 0; i < 50; i++)
            {
                var speedVector = new Vector2(xRandoms[i] + playerSpeed.X, yRandoms[i]);
                var particle = new DeathParticle(position, speedVector);
                particles.Add(particle);
            }
        }

        public void Restore()
        {
            particles.Clear();

            tint.FadeOut();

            blackFlash.ClearTransforms();
            blackFlash.FadeOut();

            sprite.ClearTransforms();
            sprite.FadeOut();

            letterbox.ClearTransforms();
            letterbox.FadeOut();

            deathSample.Stop();
        }
    }
}