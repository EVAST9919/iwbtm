using IWBTM.Game.Rooms;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace IWBTM.Game.Screens
{
    public class TestGameplayScreen : GameplayScreen
    {
        public TestGameplayScreen(Room room)
            : base(room)
        {
            AddInternal(new Container
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
                            Current = Playfield.Player.ShowHitbox
                        }
                    }
                }
            });
        }
    }
}
