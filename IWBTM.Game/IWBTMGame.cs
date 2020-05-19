using osu.Framework.Screens;
using IWBTM.Game.Screens;
using osu.Framework.Allocation;
using IWBTM.Game.Overlays;

namespace IWBTM.Game
{
    public class IWBTMGame : IWBTMGameBase
    {
        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        [BackgroundDependencyLoader]
        private void load()
        {
            NotificationOverlay notifications;
            dependencies.Cache(notifications = new NotificationOverlay());

            ScreenStack screens = new ScreenStack();
            Add(screens);
            screens.Push(new MainMenuScreen());

            Add(notifications);
        }
    }
}
