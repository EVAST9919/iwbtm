using IWBTM.Resources;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;

namespace IWBTM.Game
{
    /// <summary>
    /// Set up the relevant resource stores and texture settings.
    /// </summary>
    public abstract class IWBTMGameBase : osu.Framework.Game
    {
        private TextureStore textures;
        private PixelTextureStore pixelTextures;
        private LevelAudioManager levelAudioManager;

        private DependencyContainer dependencies;

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config)
        {
            // Load the assets from our Resources project
            Resources.AddStore(new DllResourceStore(IWBTMResources.ResourceAssembly));

            // To preserve the 8-bit aesthetic, disable texture filtering
            // so they won't become blurry when upscaled
            textures = new TextureStore(Textures);
            pixelTextures = new PixelTextureStore(Textures);
            dependencies.Cache(textures);
            dependencies.Cache(pixelTextures);

            var tracks = new ResourceStore<byte[]>();
            tracks.AddStore(new LevelAudioStore());

            levelAudioManager = new LevelAudioManager(Host.AudioThread, tracks, new ResourceStore<byte[]>()) { EventScheduler = Scheduler };
            dependencies.Cache(levelAudioManager);

            config.GetBindable<FrameSync>(FrameworkSetting.FrameSync).Value = FrameSync.Unlimited;

            Host.Window.Title = "I Wanna Be The Mapper";
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
    }
}
