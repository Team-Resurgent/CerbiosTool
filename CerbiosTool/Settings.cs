using System.Numerics;
using System.Text.Json;

namespace CerbiosTool
{
    public struct Settings
    {
        public string BiosPath { get; set; }

        public string BiosFile { get; set; }

        public string ConfigPath { get; set; }

        public Settings()
        {
            BiosPath = string.Empty;
            BiosFile = string.Empty;
            ConfigPath = string.Empty;
        }

        public static Settings LoadSettings(string settingsPath)
        {
            var settingsJson = File.ReadAllText(settingsPath);
            var result = JsonSerializer.Deserialize<Settings>(settingsJson);
            return result;
        }

        public static Settings LoadSettings()
        {
            var applicationPath = Utility.GetApplicationPath();
            if (applicationPath == null)
            {
                return new Settings();
            }

            var settingsPath = Path.Combine(applicationPath, "settings.json");
            if (!File.Exists(settingsPath))
            {
                return new Settings();
            }

            return LoadSettings(settingsPath);
        }

        public static void SaveSattings(string settingsPath, Settings? settings)
        {
            if (settings == null)
            {
                return;
            }

            var result = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(settingsPath, result);
        }

        public static void SaveSattings(Settings? settings)
        {
            var applicationPath = Utility.GetApplicationPath();
            if (applicationPath == null)
            {
                return;
            }

            var settingsPath = Path.Combine(applicationPath, "settings.json");
            SaveSattings(settingsPath, settings);
        }
    }
}
