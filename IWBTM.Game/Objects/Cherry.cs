using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;

namespace IWBTM.Game.Objects
{
    public class Cherry : CompositeDrawable
    {
        public double StartTime { get; set; }

        public double TimePreempt { get; set; }

        private readonly Sprite sprite;
        private readonly Sprite overlay;
        private readonly Sprite branch;

        public Cherry()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(15);
            AddRangeInternal(new[]
            {
                sprite = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both
                },
                overlay = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both
                },
                branch = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Position = new Vector2(1, -1)
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            sprite.Texture = textures.Get("Objects/Cherry/cherry");
            overlay.Texture = textures.Get("Objects/Cherry/cherry-overlay");
            branch.Texture = textures.Get("Objects/Cherry/cherry-branch");

            sprite.Colour = Color4.Red;
        }
    }
}
