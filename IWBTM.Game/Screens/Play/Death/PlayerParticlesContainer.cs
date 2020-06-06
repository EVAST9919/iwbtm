using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Sprites;
using osuTK;
using IWBTM.Game.Helpers;
using IWBTM.Game.Rooms.Drawables;
using System;

namespace IWBTM.Game.Screens.Play.Death
{
    public class PlayerParticlesContainer : CompositeDrawable
    {
        private Container<DeathParticle> particles;
        private Sprite circle;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            RelativeSizeAxes = Axes.Both;

            AddRangeInternal(new Drawable[]
            {
                particles = new Container<DeathParticle>
                {
                    RelativeSizeAxes = Axes.Both
                },
                circle = new Sprite
                {
                    Size = new Vector2(DrawableTile.SIZE * 5),
                    Origin = Anchor.Centre,
                    Scale = Vector2.Zero,
                    Alpha = 0,
                    AlwaysPresent = true,
                    Texture = textures.Get("death-circle")
                },
            });
        }

        public void Play(Vector2 position, Vector2 playerSpeed)
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < 50; i++)
            {
                var speedVector = new Vector2(MathExtensions.Map((float)rand.NextDouble(), 0, 1, -7, 7) + playerSpeed.X, MathExtensions.Map((float)rand.NextDouble(), 0, 1, -5, 5));
                var particle = new DeathParticle(position, speedVector);
                particles.Add(particle);
            }

            circle.Position = position;
            circle.FadeIn().Delay(250).FadeOut(750, Easing.Out);
            circle.Colour = Color4.White;
            circle.FadeColour(Color4.Red, 1000, Easing.Out);
            circle.ScaleTo(1, 1000, Easing.Out);
        }

        public void Restore()
        {
            particles.Clear();

            circle.ClearTransforms();
            circle.ScaleTo(0);
            circle.FadeOut();
        }
    }
}
