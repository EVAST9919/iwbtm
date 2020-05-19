using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Select;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace IWBTM.Game.Screens
{
    public class SelectScreen : GameScreen
    {
        private readonly RoomPreviewContainer preview;

        private Room selected;

        public SelectScreen()
        {
            AddRangeInternal(new Drawable[]
            {
                new Carousel
                {
                    Width = 0.5f,
                    OnSelection = roomSelected
                },
                preview = new RoomPreviewContainer
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Width = 0.5f
                },
                new PlayButton
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Action = () =>
                    {
                        if (selected != default)
                            this.Push(new GameplayScreen(selected));
                    }
                }
            });
        }

        private void roomSelected(Room room)
        {
            selected = room;
            preview.Preview(room);
        }

        private class PlayButton : ClickableContainer
        {
            public PlayButton()
            {
                Size = new Vector2(100, 50);
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = "Play",
                        Colour = Color4.Black
                    }
                };
            }
        }
    }
}
