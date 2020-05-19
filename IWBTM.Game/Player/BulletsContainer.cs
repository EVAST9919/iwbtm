using IWBTM.Game.Playfield;
using IWBTM.Game.Rooms;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace IWBTM.Game.Player
{
    public class BulletsContainer : CompositeDrawable
    {
        private readonly Room room;

        public BulletsContainer(Room room)
        {
            this.room = room;

            Size = DefaultPlayfield.BASE_SIZE;
        }

        public void GenerateBullet(Vector2 position, bool rightwards)
        {
            AddInternal(new Bullet(rightwards)
            {
                Position = position
            });
        }
    }
}
