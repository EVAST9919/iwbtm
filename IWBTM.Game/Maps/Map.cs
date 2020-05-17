using IWBTM.Game.Playfield;
using System;
using System.Linq;

namespace IWBTM.Game.Maps
{
    public abstract class Map
    {
        private readonly string playfield;

        public Map()
        {
            playfield = CreatePlayfield();

            if (playfield.Length != DefaultPlayfield.TILES_HEIGHT * DefaultPlayfield.TILES_WIDTH)
                throw new IndexOutOfRangeException("Playfield size is incorrect");
        }

        public char GetTileAt(int x, int y)
        {
            if (x >= DefaultPlayfield.TILES_WIDTH || x < 0 || y >= DefaultPlayfield.TILES_HEIGHT || y < 0)
                throw new IndexOutOfRangeException($"Incorrect input parameters: x: {x}, y: {y}");

            return playfield.ElementAt(y * DefaultPlayfield.TILES_WIDTH + x);
        }

        protected abstract string CreatePlayfield();
    }
}
