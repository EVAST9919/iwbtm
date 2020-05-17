using IWBTM.Game.Playfield;
using osuTK;
using System;
using System.Linq;

namespace IWBTM.Game.Rooms
{
    public abstract class Room
    {
        private readonly string layout;

        public Room()
        {
            layout = CreateLayout();

            if (layout.Length != DefaultPlayfield.TILES_HEIGHT * DefaultPlayfield.TILES_WIDTH)
                throw new IndexOutOfRangeException("Playfield size is incorrect");
        }

        public char GetTileAt(int x, int y)
        {
            if (x >= DefaultPlayfield.TILES_WIDTH || x < 0 || y >= DefaultPlayfield.TILES_HEIGHT || y < 0)
                throw new IndexOutOfRangeException($"Incorrect input parameters: x: {x}, y: {y}");

            return layout.ElementAt(y * DefaultPlayfield.TILES_WIDTH + x);
        }

        public static bool TileIsEmpty(char c) => c == ' ';

        protected abstract string CreateLayout();

        public abstract Vector2 GetPlayerSpawnPosition();
    }
}
