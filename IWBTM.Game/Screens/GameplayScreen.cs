using IWBTM.Game.Playfield;
using IWBTM.Game.Rooms;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace IWBTM.Game.Screens
{
    public class GameplayScreen : GameScreen
    {
        private readonly BindableBool showHitbox = new BindableBool(false);

        public GameplayScreen(Room room)
        {
            AddRangeInternal(new Drawable[]
            {
                new PlayfieldAdjustmentContainer
                {
                    Child = new DefaultPlayfield(room)
                    {
                        ShowHitbox = { BindTarget = showHitbox }
                    }
                },
                new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Padding = new MarginPadding(10),
                    Child = new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 5),
                        Children = new Drawable[]
                        {
                            new BasicCheckbox
                            {
                                LabelText = "Show hitbox",
                                Current = showHitbox
                            }
                        }
                    }
                }
            });
        }
    }
}
