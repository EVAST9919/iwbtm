using osu.Framework.Allocation;
using IWBTM.Game.Playfield;
using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;

namespace IWBTM.Game
{
    public class IWBTMGame : IWBTMGameBase
    {
        private readonly BindableBool showHitbox = new BindableBool(false);

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new PlayfieldAdjustmentContainer
            {
                Child = new DefaultPlayfield
                {
                    ShowHitbox = { BindTarget = showHitbox }
                }
            });

            Add(new Container
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
            });
        }
    }
}
