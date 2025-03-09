using IWBTM.Game.Screens;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace IWBTM.Game.BossCreation
{
    public partial class BossCreationScreen : IWannaScreen
    {
        private readonly BossLevel level;

        public BossCreationScreen(BossLevel level)
        {
            this.level = level;

            AddInternal(new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Relative, 0.5f),
                    new Dimension(),
                },
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.Relative, 1),
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        Empty()
                    },
                    new Drawable[]
                    {
                        Empty()
                    }
                }
            });
        }
    }
}
