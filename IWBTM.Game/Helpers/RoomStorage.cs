using IWBTM.Game.Rooms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace IWBTM.Game.Helpers
{
    public class RoomStorage
    {
        public static List<Room> GetRooms()
        {
            List<Room> rooms = new List<Room>();

            if (!Directory.Exists("Rooms"))
                Directory.CreateDirectory("Rooms");

            foreach (var file in Directory.GetFiles("Rooms"))
            {
                using (StreamReader sr = File.OpenText(file))
                {
                    var text = sr.ReadLine();
                    sr.Close();

                    rooms.Add(JsonConvert.DeserializeObject<Room>(text));
                }
            }

            return rooms;
        }

        public static void DeleteRoom(string name)
        {
            File.Delete($"Rooms/{name}");
        }

        public static void CreateRoom(string filename, List<Tile> tiles)
        {
            var file = new Room
            {
                Tiles = tiles,
                Name = filename,
            };

            string jsonResult = JsonConvert.SerializeObject(file);

            if (!Directory.Exists("Rooms"))
                Directory.CreateDirectory("Rooms");

            using (StreamWriter sw = File.CreateText($"Rooms/{filename}"))
            {
                sw.WriteLine(jsonResult.ToString());
                sw.Close();
            }
        }
    }
}
