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
        private readonly List<(Vector2, int)> deathSpots;
        private readonly Level level;
        private readonly string time;

        public ResultsScreen(List<(Vector2, int)> deathSpots, Level level, string time)
        {
            this.deathSpots = deathSpots;
            this.level = level;
            this.time = time;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            LevelPreviewContainer preview;

            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(10),
                Children = new Drawable[]
                {
                    preview = new LevelPreviewContainer
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
                            },
                            new SpriteText
                            {
                                Text = $"Time: {time}",
                                Font = FontUsage.Default.With(size: 40)
                            }
                        }
                    }
                }
            });

            preview.Preview(level, false, deathSpots);
        }
    }
}
