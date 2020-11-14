using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MovingCastles.GameSystems.Saving
{
    public class SaveManager : ISaveManager
    {
        public void Save(IGameManager gameManager)
        {
            var appDataFolder = Environment.GetEnvironmentVariable(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "LocalAppData"
                    : "Home");
            var mcFolder = Path.Combine(appDataFolder, "MovingCastles");
            Directory.CreateDirectory(mcFolder);

            var saveFilePath = Path.Combine(mcFolder, "save.mc");
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }

            using var file = File.OpenWrite(saveFilePath);
            using var writer = new StreamWriter(file);
            writer.WriteLine("Hello world");
        }
    }
}
