using IWBTM.Game.Screens.Play.Playfield;
using osuTK;
using System;
using System.Linq;

namespace IWBTM.Game.Rooms
{
    public class Room
    {
        public string Name { get; private set; }

        public Vector2 PlayerSpawnPosition { get; private set; }

        private readonly string layout;

        public Room(string name, string layout, Vector2 playerSpawnPosition)
        {
            Name = name;
            PlayerSpawnPosition = playerSpawnPosition;
            this.layout = layout;

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
    }
}
