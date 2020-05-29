using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.Rooms.Drawables
{
    public class DrawableCherry : DrawableTile
    {
        private readonly Sprite overlay;
        private readonly Sprite branch;

        public DrawableCherry(Tile tile)
            : base(tile)
        {
            AddRangeInternal(new Drawable[]
            {
                overlay = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                },
                branch = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Position = new Vector2(1, -1)
                }
            });

            MainSprite.Colour = Color4.Red;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            overlay.Texture = textures.Get("Tiles/cherry-overlay");
            branch.Texture = textures.Get("Tiles/cherry-branch");
        }
    }
}
