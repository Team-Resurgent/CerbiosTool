using System.Numerics;
using System.Text.Json;

namespace Repackinator.Shared
{
    public struct Config
    {
        public uint UDMAMode { get; set; }

        public uint SplashBackground { get; set; }

        public uint SplashCerbiosText { get; set; }

        public uint SplashSafeModeText { get; set; }

        public uint SplashLogo1 { get; set; }

        public uint SplashLogo2 { get; set; }

        public uint SplashLogo3 { get; set; }

        public Config()
        {
            UDMAMode = 2;
            SplashBackground = 0x000000;
            SplashCerbiosText = 0xFFFFFF;
            SplashSafeModeText = 0xFFFFFF;
            SplashLogo1 = 0x00018D;
            SplashLogo2 = 0x1C00C9;
            SplashLogo3 = 0x4F92F9;
        }

        public void SetDefaults()
        {
            UDMAMode = 2;
            SplashBackground = 0x000000;
            SplashCerbiosText = 0xFFFFFF;
            SplashSafeModeText = 0xFFFFFF;
            SplashLogo1 = 0x00018D;
            SplashLogo2 = 0x1C00C9;
            SplashLogo3 = 0x4F92F9;
        }

        public static Vector3 RGBToVector3(uint color)
        {
            var r = ((color >> 16) & 0xff) / 255.0f;
            var g = ((color >> 8) & 0xff) / 255.0f;
            var b = (color & 0xff) / 255.0f;
            return new Vector3(r, g, b);
        }

        public static Vector4 RGBToVector4(uint color)
        {
            var r = ((color >> 16) & 0xff) / 255.0f;
            var g = ((color >> 8) & 0xff) / 255.0f;
            var b = (color & 0xff) / 255.0f;
            return new Vector4(r, g, b, 1.0f);
        }

        public static uint Vector3ToRGB(Vector3 color)
        {
            var r = (byte)(color.X * 255.0f);
            var g = (byte)(color.Y * 255.0f);
            var b = (byte)(color.Z * 255.0f);
            return (uint)((r << 16) | (g << 8) | b);
        }

        public static Config LoadConfig(string path)
        {
            var configJson = File.ReadAllText(path);
            var result = JsonSerializer.Deserialize<Config>(configJson);
            return result;
        }

        public static Config LoadConfig()
        {
            var applicationPath = Utility.GetApplicationPath();
            if (applicationPath == null)
            {
                return new Config();
            }

            var configPath = Path.Combine(applicationPath, "config.json");
            if (!File.Exists(configPath))
            {
                return new Config();
            }

            return LoadConfig(configPath);
        }

        public static void SaveConfig(string path, Config? config)
        {
            if (config == null)
            {
                return;
            }

            var result = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, result);
        }

        public static void SaveConfig(Config? config)
        {
            var applicationPath = Utility.GetApplicationPath();
            if (applicationPath == null)
            {
                return;
            }

            var configPath = Path.Combine(applicationPath, "config.json");
            SaveConfig(configPath, config);
        }

    }
}
