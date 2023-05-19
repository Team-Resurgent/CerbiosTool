using System.ComponentModel;
using System.Numerics;
using System.Text.Json;

namespace CerbiosTool
{
    public struct Theme
    {
        public string Name { get; set; }

        public string SplashBackground { get; set; }

        public string SplashCerbiosText { get; set; }

        public string SplashSafeModeText { get; set; }

        public string SplashLogo1 { get; set; }

        public string SplashLogo2 { get; set; }

        public string SplashLogo3 { get; set; }

        public string SplashLogo4 { get; set; }

        public byte SplashScale { get; set; }

        private static Theme[] DefaultThemes()
        {
            var themes = new List<Theme>();
            themes.Add(new Theme
            {
                Name = "Blue",
                SplashBackground = "000000",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "00018D",
                SplashLogo2 = "1C00C9",
                SplashLogo3 = "4F92F9",
                SplashLogo4 = "800000",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Red",
                SplashBackground = "000000",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "8D0001",
                SplashLogo2 = "C9001C",
                SplashLogo3 = "F9924F",
                SplashLogo4 = "000080",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Golden Shower",
                SplashBackground = "000000",
                SplashCerbiosText = "965A24",
                SplashSafeModeText = "D40000",
                SplashLogo1 = "965A24",
                SplashLogo2 = "D3AF37",
                SplashLogo3 = "A28328",
                SplashLogo4 = "930000",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Green",
                SplashBackground = "000000",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "008D01",
                SplashLogo2 = "00C91C",
                SplashLogo3 = "92F94F",
                SplashLogo4 = "000080",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Silver Fox",
                SplashBackground = "FFFFFF",
                SplashCerbiosText = "000000",
                SplashSafeModeText = "000000",
                SplashLogo1 = "4C4C4C",
                SplashLogo2 = "8C8C8C",
                SplashLogo3 = "DADADA",
                SplashLogo4 = "404040",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Touch of IND",
                SplashBackground = "FFFFFF",
                SplashCerbiosText = "6FBD19",
                SplashSafeModeText = "6FBD19",
                SplashLogo1 = "125212",
                SplashLogo2 = "7FC92A",
                SplashLogo3 = "D3F134",
                SplashLogo4 = "000080",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Red Eyes, White",
                SplashBackground = "000000",
                SplashCerbiosText = "00018D",
                SplashSafeModeText = "A90000",
                SplashLogo1 = "00018D",
                SplashLogo2 = "D2D2D2",
                SplashLogo3 = "ADADAD",
                SplashLogo4 = "800000",
                SplashScale = 1
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
