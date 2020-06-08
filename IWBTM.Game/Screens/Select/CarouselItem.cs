using IWBTM.Game.Overlays;
using IWBTM.Game.Rooms;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using System;

namespace IWBTM.Game.Screens.Select
{
    public class CarouselItem : ClickableContainer, IHasContextMenu
    {
        public Action<CarouselItem> Selected;
        public Action<CarouselItem> Deleted;
        public Action<CarouselItem> OnEdit;

        [Resolved]
        private ConfirmationOverlay confirmationOverlay { get; set; }

        public Level Level { get; private set; }
        public string LevelName { get; private set; }

        public MenuItem[] ContextMenuItems => new[]
        {
            new MenuItem("Delete", onDelete),
            new MenuItem("Edit", () => OnEdit?.Invoke(this)),
        };

        private readonly IWannaSelectableButtonBackground background;

        public CarouselItem((Level, string) level)
        {
            Level = level.Item1;
            LevelName = level.Item2;

            RelativeSizeAxes = Axes.X;
            Height = 50;
            AddRangeInternal(new Drawable[]
            {
                background = new IWannaSelectableButtonBackground(),
                new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = level.Item2
                }
            });
        }

        private void onDelete()
        {
            confirmationOverlay.Push("Are you sure you want to delete this level?", () => Deleted?.Invoke(this));
        }

        public void Select()
        {
            Selected?.Invoke(this);
            background.Activate();
        }

        public void Deselect() => background.Deactivate();

        protected override bool OnClick(ClickEvent e)
        {
            base.OnClick(e);
            Select();
            return true;
        }
    }
}
