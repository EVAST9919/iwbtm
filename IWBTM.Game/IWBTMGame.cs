﻿using osu.Framework.Screens;
using IWBTM.Game.Screens;
using osu.Framework.Allocation;
using IWBTM.Game.Overlays;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics;
using IWBTM.Game.UserInterface;

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

            ConfirmationOverlay confirmationOverlay;
            dependencies.Cache(confirmationOverlay = new ConfirmationOverlay());

            ScreenStack screens = new ScreenStack();
            screens.Push(new MainMenuScreen());

            Add(new BasicContextMenuContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = new IWannaTooltipContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        screens,
                        notifications,
                        confirmationOverlay
                    }
                }
            });
        }
    }
}
