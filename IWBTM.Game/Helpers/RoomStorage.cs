using IWBTM.Game.Rooms;
using osuTK;
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
                    var layout = sr.ReadLine();
                    var x = sr.ReadLine();
                    var y = sr.ReadLine();

                    rooms.Add(new Room(file.Substring(6), layout, new Vector2(float.Parse(x), float.Parse(y))));
                }
            }

            return rooms;
        }

        public static void DeleteRoom(string name)
        {
            File.Delete($"Rooms/{name}");
        }

        public static void CreateRoom(string filename, string layout, Vector2 playerPosition)
        {
            if (!Directory.Exists("Rooms"))
                Directory.CreateDirectory("Rooms");

            using (StreamWriter sw = File.CreateText($"Rooms/{filename}"))
            {
                sw.WriteLine(layout);
                sw.WriteLine(playerPosition.X.ToString());
                sw.WriteLine(playerPosition.Y.ToString());
            }
        }
    }
}
