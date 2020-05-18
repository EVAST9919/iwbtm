using IWBTM.Game.Playfield;
using IWBTM.Game.Screens.Edit;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace IWBTM.Game.Screens
{
    public class EditorScreen : GameScreen
    {
        public EditorScreen()
        {
            AddRangeInternal(new Drawable[]
            {
                new PlayfieldAdjustmentContainer
                {
                    Scale = new Vector2(0.9f),
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                        new Grid()
                    }
                },
                new ObjectSelector
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight
                }
            });
        }
    }
}
