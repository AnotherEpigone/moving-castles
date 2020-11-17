using MovingCastles.Entities;
using MovingCastles.Serialization.Entities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace MovingCastles.GameSystems.Saving
{
    public class SaveManager : ISaveManager
    {
        private const string FileName = "save.mc";

        public void Write(Save save)
        {
            var mcFolder = GetSaveFolder();
            Directory.CreateDirectory(mcFolder);

            var saveFilePath = Path.Combine(mcFolder, FileName);
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }

            using var file = File.OpenWrite(saveFilePath);
            using var writer = new StreamWriter(file);

            var json = JsonConvert.SerializeObject(save);
            writer.Write(json);
        }

        public (bool, Save) Read()
        {
            var mcFolder = GetSaveFolder();
            var saveFilePath = Path.Combine(mcFolder, FileName);
            if (!File.Exists(saveFilePath))
            {
                return (false, null);
            }

            using var file = File.OpenRead(saveFilePath);
            using var reader = new StreamReader(file);
            var save = JsonConvert.DeserializeObject<Save>(reader.ReadToEnd());

            return (true, save);
        }

        public bool SaveExists()
        {
            var mcFolder = GetSaveFolder();
            var saveFilePath = Path.Combine(mcFolder, FileName);
            return File.Exists(saveFilePath);
        }

        private string GetSaveFolder()
        {
            var appDataFolder = Environment.GetEnvironmentVariable(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "LocalAppData"
                    : "Home");
            return Path.Combine(appDataFolder, "MovingCastles");
        }
    }
}
