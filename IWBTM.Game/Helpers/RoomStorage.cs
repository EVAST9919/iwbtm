using IWBTM.Game.Rooms;
using Newtonsoft.Json;
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

        public static void DeleteRoom(string name)
        {
            Directory.Delete($"Rooms/{name}", true);
        }

        public static void CreateRoom(string name, string music, List<Tile> tiles)
        {
            var file = new Room
            {
                Tiles = tiles,
                Music = music
            };

            string jsonResult = JsonConvert.SerializeObject(file);

            if (!Directory.Exists("Rooms"))
                Directory.CreateDirectory("Rooms");

            Directory.CreateDirectory($"Rooms/{name}");

            using (StreamWriter sw = File.CreateText($"Rooms/{name}/room"))
            {
                sw.WriteLine(jsonResult.ToString());
                sw.Close();
            }
        }
    }
}
