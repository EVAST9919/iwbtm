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
        public readonly Bindable<CarouselRoomItem> Current = new Bindable<CarouselRoomItem>();

        public Action<CarouselRoomItem> OnEdit;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        private readonly FillFlowContainer<CarouselRoomItem> flow;

        public Carousel()
        {
            RelativeSizeAxes = Axes.Both;
            Padding = new MarginPadding(10);

            AddInternal(new BasicScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = flow = new FillFlowContainer<CarouselRoomItem>
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
                flow.Add(new CarouselRoomItem(r)
                {
                    Selected = onSelection,
                    OnEdit = editRequested,
                    Deleted = deleteRequested,
                });
            }

            selectFirst();
        }

        private void deleteRequested(CarouselRoomItem item)
        {
            var name = item.RoomName;

            RoomStorage.DeleteRoom(name);
            notifications.Push($"{name} room has been deleted!", NotificationState.Good);
            selectFirst();
            item.Expire();
        }

        private void editRequested(CarouselRoomItem item)
        {
            OnEdit?.Invoke(item);
        }

        private void onSelection(CarouselRoomItem item)
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
