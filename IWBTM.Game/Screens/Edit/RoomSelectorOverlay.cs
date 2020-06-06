using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Create;
using IWBTM.Game.UserInterface;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using System;
using System.Linq;

namespace IWBTM.Game.Screens.Edit
{
    public class RoomSelectorOverlay : IWannaOverlay
    {
        private readonly FillFlowContainer<FlowItem> flow;
        private readonly RoomCreationOverlay roomCreationOverlay;

        public readonly BindableList<Room> Rooms = new BindableList<Room>();
        public readonly Bindable<Room> Selected = new Bindable<Room>();

        public RoomSelectorOverlay()
        {
            AddRange(new Drawable[]
            {
                flow = new FillFlowContainer<FlowItem>
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10)
                },
                roomCreationOverlay = new RoomCreationOverlay()
            });

            roomCreationOverlay.CreatedRoom += onCreation;
        }

        public new void Show()
        {
            resetRooms();
            base.Show();
        }

        private void resetRooms()
        {
            flow.Clear();

            for (int i = 0; i < Rooms.Count; i++)
            {
                var correspondingRoom = Rooms.ElementAt(i);
                flow.Add(new RoomItem(i, correspondingRoom, () => updateSelections(correspondingRoom))
                {
                    Selected = Selected.Value == correspondingRoom
                });
            };

            flow.Add(new CreationButton(roomCreationOverlay.Show));
        }

        private void updateSelections(Room selected)
        {
            Selected.Value = selected;

            flow.Children.OfType<RoomItem>().ForEach(c =>
            {
                c.Selected = c.Room == selected;
            });
        }

        private void onCreation(Room room)
        {
            Rooms.Add(room);
            resetRooms();
        }

        protected override void PopIn()
        {
            base.PopIn();
            flow.FadeIn(200, Easing.Out);
        }

        protected override void PopOut()
        {
            base.PopOut();
            flow.FadeOut(200, Easing.Out);
        }

        private class FlowItem : ClickableContainer
        {
            private readonly IWannaSelectableButtonBackground bg;

            public FlowItem(string text, Action action)
            {
                Size = new Vector2(200, 50);
                AddRangeInternal(new Drawable[]
                {
                    bg = new IWannaSelectableButtonBackground(),
                    new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = text
                    }
                });

                Action = action;
            }

            protected void Activate() => bg.Activate();

            protected void Deactivate() => bg.Deactivate();
        }

        private class CreationButton : FlowItem
        {
            public CreationButton(Action action)
                : base("new", action)
            {
            }
        }

        private class RoomItem : FlowItem
        {
            private bool selected;

            public bool Selected
            {
                get => selected;
                set
                {
                    selected = value;
                    updateSelection();
                }
            }

            public readonly Room Room;

            public RoomItem(int index, Room room, Action action)
                : base($"room-{index.ToString()}", action)
            {
                Room = room;
            }

            private void updateSelection()
            {
                if (Selected)
                    Activate();
                else
                    Deactivate();
            }
        }
    }
}
