using IWBTM.Game.Rooms;
using Newtonsoft.Json;
using osuTK;
using System.Collections.Generic;
using System.IO;

namespace IWBTM.Game.Helpers
{
    public class RoomStorage
    {
        public static List<(Room, string)> GetRooms()
        {
            List<(Room, string)> rooms = new List<(Room, string)>();

            if (!Directory.Exists("Rooms"))
            {
                Directory.CreateDirectory("Rooms");
                return null;
            }

            var directories = Directory.GetDirectories("Rooms/");

            if (directories.Length == 0)
                return null;

            foreach (var dir in directories)
            {
                var file = $"{dir}/room";

                if (!File.Exists(file))
                    continue;

                using (StreamReader sr = File.OpenText(file))
                {
                    var text = sr.ReadLine();
                    sr.Close();

                    var room = JsonConvert.DeserializeObject<Room>(text);
                    var name = file.Substring(6, dir.Substring(6).Length);

                    rooms.Add((room, name));
                }
            }

            return rooms;
        }

        public static bool RoomExists(string name)
        {
            if (!Directory.Exists("Rooms"))
            {
                Directory.CreateDirectory("Rooms");
                return false;
            }

            var directories = Directory.GetDirectories("Rooms");

            if (directories.Length == 0)
                return false;

            foreach (var dir in directories)
            {
                if (dir.Substring(6).ToLower() == name.ToLower())
                    return true;
            }

            return false;
        }

        public static bool RoomHasCustomAudio(string name) => File.Exists($"Rooms/{name}/audio.mp3");

        public static void DeleteRoom(string name)
        {
            Directory.Delete($"Rooms/{name}", true);
        }

        public static void CreateRoomDirectory(string name)
        {
            Directory.CreateDirectory($"Rooms/{name}");
        }

        public static Room CreateEmptyRoom(string name, string musicName, Vector2 size)
        {
            var file = new Room
            {
                Music = musicName,
                Tiles = new List<Tile>(),
                SizeX = size.X,
                SizeY = size.Y
            };

            string jsonResult = JsonConvert.SerializeObject(file);

            using (StreamWriter sw = File.CreateText($"Rooms/{name}/room"))
            {
                sw.WriteLine(jsonResult.ToString());
                sw.Close();
            }

            return file;
        }

        public static void UpdateRoomTiles(Room room, string name, List<Tile> tiles)
        {
            var file = new Room
            {
                Music = room.Music,
                Tiles = tiles,
                SizeX = room.SizeX,
                SizeY = room.SizeY
            };

            string jsonResult = JsonConvert.SerializeObject(file);

            using (StreamWriter sw = File.CreateText($"Rooms/{name}/room"))
            {
                sw.WriteLine(jsonResult.ToString());
                sw.Close();
            }
        }
    }
}
