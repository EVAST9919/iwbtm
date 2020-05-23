using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;

namespace IWBTM.Game.Screens.Select
{
    public class RoomPreviewContainer : CompositeDrawable
    {
        private readonly PlayfieldAdjustmentContainer roomPreview;

        public RoomPreviewContainer()
        {
            RelativeSizeAxes = Axes.Both;
            Padding = new MarginPadding(10);

            AddInternal(roomPreview = new PlayfieldAdjustmentContainer());
        }

        public void Preview(Room room, bool showPlayerSpawn = true, List<Vector2> deathSpots = null)
        {
            roomPreview.Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                new DrawableRoom(room, showPlayerSpawn)
            };

            if (deathSpots != null)
            {
                foreach(var spot in deathSpots)
                {
                    roomPreview.Add(new Circle
                    {
                        Origin = Anchor.Centre,
                        Size = new Vector2(5),
                        Position = spot,
                        Colour = Color4.Red
                    });
                }
            }
        }
    }
}
