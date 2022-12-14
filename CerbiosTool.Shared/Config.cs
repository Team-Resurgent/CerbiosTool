using System.Numerics;
using System.Text.Json;

namespace Repackinator.Shared
{
    public struct Config
    {
        public byte LoadConfig { get; set; }

        public byte DriveSetup { get; set; }

        public byte AVCheck { get; set; }

        public byte Debug { get; set; }

        public string CdPath1 { get; set; }

        public string CdPath2 { get; set; }

        public string CdPath3 { get; set; }

        public string DashPath1 { get; set; }

        public string DashPath2 { get; set; }

        public string DashPath3 { get; set; }

        public string BootAnimPath { get; set; }

        public string FrontLed { get; set; }

        public byte IGRMasterPort { get; set; }

        public string IGRDash { get; set; }

        public string IGRGame { get; set; }

        public string IGRFull { get; set; }

        public string IGRShutdown { get; set; }

        public byte FanSpeed { get; set; }

        public byte UDMAMode { get; set; }

        public uint SplashBackground { get; set; }

        public uint SplashCerbiosText { get; set; }

        public uint SplashSafeModeText { get; set; }

        public uint SplashLogo1 { get; set; }

        public uint SplashLogo2 { get; set; }

        public uint SplashLogo3 { get; set; }

        public uint SplashLogo4 { get; set; }

        public Config()
        {
            LoadConfig = 1;
            DriveSetup = 1;
            AVCheck = 1;
            Debug = 0;
            CdPath1 = string.Empty;
            CdPath2 = string.Empty;
            CdPath3 = string.Empty;
            DashPath1 = @"\Device\Harddisk0\Partition2\evoxdash.xbe";
            DashPath2 = @"\Device\Harddisk0\Partition2\avalaunch.xbe";
            DashPath3 = @"\Device\Harddisk0\Partition2\nexgen.xbe";
            BootAnimPath = @"\Device\Harddisk0\Partition2\BootAnims\Xbox\bootanim.xbe";
            FrontLed = "GGGG";
            IGRMasterPort = 0;
            IGRDash = "67CD";
            IGRGame = "467C";
            IGRFull = "467D";
            IGRShutdown = "678D";
            FanSpeed = 0;
            UDMAMode = 2;
            SplashBackground = 0x000000;
            SplashCerbiosText = 0xFFFFFF;
            SplashSafeModeText = 0xFFFFFF;
            SplashLogo1 = 0x00018D;
            SplashLogo2 = 0x1C00C9;
            SplashLogo3 = 0x4F92F9;
            SplashLogo4 = 0x800000;
        }

        public void SetDefaults()
        {
            LoadConfig = 1;
            DriveSetup = 1;
            AVCheck = 1;
            Debug = 0;
            CdPath1 = string.Empty;
            CdPath2 = string.Empty;
            CdPath3 = string.Empty;
            DashPath1 = @"\Device\Harddisk0\Partition2\evoxdash.xbe";
            DashPath2 = @"\Device\Harddisk0\Partition2\avalaunch.xbe";
            DashPath3 = @"\Device\Harddisk0\Partition2\nexgen.xbe";
            BootAnimPath = @"\Device\Harddisk0\Partition2\BootAnims\Xbox\bootanim.xbe";
            FrontLed = "GGGG";
            IGRMasterPort = 0;
            IGRDash = "67CD";
            IGRGame = "467C";
            IGRFull = "467D";
            IGRShutdown = "678D";
            FanSpeed = 0;
            UDMAMode = 2;
            SplashBackground = 0x000000;
            SplashCerbiosText = 0xFFFFFF;
            SplashSafeModeText = 0xFFFFFF;
            SplashLogo1 = 0x00018D;
            SplashLogo2 = 0x1C00C9;
            SplashLogo3 = 0x4F92F9;
            SplashLogo4 = 0x800000;
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

        public static Config LoadConfiguration(string path)
        {
            var configJson = File.ReadAllText(path);
            var result = JsonSerializer.Deserialize<Config>(configJson);
            return result;
        }

        public static Config LoadConfiguration()
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

            return LoadConfiguration(configPath);
        }

        public static void SaveConfiguration(string path, Config? config)
        {
            if (config == null)
            {
                return;
            }

            var result = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, result);
        }

        public static void SaveConfiguration(Config? config)
        {
            var applicationPath = Utility.GetApplicationPath();
            if (applicationPath == null)
            {
                return;
            }

            var configPath = Path.Combine(applicationPath, "config.json");
            SaveConfiguration(configPath, config);
        }

    }
}
