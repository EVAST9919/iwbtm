using IWBTM.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osuTK.Graphics.ES30;

namespace IWBTM.Game
{
    /// <summary>
    /// Set up the relevant resource stores and texture settings.
    /// </summary>
    public abstract class IWBTMGameBase : osu.Framework.Game
    {
        private TextureStore textures;

        private DependencyContainer dependencies;

        [BackgroundDependencyLoader]
        private void load()
        {
            // Load the assets from our Resources project
            Resources.AddStore(new DllResourceStore(IWBTMResources.ResourceAssembly));

            // To preserve the 8-bit aesthetic, disable texture filtering
            // so they won't become blurry when upscaled
            textures = new TextureStore(Textures, filteringMode: All.Nearest);
            dependencies.Cache(textures);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
    }
}
