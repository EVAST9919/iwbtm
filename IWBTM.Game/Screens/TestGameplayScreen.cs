using IWBTM.Game.Rooms;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace IWBTM.Game.Screens
{
    public class TestGameplayScreen : GameplayScreen
    {
        private readonly SpriteText xPosition;
        private readonly SpriteText yPosition;

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
                        },
                        xPosition = new SpriteText(),
                        yPosition = new SpriteText(),
                    }
                }
            });
        }

        private Vector2 lastPlayerPosition;

        protected override void Update()
        {
            base.Update();

            var playerPosition = Playfield.Player.PlayerPosition();

            if (playerPosition != lastPlayerPosition)
            {
                lastPlayerPosition = playerPosition;
                xPosition.Text = $"X: {playerPosition.X}";
                yPosition.Text = $"Y: {playerPosition.Y}";
            }
        }
    }
}
