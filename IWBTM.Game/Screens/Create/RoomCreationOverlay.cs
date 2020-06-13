using IWBTM.Game.Overlays;
using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
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
        protected readonly MusicSelector MusicSelector;
        protected readonly SkinSelector SkinSelector;
        protected readonly SizeSetting SizeSetting;
        private readonly SizeAdjustmentOverlay sizeAdjustmentOverlay;
        private readonly BasicCheckbox createBorders;

        public RoomCreationOverlay(bool showCreationSettings = true)
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
                                SizeSetting = new SizeSetting
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                },
                                createBorders = new BasicCheckbox
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    LabelText = "Create borders",
                                    Alpha = showCreationSettings ? 1 : 0
                                },
                                new SettingName("Skin"),
                                new Container
                                {
                                    Width = 300,
                                    Height = 40,
                                    Anchor = Anchor.Centre,
                                    Depth = -int.MaxValue,
                                    Origin = Anchor.Centre,
                                    Child = SkinSelector = new SkinSelector()
                                },
                                new SettingName("Audio"),
                                new Container
                                {
                                    Width = 300,
                                    Height = 40,
                                    Anchor = Anchor.Centre,
                                    Depth = -int.MaxValue,
                                    Origin = Anchor.Centre,
                                    Child = MusicSelector = new MusicSelector()
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

            SizeSetting.AdjustRequested += onSizeAdjust;
            sizeAdjustmentOverlay.NewSize += newSize => SizeSetting.Current.Value = newSize;
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

        public Room GenerateRoom()
        {
            var xSize = SizeSetting.Current.Value.X;
            var ySize = SizeSetting.Current.Value.Y;

            var tiles = new List<Tile>();

            if (createBorders.Current.Value)
            {
                tiles.AddRange(new Tile[]
                {
                    new Tile
                    {
                        Type = TileType.PlatformCorner,
                        PositionX = 0,
                        PositionY = 0
                    },
                    new Tile
                    {
                        Type = TileType.PlatformCorner,
                        PositionX = ((int)xSize - 1) * DrawableTile.SIZE,
                        PositionY = 0
                    },
                    new Tile
                    {
                        Type = TileType.PlatformCorner,
                        PositionX = 0,
                        PositionY = ((int)ySize - 1) * DrawableTile.SIZE
                    },
                    new Tile
                    {
                        Type = TileType.PlatformCorner,
                        PositionX = ((int)xSize - 1) * DrawableTile.SIZE,
                        PositionY = ((int)ySize - 1) * DrawableTile.SIZE
                    }
                });

                for (int i = 1; i < (int)xSize - 1; i++)
                {
                    tiles.AddRange(new Tile[]
                    {
                        new Tile
                        {
                            PositionX = i * DrawableTile.SIZE,
                            PositionY = 0,
                            Type = TileType.PlatformMiddle
                        },
                        new Tile
                        {
                            PositionX = i * DrawableTile.SIZE,
                            PositionY = ((int)ySize - 1) * DrawableTile.SIZE,
                            Type = TileType.PlatformMiddle
                        }
                    });
                }

                for (int i = 1; i < (int)ySize - 1; i++)
                {
                    tiles.AddRange(new Tile[]
                    {
                        new Tile
                        {
                            PositionX = 0,
                            PositionY = i * DrawableTile.SIZE,
                            Type = TileType.PlatformMiddleRotated
                        },
                        new Tile
                        {
                            PositionX = ((int)xSize - 1) * DrawableTile.SIZE,
                            PositionY = i * DrawableTile.SIZE,
                            Type = TileType.PlatformMiddleRotated
                        }
                    });
                }
            }

            return new Room
            {
                Music = MusicSelector.Current.Value,
                Skin = SkinSelector.Current.Value,
                Tiles = tiles,
                SizeX = xSize,
                SizeY = ySize
            };
        }

        private void onSizeAdjust()
        {
            sizeAdjustmentOverlay.Show();
        }

        private void onCommit()
        {
            if (!canBeCreated())
                return;

            Commit();
            Hide();
        }

        protected virtual void Commit()
        {
            CreatedRoom?.Invoke(GenerateRoom());
        }

        private bool canBeCreated()
        {
            if (SizeSetting.Current.Value.X < 4 || SizeSetting.Current.Value.Y < 4)
            {
                notifications.Push("Width and Height can't be below 4", NotificationState.Bad);
                return false;
            }

            return true;
        }
    }
}
