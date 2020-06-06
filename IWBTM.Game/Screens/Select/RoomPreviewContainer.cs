using IWBTM.Game.Rooms;
using IWBTM.Game.Rooms.Drawables;
using IWBTM.Game.Screens.Play.Playfield;
using IWBTM.Game.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using System.Collections.Generic;

namespace IWBTM.Game.Screens.Select
{
    public class RoomPreviewContainer : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Padding = new MarginPadding(10);
        }

        public void Preview(Room room, bool showPlayerSpawn = true, List<Vector2> deathSpots = null)
        {
            FullRoomPreviewContainer roomPreview;

            InternalChild = roomPreview = new FullRoomPreviewContainer(new Vector2(room.SizeX, room.SizeY));

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
                foreach (var spot in deathSpots)
                {
                    roomPreview.Add(new DeathSpot
                    {
                        Position = spot
                    });
                }
            }
        }
    }
}
