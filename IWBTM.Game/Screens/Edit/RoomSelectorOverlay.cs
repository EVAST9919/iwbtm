using IWBTM.Game.Overlays;
using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Create;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using System;
using System.Linq;

namespace IWBTM.Game.Screens.Edit
{
    public class RoomSelectorOverlay : IWannaOverlay
    {
        [Resolved]
        private NotificationOverlay notifications { get; set; }

        [Resolved]
        private ConfirmationOverlay confirmation { get; set; }

        private readonly FillFlowContainer<FlowItem> flow;
        private readonly RoomCreationOverlay roomCreationOverlay;

        public readonly BindableList<Room> Rooms = new BindableList<Room>();
        public readonly Bindable<Room> Selected = new Bindable<Room>();

        public RoomSelectorOverlay()
        {
            AddRange(new Drawable[]
            {
                new BasicContextMenuContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = new Container
                    {
                        Padding = new MarginPadding(10),
                        RelativeSizeAxes = Axes.Both,
                        Child = new IWannaScrollContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = flow = new FillFlowContainer<FlowItem>
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Direction = FillDirection.Full,
                                Spacing = new Vector2(10)
                            }
                        }
                    }
                },
                roomCreationOverlay = new RoomCreationOverlay()
            });

            roomCreationOverlay.CreatedRoom += onCreation;
        }

        private void resetRooms()
        {
            flow.Clear();

            for (int i = 0; i < Rooms.Count; i++)
            {
                var correspondingRoom = Rooms.ElementAt(i);
                flow.Add(new RoomItem(i, correspondingRoom, () => updateSelections(correspondingRoom))
                {
                    Selected = Selected.Value == correspondingRoom,
                    DeleteRequested = tryDelete
                });
            };

            flow.Add(new CreationButton(roomCreationOverlay.Show));
        }

        private void tryDelete(RoomItem item)
        {
            if (Rooms.Count == 1)
            {
                notifications.Push("You can't delete the only one room in the level", NotificationState.Bad);
                return;
            }

            confirmation.Push("Are you sure you want to delete selected room?", () => confirmDeletion(item));
        }

        private void tryDelete()
        {
            flow.Children.OfType<RoomItem>().ForEach(c =>
            {
                if (c.Selected)
                {
                    tryDelete(c);
                    return;
                }
            });
        }

        private void confirmDeletion(RoomItem item)
        {
            var removeableRoom = item.Room;
            var index = Rooms.IndexOf(removeableRoom);
            var newIndex = index == 0 ? 1 : index - 1;
            Selected.Value = Rooms[newIndex];
            Rooms.Remove(removeableRoom);

            resetRooms();
            notifications.Push("Selected room has been deleted", NotificationState.Good);
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
            Selected.Value = room;
            resetRooms();
        }

        protected override void PopIn()
        {
            base.PopIn();
            resetRooms();
            flow.FadeIn(200, Easing.Out);
        }

        protected override void PopOut()
        {
            base.PopOut();
            flow.FadeOut(200, Easing.Out);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Delete:
                        tryDelete();
                        return true;
                }
            }

            return base.OnKeyDown(e);
        }

        private class FlowItem : ClickableContainer
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

            protected override Container<Drawable> Content => content;

            private readonly Container textContainer;
            private readonly Container content;

            public FlowItem(string text, Action action)
            {
                Size = new Vector2(200);
                BorderColour = IWannaColour.Blue;
                Masking = true;
                CornerRadius = 5;
                AddRangeInternal(new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black,
                        Alpha = 0.5f
                    },
                    content = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    },
                    textContainer = new Container
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        RelativeSizeAxes = Axes.X,
                        Height = 40,
                        Y = 40,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.Black,
                                Alpha = 0.5f
                            },
                            new SpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Text = text,
                            }
                        }
                    }
                });

                Action = action;
            }

            protected override bool OnHover(HoverEvent e)
            {
                BorderThickness = 5;
                textContainer.MoveToY(0, 100, Easing.Out);
                content.ScaleTo(1.1f, 100, Easing.Out);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                base.OnHoverLost(e);
                if (!selected)
                    BorderThickness = 0;

                textContainer.MoveToY(40, 100, Easing.Out);
                content.ScaleTo(1, 100, Easing.Out);
            }

            protected void Activate() => BorderThickness = 5;

            protected void Deactivate()
            {
                if (!IsHovered)
                    BorderThickness = 0;
            }

            private void updateSelection()
            {
                if (Selected)
                    Activate();
                else
                    Deactivate();
            }
        }

        private class CreationButton : FlowItem
        {
            public CreationButton(Action action)
                : base("new", action)
            {
                Add(new SpriteIcon
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(100),
                    Icon = FontAwesome.Solid.Plus
                });
            }
        }

        private class RoomItem : FlowItem, IHasContextMenu
        {
            public Action<RoomItem> DeleteRequested;
            public readonly Room Room;

            public RoomItem(int index, Room room, Action action)
                : base($"room-{index.ToString()}", action)
            {
                Room = room;

                Add(new FullRoomPreviewContainer(new Vector2(room.SizeX, room.SizeY))
                {
                    Child = new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both
                            },
                            new DrawableRoom(room, true)
                        }
                    }
                });
            }

            public MenuItem[] ContextMenuItems => new[]
            {
                new MenuItem("Delete", () => DeleteRequested?.Invoke(this))
            };
        }
    }
}
