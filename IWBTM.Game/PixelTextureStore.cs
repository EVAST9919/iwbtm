using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Textures;

namespace IWBTM.Game
{
    public class PixelTextureStore : TextureStore
    {
        public PixelTextureStore(IRenderer renderer)
            : base(renderer, filteringMode: TextureFilteringMode.Nearest)
        {
        }
    }
}
