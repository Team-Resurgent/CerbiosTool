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
            themes.Add(new Theme
            {
                Name = "Resurgent",
                SplashBackground = "000000",
                SplashCerbiosText = "9EDAF9",
                SplashSafeModeText = "FB9E9E",
                SplashLogo1 = "781244",
                SplashLogo2 = "EB2F75",
                SplashLogo3 = "CD0000",
                SplashLogo4 = "6E006E",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Underdog",
                SplashBackground = "000000",
                SplashCerbiosText = "7F7F7F",
                SplashSafeModeText = "FF1818",
                SplashLogo1 = "003700",
                SplashLogo2 = "005A00",
                SplashLogo3 = "008400",
                SplashLogo4 = "002800",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "TopDog",
                SplashBackground = "FFF3F3",
                SplashCerbiosText = "333333",
                SplashSafeModeText = "8B8B8B",
                SplashLogo1 = "003A00",
                SplashLogo2 = "005F00",
                SplashLogo3 = "1B7B1B",
                SplashLogo4 = "002A00",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Blue Nose",
                SplashBackground = "023C5E",
                SplashCerbiosText = "949494",
                SplashSafeModeText = "ADADAD",
                SplashLogo1 = "8F8F8F",
                SplashLogo2 = "CFCFCF",
                SplashLogo3 = "E9E9E9",
                SplashLogo4 = "023C5E",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Pink Lady",
                SplashBackground = "393939",
                SplashCerbiosText = "848484",
                SplashSafeModeText = "BABABA",
                SplashLogo1 = "FF0074",
                SplashLogo2 = "FF00A6",
                SplashLogo3 = "FF4DE9",
                SplashLogo4 = "393939",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Sh4d3s 0f Gr3y",
                SplashBackground = "000000",
                SplashCerbiosText = "0F0F0F",
                SplashSafeModeText = "1E1E1E",
                SplashLogo1 = "0B0A0A",
                SplashLogo2 = "111010",
                SplashLogo3 = "1A1A1A",
                SplashLogo4 = "030303",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "4 The Ladies",
                SplashBackground = "000000",
                SplashCerbiosText = "FF00BD",
                SplashSafeModeText = "53EED8",
                SplashLogo1 = "FF00BD",
                SplashLogo2 = "01B0DF",
                SplashLogo3 = "53EED8",
                SplashLogo4 = "000000",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Teal On Steel Dark",
                SplashBackground = "2C2C2C",
                SplashCerbiosText = "555555",
                SplashSafeModeText = "8C8C8C",
                SplashLogo1 = "00888D",
                SplashLogo2 = "00C6C9",
                SplashLogo3 = "4FF3F9",
                SplashLogo4 = "004B4E",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Teal On Steel Light",
                SplashBackground = "555555",
                SplashCerbiosText = "2C2C2C",
                SplashSafeModeText = "8C8C8C",
                SplashLogo1 = "00888D",
                SplashLogo2 = "00C6C9",
                SplashLogo3 = "4FF3F9",
                SplashLogo4 = "0C7678",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Teal On Steel Light v2",
                SplashBackground = "555555",
                SplashCerbiosText = "2A2A2A",
                SplashSafeModeText = "8C8C8C",
                SplashLogo1 = "017D82",
                SplashLogo2 = "02B4B7",
                SplashLogo3 = "4BD6DC",
                SplashLogo4 = "0C7678",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Crystal",
                SplashBackground = "E8E8E8",
                SplashCerbiosText = "9D9D9D",
                SplashSafeModeText = "C2C2C2",
                SplashLogo1 = "7F7F7F",
                SplashLogo2 = "EAEAEA",
                SplashLogo3 = "BFBDBD",
                SplashLogo4 = "9A9A9A",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Hotline XBox",
                SplashBackground = "1B1B1B",
                SplashCerbiosText = "C1C1C1",
                SplashSafeModeText = "53EED8",
                SplashLogo1 = "E500A6",
                SplashLogo2 = "00D7DF",
                SplashLogo3 = "B1E233",
                SplashLogo4 = "3AD07B",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Shes a lady",
                SplashBackground = "000000",
                SplashCerbiosText = "701662",
                SplashSafeModeText = "6A25CC",
                SplashLogo1 = "730F73",
                SplashLogo2 = "DD37E1",
                SplashLogo3 = "6A25CC",
                SplashLogo4 = "620D77",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Gender Unknown",
                SplashBackground = "FFFFFF",
                SplashCerbiosText = "0CDBFD",
                SplashSafeModeText = "FF04CE",
                SplashLogo1 = "0CDBFD",
                SplashLogo2 = "FF04CE",
                SplashLogo3 = "9B7CFF",
                SplashLogo4 = "27E8FF",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Republic Of Shamers",
                SplashBackground = "000000",
                SplashCerbiosText = "9999A1",
                SplashSafeModeText = "595959",
                SplashLogo1 = "9999A1",
                SplashLogo2 = "B80404",
                SplashLogo3 = "EDEDED",
                SplashLogo4 = "7A7A7A",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "BSOD",
                SplashBackground = "000092",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "E9E9E9",
                SplashLogo2 = "FFFFFF",
                SplashLogo3 = "D9D7D7",
                SplashLogo4 = "000092",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Andy Wardog",
                SplashBackground = "272727",
                SplashCerbiosText = "E16AAB",
                SplashSafeModeText = "FEC361",
                SplashLogo1 = "18BBFA",
                SplashLogo2 = "10D9A5",
                SplashLogo3 = "FEC361",
                SplashLogo4 = "E16AAB",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Red Blue White",
                SplashBackground = "000000",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "AD0D0D",
                SplashLogo2 = "1802B9",
                SplashLogo3 = "FFFFFF",
                SplashLogo4 = "000000",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "PS2",
                SplashBackground = "000000",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "CBCBCB",
                SplashLogo1 = "5B17B0",
                SplashLogo2 = "0085C9",
                SplashLogo3 = "00D5FF",
                SplashLogo4 = "000000",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "SU v1",
                SplashBackground = "D2B41B",
                SplashCerbiosText = "1559C9",
                SplashSafeModeText = "3E78D6",
                SplashLogo1 = "284BBF",
                SplashLogo2 = "1559C9",
                SplashLogo3 = "3E78D6",
                SplashLogo4 = "D2B41B",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "SU v2",
                SplashBackground = "1559C9",
                SplashCerbiosText = "D2BD0E",
                SplashSafeModeText = "C3B847",
                SplashLogo1 = "9E940B",
                SplashLogo2 = "E4CD15",
                SplashLogo3 = "A89E2C",
                SplashLogo4 = "1559C9",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Oranje",
                SplashBackground = "DD6600",
                SplashCerbiosText = "F18A01",
                SplashSafeModeText = "FE7B00",
                SplashLogo1 = "EE8309",
                SplashLogo2 = "EE901D",
                SplashLogo3 = "C85000",
                SplashLogo4 = "DF5200",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Dog Marley",
                SplashBackground = "007038",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "E4312A",
                SplashLogo2 = "FBC324",
                SplashLogo3 = "E4502A",
                SplashLogo4 = "000000",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Foxy Roxy",
                SplashBackground = "1B1B1B",
                SplashCerbiosText = "4E4E4E",
                SplashSafeModeText = "A8A8A8",
                SplashLogo1 = "D47CC2",
                SplashLogo2 = "C336A7",
                SplashLogo3 = "F69AD4",
                SplashLogo4 = "881E5E",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Cold One Wif Da Boiz",
                SplashBackground = "046836",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "ED2027",
                SplashLogo2 = "FFFFFF",
                SplashLogo3 = "009C41",
                SplashLogo4 = "000000",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Dole Bludger",
                SplashBackground = "000000",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "00BACC",
                SplashLogo2 = "83C43F",
                SplashLogo3 = "F1D40A",
                SplashLogo4 = "28B449",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Planet Express",
                SplashBackground = "000000",
                SplashCerbiosText = "750001",
                SplashSafeModeText = "EFD028",
                SplashLogo1 = "022922",
                SplashLogo2 = "46CD6A",
                SplashLogo3 = "D40000",
                SplashLogo4 = "449ED0",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Shiny Metal Ass",
                SplashBackground = "000000",
                SplashCerbiosText = "750001",
                SplashSafeModeText = "EFD028",
                SplashLogo1 = "9FBBC1",
                SplashLogo2 = "7F9EA1",
                SplashLogo3 = "607C85",
                SplashLogo4 = "EDF893",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Halo",
                SplashBackground = "000000",
                SplashCerbiosText = "2C63BB",
                SplashSafeModeText = "FBF9FE",
                SplashLogo1 = "4C4A45",
                SplashLogo2 = "507D2A",
                SplashLogo3 = "677156",
                SplashLogo4 = "EFB82A",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Monochro Minimal",
                SplashBackground = "000000",
                SplashCerbiosText = "000000",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "000000",
                SplashLogo2 = "000000",
                SplashLogo3 = "696969",
                SplashLogo4 = "000000",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "How I got these scares",
                SplashBackground = "2E005A",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "12D920",
                SplashLogo2 = "000000",
                SplashLogo3 = "027816",
                SplashLogo4 = "FF0000",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Yeti",
                SplashBackground = "000000",
                SplashCerbiosText = "565656",
                SplashSafeModeText = "484848",
                SplashLogo1 = "D67400",
                SplashLogo2 = "00ACDE",
                SplashLogo3 = "79CEFF",
                SplashLogo4 = "212121",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Baby Doll v1",
                SplashBackground = "FFFFFF",
                SplashCerbiosText = "8BD9CF",
                SplashSafeModeText = "FFA5DD",
                SplashLogo1 = "8ED5F3",
                SplashLogo2 = "8BD9CF",
                SplashLogo3 = "E5BACC",
                SplashLogo4 = "77D1DF",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Baby Doll v2",
                SplashBackground = "FFFFFF",
                SplashCerbiosText = "E5BACC",
                SplashSafeModeText = "77D1DF",
                SplashLogo1 = "8ED5F3",
                SplashLogo2 = "8BD9CF",
                SplashLogo3 = "E5BACC",
                SplashLogo4 = "77D1DF",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Deja Vu",
                SplashBackground = "FFF6F6",
                SplashCerbiosText = "2B2B2B",
                SplashSafeModeText = "848484",
                SplashLogo1 = "545596",
                SplashLogo2 = "3CA5A9",
                SplashLogo3 = "1C6BB3",
                SplashLogo4 = "5C3E73",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "WinDog 98",
                SplashBackground = "028080",
                SplashCerbiosText = "3D3D3D",
                SplashSafeModeText = "7B7B7B",
                SplashLogo1 = "3F3F3F",
                SplashLogo2 = "515151",
                SplashLogo3 = "606060",
                SplashLogo4 = "3E3E3E",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Raiders",
                SplashBackground = "000000",
                SplashCerbiosText = "B8B8B8",
                SplashSafeModeText = "3D3D3D",
                SplashLogo1 = "2D2D2D",
                SplashLogo2 = "7C7C7C",
                SplashLogo3 = "C0C0C0",
                SplashLogo4 = "242424",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Aussi",
                SplashBackground = "171717",
                SplashCerbiosText = "292929",
                SplashSafeModeText = "292929",
                SplashLogo1 = "AF8404",
                SplashLogo2 = "F3920B",
                SplashLogo3 = "B26C2D",
                SplashLogo4 = "4D4F05",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "n0n4m3",
                SplashBackground = "000000",
                SplashCerbiosText = "11275E",
                SplashSafeModeText = "7E0D96",
                SplashLogo1 = "401B67",
                SplashLogo2 = "7E0D96",
                SplashLogo3 = "11275E",
                SplashLogo4 = "352577",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Evil Darkness",
                SplashBackground = "000000",
                SplashCerbiosText = "080808",
                SplashSafeModeText = "121212",
                SplashLogo1 = "000000",
                SplashLogo2 = "000000",
                SplashLogo3 = "000000",
                SplashLogo4 = "2B0000",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Gold Plated",
                SplashBackground = "520585",
                SplashCerbiosText = "A659FF",
                SplashSafeModeText = "A659FF",
                SplashLogo1 = "FF9900",
                SplashLogo2 = "7603D7",
                SplashLogo3 = "FFB18B",
                SplashLogo4 = "A659FF",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Red Raw",
                SplashBackground = "000000",
                SplashCerbiosText = "411919",
                SplashSafeModeText = "6F2F2F",
                SplashLogo1 = "2D1010",
                SplashLogo2 = "B41919",
                SplashLogo3 = "1A1A1A",
                SplashLogo4 = "2D1010",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Red Bull",
                SplashBackground = "000064",
                SplashCerbiosText = "E11B4C",
                SplashSafeModeText = "C44563",
                SplashLogo1 = "FFD300",
                SplashLogo2 = "E11B4C",
                SplashLogo3 = "C9A6A6",
                SplashLogo4 = "E1B612",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Carved In Stone",
                SplashBackground = "2C2C2C",
                SplashCerbiosText = "1B1B1B",
                SplashSafeModeText = "454545",
                SplashLogo1 = "191919",
                SplashLogo2 = "232323",
                SplashLogo3 = "373636",
                SplashLogo4 = "4A4A4A",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Baby Blue",
                SplashBackground = "000000",
                SplashCerbiosText = "9A9A9A",
                SplashSafeModeText = "525252",
                SplashLogo1 = "0074FF",
                SplashLogo2 = "008BFE",
                SplashLogo3 = "349FF9",
                SplashLogo4 = "760B0B",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Halo 360",
                SplashBackground = "000000",
                SplashCerbiosText = "C74F41",
                SplashSafeModeText = "444040",
                SplashLogo1 = "37342D",
                SplashLogo2 = "4B5A16",
                SplashLogo3 = "556440",
                SplashLogo4 = "DA872B",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Girly - Apricot",
                SplashBackground = "FE866B",
                SplashCerbiosText = "D6D6D6",
                SplashSafeModeText = "E4E4E4",
                SplashLogo1 = "71D5F9",
                SplashLogo2 = "5CFAC4",
                SplashLogo3 = "B7FE8D",
                SplashLogo4 = "FE866B",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Girly - Rose",
                SplashBackground = "DA9EAB",
                SplashCerbiosText = "D6D6D6",
                SplashSafeModeText = "E4E4E4",
                SplashLogo1 = "71D5F9",
                SplashLogo2 = "5CFAC4",
                SplashLogo3 = "B7FE8D",
                SplashLogo4 = "DA9EAB",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Barbie Gal",
                SplashBackground = "181818",
                SplashCerbiosText = "E977D5",
                SplashSafeModeText = "B46DAF",
                SplashLogo1 = "FF8AE1",
                SplashLogo2 = "E977D5",
                SplashLogo3 = "FF92FC",
                SplashLogo4 = "84487E",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Barbie Gal - ALT",
                SplashBackground = "181818",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "5F5F5F",
                SplashLogo1 = "FF8AE1",
                SplashLogo2 = "E977D5",
                SplashLogo3 = "F7C2F4",
                SplashLogo4 = "84487E",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Mint Boggling",
                SplashBackground = "0FB76E",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "0D7646",
                SplashLogo2 = "0FB76E",
                SplashLogo3 = "10EC8D",
                SplashLogo4 = "91F9AF",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "PFTMCLUB",
                SplashBackground = "000000",
                SplashCerbiosText = "AE07CE",
                SplashSafeModeText = "4A95D1",
                SplashLogo1 = "13D012",
                SplashLogo2 = "D2D2D2",
                SplashLogo3 = "ADADAD",
                SplashLogo4 = "2EFF0D",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Breeze",
                SplashBackground = "76FACA",
                SplashCerbiosText = "3C3C3C",
                SplashSafeModeText = "434343",
                SplashLogo1 = "D8B545",
                SplashLogo2 = "D9CE44",
                SplashLogo3 = "EBDD74",
                SplashLogo4 = "9B8114",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Scream of Green",
                SplashBackground = "015F01",
                SplashCerbiosText = "2D3B2D",
                SplashSafeModeText = "2C4B2D",
                SplashLogo1 = "2D3B2D",
                SplashLogo2 = "13760A",
                SplashLogo3 = "2DA41B",
                SplashLogo4 = "173F12",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "ModzvilleUSA",
                SplashBackground = "000000",
                SplashCerbiosText = "FF7DA8",
                SplashSafeModeText = "D700B9",
                SplashLogo1 = "6A02FB",
                SplashLogo2 = "FF7DA8",
                SplashLogo3 = "D700B9",
                SplashLogo4 = "1D0142",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Flashback",
                SplashBackground = "704D2D",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "F5D109",
                SplashLogo2 = "E67E0F",
                SplashLogo3 = "C15C0E",
                SplashLogo4 = "704D2D",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "That 70s Dog",
                SplashBackground = "20429C",
                SplashCerbiosText = "FFFFFF",
                SplashSafeModeText = "FFFFFF",
                SplashLogo1 = "0089D1",
                SplashLogo2 = "92CA65",
                SplashLogo3 = "E46C16",
                SplashLogo4 = "F3B026",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Reddit",
                SplashBackground = "FFFFFF",
                SplashCerbiosText = "222222",
                SplashSafeModeText = "222222",
                SplashLogo1 = "FF4400",
                SplashLogo2 = "FFFFFF",
                SplashLogo3 = "FF4400",
                SplashLogo4 = "FF4400",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Modern Fart",
                SplashBackground = "000000",
                SplashCerbiosText = "9E2D01",
                SplashSafeModeText = "9E2D01",
                SplashLogo1 = "9E2D01",
                SplashLogo2 = "4E005F",
                SplashLogo3 = "216400",
                SplashLogo4 = "2602BF",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Milk Coffee",
                SplashBackground = "000000",
                SplashCerbiosText = "262626",
                SplashSafeModeText = "393939",
                SplashLogo1 = "231300",
                SplashLogo2 = "472700",
                SplashLogo3 = "412100",
                SplashLogo4 = "281800",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "ComputerBooter",
                SplashBackground = "2E2E36",
                SplashCerbiosText = "00FF00",
                SplashSafeModeText = "E4AF07",
                SplashLogo1 = "061D12",
                SplashLogo2 = "0A3823",
                SplashLogo3 = "198553",
                SplashLogo4 = "00FF00",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "OGXBox Installer",
                SplashBackground = "E3E3E3",
                SplashCerbiosText = "000F17",
                SplashSafeModeText = "002B3F",
                SplashLogo1 = "000F17",
                SplashLogo2 = "002B3F",
                SplashLogo3 = "004B71",
                SplashLogo4 = "E3E3E3",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "tnegruseR",
                SplashBackground = "000000",
                SplashCerbiosText = "87EDBB",
                SplashSafeModeText = "046161",
                SplashLogo1 = "87EDBB",
                SplashLogo2 = "14D08A",
                SplashLogo3 = "32FFFF",
                SplashLogo4 = "91FF91",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "NoFameTheme",
                SplashBackground = "FFFFFF",
                SplashCerbiosText = "9EDAF9",
                SplashSafeModeText = "9EE4FB",
                SplashLogo1 = "0F4878",
                SplashLogo2 = "2ECAEB",
                SplashLogo3 = "0083CD",
                SplashLogo4 = "00556E",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "XBox-Scene",
                SplashBackground = "E5E5E8",
                SplashCerbiosText = "000000",
                SplashSafeModeText = "24201F",
                SplashLogo1 = "3B7602",
                SplashLogo2 = "7DB22E",
                SplashLogo3 = "A8D360",
                SplashLogo4 = "CFCFCF",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Mountain-Dew",
                SplashBackground = "000000",
                SplashCerbiosText = "E8182A",
                SplashSafeModeText = "59B534",
                SplashLogo1 = "026232",
                SplashLogo2 = "A5EF5F",
                SplashLogo3 = "FFFFFF",
                SplashLogo4 = "E8182A",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Aurora",
                SplashBackground = "C1C1C1",
                SplashCerbiosText = "FFBCBC",
                SplashSafeModeText = "9FF9B5",
                SplashLogo1 = "FFBCBC",
                SplashLogo2 = "64DDDF",
                SplashLogo3 = "9FF9B5",
                SplashLogo4 = "FFBCBC",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Tainted Purple",
                SplashBackground = "111111",
                SplashCerbiosText = "4E4E4E",
                SplashSafeModeText = "6C6C6C",
                SplashLogo1 = "7940C1",
                SplashLogo2 = "FE9EE7",
                SplashLogo3 = "FC81F8",
                SplashLogo4 = "E375FF",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Pastel",
                SplashBackground = "FFFFFF",
                SplashCerbiosText = "404040",
                SplashSafeModeText = "FAD2A1",
                SplashLogo1 = "78F6F8",
                SplashLogo2 = "85FDD1",
                SplashLogo3 = "FC7CCF",
                SplashLogo4 = "FFFFFF",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Not a golder retriever",
                SplashBackground = "000000",
                SplashCerbiosText = "EDEDED",
                SplashSafeModeText = "3B3B3B",
                SplashLogo1 = "D4B928",
                SplashLogo2 = "D5D114",
                SplashLogo3 = "EDF751",
                SplashLogo4 = "DA2121",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Spirit",
                SplashBackground = "FFFFFF",
                SplashCerbiosText = "F0EEEE",
                SplashSafeModeText = "F0EEEE",
                SplashLogo1 = "EFEFEF",
                SplashLogo2 = "E9E8E8",
                SplashLogo3 = "F2F2F2",
                SplashLogo4 = "F8F8F8",
                SplashScale = 1
            });
            themes.Add(new Theme
            {
                Name = "Spirit v2",
                SplashBackground = "FFFFFF",
                SplashCerbiosText = "F0EEEE",
                SplashSafeModeText = "F0EEEE",
                SplashLogo1 = "F7F7F7",
                SplashLogo2 = "F0F0F0",
                SplashLogo3 = "F5F5F5",
                SplashLogo4 = "C6FCFA",
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
