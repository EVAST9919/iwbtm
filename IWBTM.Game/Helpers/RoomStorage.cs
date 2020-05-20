using IWBTM.Game.Rooms;
using osu.Framework.Platform;
using osuTK;
using System.Collections.Generic;
using System.IO;

namespace IWBTM.Game.Helpers
{
    public class RoomStorage
    {
        public static List<Room> GetRooms(Storage storage)
        {
            List<Room> rooms = new List<Room>();

            foreach (var file in storage.GetFiles(""))
            {
                using (StreamReader sr = File.OpenText(storage.GetFullPath(file)))
                {
                    var layout = sr.ReadLine();
                    var x = sr.ReadLine();
                    var y = sr.ReadLine();

                    rooms.Add(new Room(file, layout, new Vector2(float.Parse(x), float.Parse(y))));
                }
            }

            return rooms;
        }

        public static void DeleteRoom(string name, Storage storage)
        {
            File.Delete(storage.GetFullPath(name));
        }

        public static void CreateRoom(Storage storage, string filename, string layout, Vector2 playerPosition)
        {
            using (StreamWriter sw = File.CreateText(storage.GetFullPath(filename)))
            {
                sw.WriteLine(layout);
                sw.WriteLine(playerPosition.X.ToString());
                sw.WriteLine(playerPosition.Y.ToString());
            }
        }
    }
}
