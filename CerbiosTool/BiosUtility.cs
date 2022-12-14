using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerbiosTool
{
    public static class BiosUtility
    {
        public static int SearchData(byte[] data, byte[] searchPattern)
        {
            if (data.Length < searchPattern.Length)
            {
                return -1;
            }
            for (var i = 0; i < data.Length; ++i)
            {
                for (var j = 0; j < searchPattern.Length; ++j)
                {
                    if ((i + j) >= data.Length || searchPattern[j] != data[i + j])
                    {
                        break;
                    }
                    if (j == searchPattern.Length - 1)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private static string GetString(byte[] buffer, int offset, int maxLength)
        {
            var result = string.Empty;
            for (var i = 0; i < maxLength; i++)
            {
                var value = (byte)Encoding.UTF8.GetString(buffer, i + offset, 1)[0];
                if (value == 0)
                {
                    break;
                }
                result += (char)value;
            }
            return result;
        }

        private static void SetString(string value, byte[] buffer, int offset, int maxLength)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            for (var i = 0; i < maxLength; i++)
            {
                if (i < valueBytes.Length)
                {
                    buffer[i + offset] = valueBytes[i];
                }
                else
                {
                    buffer[i + offset] = 0;
                }
            }
        }

        private static ushort IGRToUshort(string value)
        {
            ushort igrValue = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var shift = Convert.ToUInt16(value[i].ToString(), 16);
                igrValue = (ushort)(igrValue | (1 << (shift ^ 8)));
            }
            return igrValue;
        }

        private static string ShortToIGR(ushort value)
        {
            var temp = string.Empty;
            for (var i = 0; i < 16; i++)
            {
                var shift = 1 << (i ^ 8);
                if ((value & shift) > 0)
                {
                    temp += i.ToString("X1");
                }
            }
            return temp;
        }

        public static bool LoadBiosComfig(string path, ref Config config, ref byte[] biosData)
        {
            var unpacker = ResourceLoader.GetEmbeddedResourceBytes("CerbiosTool.Resources.unpack.exe");
            var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            File.WriteAllBytes(tempFile, unpacker);

            var tempOutFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var processStartInfo = new ProcessStartInfo(tempFile, $"\"{path}\" \"{tempOutFile}\"")
            {
                CreateNoWindow = true
            };
            var process = Process.Start(processStartInfo);
            process?.WaitForExit();
            if (process?.ExitCode != 0)
            {
                return false;
            }

            biosData = File.ReadAllBytes(tempOutFile);

            var searchPattern = new byte[] { 0x43, 0x45, 0x52, 0x42, 0x49, 0x4f, 0x53, 0x21, 0x21, 0x21 };
            var configOffset = SearchData(biosData, searchPattern);
            if (configOffset >= 0)
            {                
                var version = biosData[configOffset + 10];                
                if (version != 1)
                {
                    return false;
                }

                config.LoadConfig = biosData[configOffset + 11];
                config.DriveSetup = biosData[configOffset + 12];
                config.AVCheck = biosData[configOffset + 13];
                config.Debug = biosData[configOffset + 14];
                config.CdPath1 = GetString(biosData, configOffset + 15, 100);
                config.CdPath2 = GetString(biosData, configOffset + 115, 100);
                config.CdPath3 = GetString(biosData, configOffset + 215, 100);
                config.DashPath1 = GetString(biosData, configOffset + 315, 100);
                config.DashPath2 = GetString(biosData, configOffset + 415, 100);
                config.DashPath3 = GetString(biosData, configOffset + 515, 100);
                config.BootAnimPath = GetString(biosData, configOffset + 615, 100);
                config.FrontLed = GetString(biosData, configOffset + 715, 4);
                config.IGRMasterPort = biosData[configOffset + 719];
                config.IGRDash = ShortToIGR((ushort)((biosData[configOffset + 720] << 8) | biosData[configOffset + 721]));
                config.IGRGame = ShortToIGR((ushort)((biosData[configOffset + 722] << 8) | biosData[configOffset + 723]));
                config.IGRFull = ShortToIGR((ushort)((biosData[configOffset + 724] << 8) | biosData[configOffset + 725]));
                config.IGRShutdown = ShortToIGR((ushort)((biosData[configOffset + 726] << 8) | biosData[configOffset + 727]));
                config.FanSpeed = biosData[configOffset + 728];
                config.UDMAMode = biosData[configOffset + 729];
                config.SplashBackground = (uint)((biosData[configOffset + 730] << 16) | (biosData[configOffset + 731] << 8) | biosData[configOffset + 732]);
                config.SplashCerbiosText = (uint)((biosData[configOffset + 733] << 16) | (biosData[configOffset + 734] << 8) | biosData[configOffset + 735]);
                config.SplashSafeModeText = (uint)((biosData[configOffset + 736] << 16) | (biosData[configOffset + 737] << 8) | biosData[configOffset + 738]);
                config.SplashLogo1 = (uint)((biosData[configOffset + 739] << 16) | (biosData[configOffset + 740] << 8) | biosData[configOffset + 741]);
                config.SplashLogo2 = (uint)((biosData[configOffset + 742] << 16) | (biosData[configOffset + 743] << 8) | biosData[configOffset + 744]);
                config.SplashLogo3 = (uint)((biosData[configOffset + 745] << 16) | (biosData[configOffset + 746] << 8) | biosData[configOffset + 747]);
                config.SplashLogo4 = (uint)((biosData[configOffset + 748] << 16) | (biosData[configOffset + 749] << 8) | biosData[configOffset + 750]);
            }

            return true;
        }

        public static void SaveBiosConfig(Config config, string loadPath, string savePath, byte[] biosData)
        {
            var packer = ResourceLoader.GetEmbeddedResourceBytes("CerbiosTool.Resources.pack.exe");
            var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            File.WriteAllBytes(tempFile, packer);

            var searchPattern = new byte[] { 0x43, 0x45, 0x52, 0x42, 0x49, 0x4f, 0x53, 0x21, 0x21, 0x21 };
            var configOffset = SearchData(biosData, searchPattern);
            if (configOffset >= 0)
            {
                var version = biosData[configOffset + 10];
                if (version != 1)
                {
                    return;
                }

                biosData[configOffset + 11] = config.LoadConfig;
                biosData[configOffset + 12] = config.DriveSetup;
                biosData[configOffset + 13] = config.AVCheck;
                biosData[configOffset + 14] = config.Debug;
                SetString(config.CdPath1, biosData, configOffset + 15, 100);
                SetString(config.CdPath2, biosData, configOffset + 115, 100);
                SetString(config.CdPath3, biosData, configOffset + 215, 100);
                SetString(config.DashPath1, biosData, configOffset + 315, 100);
                SetString(config.DashPath2, biosData, configOffset + 415, 100);
                SetString(config.DashPath3, biosData, configOffset + 515, 100);
                SetString(config.BootAnimPath, biosData, configOffset + 615, 100);
                SetString(config.FrontLed.PadRight(4, 'O'), biosData, configOffset + 715, 4);
                biosData[configOffset + 719] = config.IGRMasterPort;
                var tempIgrDash = IGRToUshort(config.IGRDash);
                biosData[configOffset + 720] = (byte)((tempIgrDash >> 8) & 0xff);
                biosData[configOffset + 721] = (byte)(tempIgrDash & 0xff);
                var tempIgrGame = IGRToUshort(config.IGRGame);
                biosData[configOffset + 722] = (byte)((tempIgrGame >> 8) & 0xff);
                biosData[configOffset + 723] = (byte)(tempIgrGame & 0xff);
                var tempIgrFull = IGRToUshort(config.IGRFull);
                biosData[configOffset + 724] = (byte)((tempIgrFull >> 8) & 0xff);
                biosData[configOffset + 725] = (byte)(tempIgrFull & 0xff);
                var tempIgrShutdown = IGRToUshort(config.IGRShutdown);
                biosData[configOffset + 726] = (byte)((tempIgrShutdown >> 8) & 0xff);
                biosData[configOffset + 727] = (byte)(tempIgrShutdown & 0xff);
                biosData[configOffset + 728] = config.FanSpeed;
                biosData[configOffset + 729] = (byte)config.UDMAMode;
                biosData[configOffset + 730] = (byte)((config.SplashBackground >> 16) & 0xff);
                biosData[configOffset + 731] = (byte)((config.SplashBackground >> 8) & 0xff);
                biosData[configOffset + 732] = (byte)(config.SplashBackground & 0xff);
                biosData[configOffset + 733] = (byte)((config.SplashCerbiosText >> 16) & 0xff);
                biosData[configOffset + 734] = (byte)((config.SplashCerbiosText >> 8) & 0xff);
                biosData[configOffset + 735] = (byte)(config.SplashCerbiosText & 0xff);
                biosData[configOffset + 736] = (byte)((config.SplashSafeModeText >> 16) & 0xff);
                biosData[configOffset + 737] = (byte)((config.SplashSafeModeText >> 8) & 0xff);
                biosData[configOffset + 738] = (byte)(config.SplashSafeModeText & 0xff);
                biosData[configOffset + 739] = (byte)((config.SplashLogo1 >> 16) & 0xff);
                biosData[configOffset + 740] = (byte)((config.SplashLogo1 >> 8) & 0xff);
                biosData[configOffset + 741] = (byte)(config.SplashLogo1 & 0xff);
                biosData[configOffset + 742] = (byte)((config.SplashLogo2 >> 16) & 0xff);
                biosData[configOffset + 743] = (byte)((config.SplashLogo2 >> 8) & 0xff);
                biosData[configOffset + 744] = (byte)(config.SplashLogo2 & 0xff);
                biosData[configOffset + 745] = (byte)((config.SplashLogo3 >> 16) & 0xff);
                biosData[configOffset + 746] = (byte)((config.SplashLogo3 >> 8) & 0xff);
                biosData[configOffset + 747] = (byte)(config.SplashLogo3 & 0xff);
                biosData[configOffset + 748] = (byte)((config.SplashLogo4 >> 16) & 0xff);
                biosData[configOffset + 749] = (byte)((config.SplashLogo4 >> 8) & 0xff);
                biosData[configOffset + 750] = (byte)(config.SplashLogo4 & 0xff);
            }

            var tempInFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            File.WriteAllBytes(tempInFile, biosData);

            var arguments = $"\"{tempInFile}\" \"{loadPath}\" \"{savePath}\"";
            var processStartInfo = new ProcessStartInfo(tempFile, arguments)
            {
                CreateNoWindow = true
            };
            var process = Process.Start(processStartInfo);
            process?.WaitForExit();
        }
    }
}
