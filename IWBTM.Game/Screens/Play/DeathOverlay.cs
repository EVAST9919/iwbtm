using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Allocation;
using osu.Framework.Audio;

namespace IWBTM.Game.Screens.Play
{
    public class DeathOverlay : CompositeDrawable
    {
        private readonly Box tint;
        private DrawableSample deathSample;

        public DeathOverlay()
        {
            RelativeSizeAxes = Axes.Both;

            AddRangeInternal(new Drawable[]
            {
                tint = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Red,
                    Alpha = 0
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            AddRangeInternal(new[]
            {
                deathSample = new DrawableSample(audio.Samples.Get("death")),
            });
        }

        public void Play()
        {
            tint.FadeTo(0.5f, 1000, Easing.OutQuint);
            deathSample.Play();
        }

        public void Restore()
        {
            tint.FadeOut();
            deathSample.Stop();
        }
    }
}
