using System.Numerics;
using System.Text.Json;

namespace CerbiosTool
{
    public struct Theme
    {
        public string Name { get; set; }

        public uint SplashBackground { get; set; }

        public uint SplashCerbiosText { get; set; }

        public uint SplashSafeModeText { get; set; }

        public uint SplashLogo1 { get; set; }

        public uint SplashLogo2 { get; set; }

        public uint SplashLogo3 { get; set; }

        public uint SplashLogo4 { get; set; }

        public byte SplashScale { get; set; }

        private static Theme[] DefaultThemes()
        {
            var themes = new List<Theme>();
            themes.Add(new Theme {
                Name = "Blue",
                SplashBackground = 0x000000,
                SplashCerbiosText = 0xFFFFFF,
                SplashSafeModeText = 0xFFFFFF,
                SplashLogo1 = 0x00018D,
                SplashLogo2 = 0x1C00C9,
                SplashLogo3 = 0x4F92F9,
                SplashLogo4 = 0x800000
            });
            themes.Add(new Theme
            {
                Name = "Red",
                SplashBackground = 0x000000,
                SplashCerbiosText = 0xFFFFFF,
                SplashSafeModeText = 0xFFFFFF,
                SplashLogo1 = 0x8D0001,
                SplashLogo2 = 0xC9001C,
                SplashLogo3 = 0xF9924F,
                SplashLogo4 = 0x000080
            });
            themes.Add(new Theme
            {
                Name = "Green",
                SplashBackground = 0x000000,
                SplashCerbiosText = 0xFFFFFF,
                SplashSafeModeText = 0xFFFFFF,
                SplashLogo1 = 0x008D01,
                SplashLogo2 = 0x00C91C,
                SplashLogo3 = 0x92F94F,
                SplashLogo4 = 0x000080
            });
            themes.Add(new Theme
            {
                Name = "Touch of IND",
                SplashBackground = 0xFFFFFF,
                SplashCerbiosText = 0x6FBD19,
                SplashSafeModeText = 0x6FBD19,
                SplashLogo1 = 0x125212,
                SplashLogo2 = 0x7FC92A,
                SplashLogo3 = 0xD3F134,
                SplashLogo4 = 0x000080
            });
            themes.Add(new Theme
            {
                Name = "Red Eyes, White",
                SplashBackground = 0x000000,
                SplashCerbiosText = 0x00018D,
                SplashSafeModeText = 0xA90000,
                SplashLogo1 = 0x00018D,
                SplashLogo2 = 0xD2D2D2,
                SplashLogo3 = 0xADADAD,
                SplashLogo4 = 0x800000
            });
            return themes.OrderBy(n => n.Name).ToArray();
        }

        private static void SaveThemes()
        {
            var themes = DefaultThemes();

            var applicationPath = Utility.GetApplicationPath();
            if (applicationPath == null)
            {
                return;
            }

            var settingsPath = Path.Combine(applicationPath, "themes.json");
            var result = JsonSerializer.Serialize(themes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(settingsPath, result);
        }

        public static Theme[] LoadThemes()
        {
            var applicationPath = Utility.GetApplicationPath();
            if (applicationPath == null)
            {
                return DefaultThemes();
            }

            var settingsPath = Path.Combine(applicationPath, "themes.json");
            if (!File.Exists(settingsPath))
            {
                SaveThemes();
            }

            var settingsJson = File.ReadAllText(settingsPath);
            var result = JsonSerializer.Deserialize<Theme[]>(settingsJson);
            if (result != null)
            {
                return result.OrderBy(n => n.Name).ToArray();
            }
            return DefaultThemes();
        }

        public static string[] BuildThemeDropDownList(Theme[] themes)
        {
            var themeList = new List<string>
            {
                "Current"
            };
            foreach (var theme in themes)
            {
                themeList.Add(theme.Name);
            }
            return themeList.ToArray();
        }
    }
}
