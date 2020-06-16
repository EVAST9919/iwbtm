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
            new MenuItem("Delete", OnDelete),
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

        public void OnDelete()
        {
            confirmationOverlay.Push($"Are you sure you want to delete \"{LevelName}\" level?", () => Deleted?.Invoke(this));
        }

        public bool IsSelected;

        public void Select()
        {
            Selected?.Invoke(this);
            background.Activate();
            IsSelected = true;
        }

        public void Deselect()
        {
            IsSelected = false;
            background.Deactivate();
        }

        protected override bool OnClick(ClickEvent e)
        {
            base.OnClick(e);
            Select();
            return true;
        }
    }
}
