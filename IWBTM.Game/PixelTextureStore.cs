using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osuTK.Graphics.ES30;

namespace IWBTM.Game
{
    public class PixelTextureStore : TextureStore
    {
        public PixelTextureStore(IResourceStore<TextureUpload> store)
            : base(store, filteringMode: All.Nearest)
        {
        }
    }
}
