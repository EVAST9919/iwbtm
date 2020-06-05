using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Select;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using System.Collections.Generic;
using osu.Framework.Allocation;

namespace IWBTM.Game.Screens
{
    public class ResultsScreen : IWannaScreen
    {
        private readonly List<Vector2> deathSpots;
        private readonly Room room;

        public ResultsScreen(List<Vector2> deathSpots, Room room)
        {
            this.deathSpots = deathSpots;
            this.room = room;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RoomPreviewContainer preview;

            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(10),
                Children = new Drawable[]
                {
                    preview = new RoomPreviewContainer
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Width = 0.5f
                    },
                    new FillFlowContainer
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 10),
                        Children = new Drawable[]
                        {
                            new SpriteText
                            {
                                Text = $"Deaths: {deathSpots.Count}",
                                Font = FontUsage.Default.With(size: 40)
                            }
                        }
                    }
                }
            });

            preview.Preview(room, false, deathSpots);
        }
    }
}
