using IWBTM.Game.Rooms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace IWBTM.Game.Helpers
{
    public static class LevelStorage
    {
        public static List<(Level, string)> GetLevels()
        {
            List<(Level, string)> levels = new List<(Level, string)>();

            if (!checkMainDirectoryExistance())
                return levels;

            var directories = Directory.GetDirectories("Levels/");

            if (directories.Length == 0)
                return levels;

            foreach (var dir in directories)
            {
                var file = $"{dir}/level";

                if (!File.Exists(file))
                    continue;

                using (StreamReader sr = File.OpenText(file))
                {
                    var text = sr.ReadLine();
                    sr.Close();

                    var level = JsonConvert.DeserializeObject<Level>(text);
                    var name = file.Substring(7, dir.Substring(7).Length);

                    levels.Add((level, name));
                }
            }

            return levels;
        }

        public static Level CreateEmptyLevel(string name, List<Room> rooms)
        {
            var file = new Level
            {
                Rooms = rooms
            };

            string jsonResult = JsonConvert.SerializeObject(file);

            using (StreamWriter sw = File.CreateText($"Levels/{name}/level"))
            {
                sw.WriteLine(jsonResult.ToString());
                sw.Close();
            }

            return file;
        }

        public static void UpdateLevel(string name, Level level)
        {
            string jsonResult = JsonConvert.SerializeObject(level);

            using (StreamWriter sw = File.CreateText($"Levels/{name}/level"))
            {
                sw.WriteLine(jsonResult.ToString());
                sw.Close();
            }
        }

        public static bool LevelExists(string name)
        {
            if (!checkMainDirectoryExistance())
                return false;

            var directories = Directory.GetDirectories("Levels");

            if (directories.Length == 0)
                return false;

            foreach (var dir in directories)
            {
                if (dir.Substring(7).ToLower() == name.ToLower())
                    return true;
            }

            return false;
        }

        public static void DeleteLevel(string name)
        {
            Directory.Delete($"Levels/{name}", true);
        }

        public static void CreateLevelDirectory(string name)
        {
            checkMainDirectoryExistance();

            Directory.CreateDirectory($"Levels/{name}");
        }

        public static bool LevelHasCustomAudio(string name) => File.Exists($"Levels/{name}/audio.mp3");

        private static bool checkMainDirectoryExistance()
        {
            if (!Directory.Exists("Levels"))
            {
                Directory.CreateDirectory("Levels");
                return false;
            }

            return true;
        }
    }
}
