using IWBTM.Game.Overlays;
using IWBTM.Game.Rooms;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using System;
using System.Collections.Generic;

namespace IWBTM.Game.Screens.Create
{
    public class RoomCreationOverlay : IWannaOverlay
    {
        public Action<Room> CreatedRoom;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        private readonly Container content;
        private readonly MusicSelector musicSelector;
        private readonly SizeSetting sizeSetting;
        private readonly SizeAdjustmentOverlay sizeAdjustmentOverlay;

        public RoomCreationOverlay()
        {
            AddRange(new Drawable[]
            {
                content = new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = IWannaColour.GrayDarker
                        },
                        new FillFlowContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 10),
                            Margin = new MarginPadding(10),
                            Children = new Drawable[]
                            {
                                new SettingName("Size"),
                                sizeSetting = new SizeSetting
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                },
                                new SettingName("Audio"),
                                new Container
                                {
                                    Width = 300,
                                    Height = 40,
                                    Anchor = Anchor.Centre,
                                    Depth = -int.MaxValue,
                                    Origin = Anchor.Centre,
                                    Child = musicSelector = new MusicSelector()
                                },
                                new IWannaBasicButton("Ok", onCommit)
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                }
                            }
                        }
                    }
                },
                sizeAdjustmentOverlay = new SizeAdjustmentOverlay(),
            });

            sizeSetting.AdjustRequested += onSizeAdjust;
            sizeAdjustmentOverlay.NewSize += newSize => sizeSetting.Current.Value = newSize;
        }

        protected override void PopIn()
        {
            base.PopIn();
            content.ScaleTo(1, 200, Easing.Out);
            content.FadeIn(200, Easing.Out);
        }

        protected override void PopOut()
        {
            base.PopOut();
            content.ScaleTo(1.1f, 200, Easing.Out);
            content.FadeOut(200, Easing.Out);
        }

        public Room GenerateRoom() => new Room
        {
            Music = musicSelector.Current.Value,
            Tiles = new List<Tile>(),
            SizeX = sizeSetting.Current.Value.X,
            SizeY = sizeSetting.Current.Value.Y
        };

        private void onSizeAdjust()
        {
            sizeAdjustmentOverlay.Show();
        }

        private void onCommit()
        {
            if (!canBeCreated())
                return;

            CreatedRoom?.Invoke(GenerateRoom());
            Hide();
        }

        private bool canBeCreated()
        {
            if (sizeSetting.Current.Value.X < 4 || sizeSetting.Current.Value.Y < 4)
            {
                notifications.Push("Width and Height can't be below 4", NotificationState.Bad);
                return false;
            }

            return true;
        }
    }
}
