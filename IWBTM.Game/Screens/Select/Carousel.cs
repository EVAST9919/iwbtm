using IWBTM.Game.Helpers;
using IWBTM.Game.Overlays;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;
using System.Linq;

namespace IWBTM.Game.Screens.Select
{
    public class Carousel : CompositeDrawable
    {
        public readonly Bindable<CarouselItem> Current = new Bindable<CarouselItem>();

        public Action<CarouselItem> OnEdit;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        private FillFlowContainer<CarouselItem> flow;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Padding = new MarginPadding(10);

            AddInternal(new IWannaScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = flow = new FillFlowContainer<CarouselItem>
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10)
                }
            });
        }

        public void UpdateItems()
        {
            flow.Clear();

            foreach (var l in LevelStorage.GetLevels())
            {
                flow.Add(new CarouselItem(l)
                {
                    Selected = onSelection,
                    OnEdit = editRequested,
                    Deleted = deleteRequested,
                });
            }

            selectFirst();
        }

        public void TrySelectNext()
        {
            if (Current.Value == flow.Children.Last())
                return;

            for (int i = 0; i < flow.Count; i++)
            {
                if (Current.Value == flow.Children[i])
                {
                    flow.Children[i + 1].Select();
                    return;
                }
            }
        }

        public void TrySelectPrev()
        {
            if (Current.Value == flow.Children.First())
                return;

            for (int i = 0; i < flow.Count; i++)
            {
                if (Current.Value == flow.Children[i])
                {
                    flow.Children[i - 1].Select();
                    return;
                }
            }
        }

        public void TryDelete()
        {
            flow.ForEach(c =>
            {
                if (c.IsSelected)
                {
                    c.OnDelete();
                    return;
                }
            });
        }

        private void deleteRequested(CarouselItem item)
        {
            var name = item.LevelName;

            LevelStorage.DeleteLevel(name);
            notifications.Push($"{name} level has been deleted!", NotificationState.Good);
            selectFirst();
            item.Expire();
        }

        private void editRequested(CarouselItem item)
        {
            OnEdit?.Invoke(item);
        }

        private void onSelection(CarouselItem item)
        {
            Current.Value = item;

            flow.Children.ForEach(i =>
            {
                if (i != item)
                    i.Deselect();
            });
        }

        private void selectFirst()
        {
            if (!flow.Any())
            {
                Current.Value = null;
                return;
            }

            flow.Children.FirstOrDefault().Select();
        }
    }
}
