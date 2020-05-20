using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace IWBTM.Game.Screens.Play
{
    public class DeathOverlay : CompositeDrawable
    {
        private Box tint;
        private Box blackFlash;
        private Sprite sprite;
        private LetterboxOverlay letterbox;
        private DrawableSample deathSample;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures)
        {
            RelativeSizeAxes = Axes.Both;

            AddRangeInternal(new Drawable[]
            {
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

        public void Play()
        {
            tint.FadeTo(0.6f, 260);
            blackFlash.FadeIn(0.8f).Then().FadeOut(180);
            sprite.Delay(200).FadeIn(600);
            letterbox.Delay(330).FadeIn(700);
            deathSample.Play();
        }

        public void Restore()
        {
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
