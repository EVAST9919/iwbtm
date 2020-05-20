using IWBTM.Game.Helpers;
using IWBTM.Game.Overlays;
using IWBTM.Game.Rooms;
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
        public readonly Bindable<Room> Current = new Bindable<Room>();

        public Action<Room> OnEdit;

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
            flow.AddRange(new[]
            {
                new CarouselRoomItem(new BossRoom())
                {
                    Selected = onSelection,
                    OnEdit = editRequested,
                    Deleted = deleteRequested,
                },
                new CarouselRoomItem(new EmptyRoom())
                {
                    Selected = onSelection,
                    OnEdit = editRequested,
                    Deleted = deleteRequested,
                }
            });

            foreach (var r in RoomStorage.GetRooms())
            {
                flow.Add(new CarouselRoomItem(r, true)
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
            var name = item.Room.Name;

            RoomStorage.DeleteRoom(name);
            notifications.Push($"{name} room has been deleted!", NotificationState.Good);
            selectFirst();
            item.Expire();
        }

        private void editRequested(CarouselRoomItem item)
        {
            OnEdit?.Invoke(item.Room);
        }

        private void onSelection(CarouselRoomItem item)
        {
            Current.Value = item.Room;

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
