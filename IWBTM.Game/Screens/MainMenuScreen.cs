using IWBTM.Game.Rooms;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace IWBTM.Game.Screens
{
    public class MainMenuScreen : Screen
    {
        public MainMenuScreen()
        {
            AddInternal(new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "IWBTM",
                Font = FontUsage.Default.With(size: 30),
                Y = -100
            });

            AddInternal(new FillFlowContainer<Button>
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(5, 0),
                Children = new[]
                {
                    new Button("Play")
                    {
                        Action = () => this.Push(new SelectScreen())
                    },
                    new Button("Edit")
                    {
                        Action = () => this.Push(new EditorScreen())
                    }
                }
            });
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!e.Repeat)
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        Game.Exit();
                        return true;
                }
            };

            return base.OnKeyDown(e);
        }

        private class Button : ClickableContainer
        {
            public Button(string text)
            {
                Size = new Vector2(100);

                AddRangeInternal(new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    new SpriteText
                    {
                        Text = text,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Color4.Black
                    }
                });
            }
        }
    }
}
