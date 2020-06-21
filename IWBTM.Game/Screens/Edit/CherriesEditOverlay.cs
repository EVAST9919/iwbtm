using IWBTM.Game.Helpers;
using IWBTM.Game.Overlays;
using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IWBTM.Game.Screens.Edit
{
    public class CherriesEditOverlay : IWannaOverlay
    {
        public Action<List<DrawableTile>> PreviewUpdated;
        public Action<List<DrawableTile>> CherriesAdded;

        public string Skin { get; set; }

        protected override float Dim => 0;

        [Resolved]
        private NotificationOverlay notifications { get; set; }

        private readonly Container content;
        private readonly IWannaTextBox count;
        private readonly IWannaTextBox distance;
        private readonly IWannaTextBox originX;
        private readonly IWannaTextBox originY;
        private readonly IWannaTextBox angleOffset;
        private readonly IWannaTextBox rotationDuration;

        private List<DrawableTile> cherries;

        public CherriesEditOverlay()
        {
            AddRange(new Drawable[]
            {
                content = new Container
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    RelativeSizeAxes = Axes.Y,
                    Width = 200,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = IWannaColour.GrayDark
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(10),
                            Child = new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Direction = FillDirection.Vertical,
                                Spacing = new Vector2(0, 10),
                                Children = new Drawable[]
                                {
                                    new SpriteText
                                    {
                                        Text = "Count:"
                                    },
                                    count = new IWannaTextBox
                                    {
                                        RelativeSizeAxes = Axes.X
                                    },
                                    new SpriteText
                                    {
                                        Text = "Distance:"
                                    },
                                    distance = new IWannaTextBox
                                    {
                                        RelativeSizeAxes = Axes.X
                                    },
                                    new SpriteText
                                    {
                                        Text = "OriginX:"
                                    },
                                    originX = new IWannaTextBox
                                    {
                                        RelativeSizeAxes = Axes.X
                                    },
                                    new SpriteText
                                    {
                                        Text = "OriginY:"
                                    },
                                    originY = new IWannaTextBox
                                    {
                                        RelativeSizeAxes = Axes.X
                                    },
                                    new SpriteText
                                    {
                                        Text = "Angle offset:"
                                    },
                                    angleOffset = new IWannaTextBox
                                    {
                                        RelativeSizeAxes = Axes.X
                                    },
                                    new SpriteText
                                    {
                                        Text = "Rotation duration:"
                                    },
                                    rotationDuration = new IWannaTextBox
                                    {
                                        RelativeSizeAxes = Axes.X
                                    },
                                    new IWannaBasicButton("Add", onAdd),
                                }
                            }
                        }
                    }
                }
            });

            count.Current.BindValueChanged(_ => updatePreview());
            distance.Current.BindValueChanged(_ => updatePreview());
            originX.Current.BindValueChanged(_ => updatePreview());
            originY.Current.BindValueChanged(_ => updatePreview());
            angleOffset.Current.BindValueChanged(_ => updatePreview());
            rotationDuration.Current.BindValueChanged(_ => updatePreview(), true);
        }

        private void updatePreview()
        {
            int cherriesCount;
            float cherryDistance;
            float originXPos;
            float originYPos;
            float cherryAngleOffset;
            float cherryRotationDuration;

            try
            {
                cherriesCount = int.Parse(count.Current.Value);
                cherryDistance = float.Parse(distance.Current.Value);

                var originXString = originX.Current.Value;
                originXPos = string.IsNullOrEmpty(originXString) ? 0 : float.Parse(originXString);

                var originYString = originY.Current.Value;
                originYPos = string.IsNullOrEmpty(originYString) ? 0 : float.Parse(originYString);

                var angleOffsetString = angleOffset.Current.Value;
                cherryAngleOffset = string.IsNullOrEmpty(angleOffsetString) ? 0 : float.Parse(angleOffsetString);

                var cherryRotationDurationString = rotationDuration.Current.Value;
                cherryRotationDuration = string.IsNullOrEmpty(cherryRotationDurationString) ? 0 : float.Parse(cherryRotationDurationString);
            }
            catch
            {
                cherries = new List<DrawableTile>();
                PreviewUpdated?.Invoke(new List<DrawableTile>());
                return;
            }

            if (cherriesCount <= 0 || cherryDistance <= 0)
                return;

            createCherries(cherriesCount, cherryDistance, cherryAngleOffset, new Vector2(originXPos, originYPos), cherryRotationDuration);
        }

        private void createCherries(int count, float distance, float cherryAngleOffset, Vector2 origin, float cherryRotationDuration)
        {
            cherries = new List<DrawableTile>();

            for (int i = 0; i < count; i++)
            {
                var angle = (float)i / count * 360f + cherryAngleOffset;
                var position = MathExtensions.GetRotatedPosition(origin, distance, angle) - Vector2.Divide(DrawableTile.GetSize(TileType.Cherry), new Vector2(2));

                var tile = new Tile
                {
                    Type = TileType.Cherry,
                    PositionX = (int)position.X,
                    PositionY = (int)position.Y,
                };

                if (cherryRotationDuration != 0)
                {
                    tile.Action = new TileAction
                    {
                        Type = TileActionType.Rotation,
                        Time = cherryRotationDuration,
                        EndX = origin.X,
                        EndY = origin.Y
                    };
                }

                cherries.Add(new DrawableTile(tile, Skin, false));
            }

            PreviewUpdated?.Invoke(cherries);
        }

        private void onAdd()
        {
            if (!cherries.Any())
            {
                notifications?.Push("Nothing to add", NotificationState.Bad);
                return;
            }

            CherriesAdded?.Invoke(cherries);
            Hide();
        }

        protected override void PopIn()
        {
            base.PopIn();
            content.MoveToX(0, 200, Easing.Out);
            content.FadeIn(200, Easing.Out);
            updatePreview();
        }

        protected override void PopOut()
        {
            base.PopOut();
            content.MoveToX(200, 200, Easing.Out);
            content.FadeOut(200, Easing.Out);
            PreviewUpdated?.Invoke(new List<DrawableTile>());
        }
    }
}
