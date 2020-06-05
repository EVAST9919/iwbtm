using IWBTM.Game.Helpers;
using IWBTM.Game.Overlays;
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

        private readonly FillFlowContainer<CarouselItem> flow;

        public Carousel()
        {
            RelativeSizeAxes = Axes.Both;
            Padding = new MarginPadding(10);

            AddInternal(new BasicScrollContainer
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

            foreach (var r in RoomStorage.GetRooms())
            {
                flow.Add(new CarouselItem(r)
                {
                    Selected = onSelection,
                    OnEdit = editRequested,
                    Deleted = deleteRequested,
                });
            }

            selectFirst();
        }

        private void deleteRequested(CarouselItem item)
        {
            var name = item.RoomName;

            RoomStorage.DeleteRoom(name);
            notifications.Push($"{name} room has been deleted!", NotificationState.Good);
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
            flow.Children.FirstOrDefault().Select();
        }
    }
}
