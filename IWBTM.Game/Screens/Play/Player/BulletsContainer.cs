using IWBTM.Game.Rooms;
using IWBTM.Game.Screens.Play.Playfield;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace IWBTM.Game.Screens.Play.Player
{
    public class BulletsContainer : CompositeDrawable
    {
        public Action OnSave;

        private readonly Room room;

        public BulletsContainer(Room room)
        {
            this.room = room;

            Size = DefaultPlayfield.BASE_SIZE;
        }

        public void GenerateBullet(Vector2 position, bool rightwards)
        {
            AddInternal(new Bullet(room, rightwards)
            {
                Position = position,
                OnSave = OnSave
            });
        }
    }
}
