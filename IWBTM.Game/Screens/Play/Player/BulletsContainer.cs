using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace IWBTM.Game.Screens.Play.Player
{
    public class BulletsContainer : CompositeDrawable
    {
        public Action OnSave;

        private readonly Vector2 roomSize;

        public BulletsContainer(Vector2 roomSize)
        {
            this.roomSize = roomSize;
            Size = roomSize;
        }

        public void GenerateBullet(Vector2 position, bool rightwards)
        {
            AddInternal(new Bullet(rightwards, roomSize)
            {
                Position = position,
                OnSave = OnSave
            });
        }
    }
}
