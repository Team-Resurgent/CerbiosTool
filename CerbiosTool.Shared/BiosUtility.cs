using Repackinator.Shared;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerbiosTool.Shared
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

        public static bool LoadBiosComfig(string path, ref Config config, ref byte[] biosData)
        {
            var unpacker = ResourceLoader.GetEmbeddedResourceBytes("CerbiosTool.Shared.Resources.unpack.exe");
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

            var searchPattern = new byte[] { 0x2A, 0x2A, 0x5B, 0x43, 0x45, 0x52, 0x42, 0x49, 0x4F, 0x53, 0x5D, 0x2A };
            var configOffset = SearchData(biosData, searchPattern);
            if (configOffset >= 0)
            {
                configOffset += 16;
                config.UDMAMode = biosData[configOffset];
                config.SplashBackground = (uint)((biosData[configOffset + 1] << 16) | (biosData[configOffset + 2] << 8) | biosData[configOffset + 3]);
                config.SplashCerbiosText = (uint)((biosData[configOffset + 4] << 16) | (biosData[configOffset + 5] << 8) | biosData[configOffset + 6]);
                config.SplashSafeModeText = (uint)((biosData[configOffset + 7] << 16) | (biosData[configOffset + 8] << 8) | biosData[configOffset + 9]);
                config.SplashLogo1 = (uint)((biosData[configOffset + 10] << 16) | (biosData[configOffset + 11] << 8) | biosData[configOffset + 12]);
                config.SplashLogo2 = (uint)((biosData[configOffset + 13] << 16) | (biosData[configOffset + 14] << 8) | biosData[configOffset + 15]);
                config.SplashLogo3 = (uint)((biosData[configOffset + 16] << 16) | (biosData[configOffset + 17] << 8) | biosData[configOffset + 18]);
            }

            return true;
        }

        public static void SaveBiosComfig(Config config, string path, byte[] biosData)
        {
            var packer = ResourceLoader.GetEmbeddedResourceBytes("CerbiosTool.Shared.Resources.pack.exe");
            var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            File.WriteAllBytes(tempFile, packer);

            var searchPattern = new byte[] { 0x2A, 0x2A, 0x5B, 0x43, 0x45, 0x52, 0x42, 0x49, 0x4F, 0x53, 0x5D, 0x2A };
            var configOffset = SearchData(biosData, searchPattern);
            if (configOffset >= 0)
            {
                configOffset += 16;
                biosData[configOffset] = (byte)config.UDMAMode;
                biosData[configOffset + 1] = (byte)((config.SplashBackground >> 16) & 0xff);
                biosData[configOffset + 2] = (byte)((config.SplashBackground >> 8) & 0xff);
                biosData[configOffset + 3] = (byte)(config.SplashBackground & 0xff);
                biosData[configOffset + 4] = (byte)((config.SplashCerbiosText >> 16) & 0xff);
                biosData[configOffset + 5] = (byte)((config.SplashCerbiosText >> 8) & 0xff);
                biosData[configOffset + 6] = (byte)(config.SplashCerbiosText & 0xff);
                biosData[configOffset + 7] = (byte)((config.SplashSafeModeText >> 16) & 0xff);
                biosData[configOffset + 8] = (byte)((config.SplashSafeModeText >> 8) & 0xff);
                biosData[configOffset + 9] = (byte)(config.SplashSafeModeText & 0xff);
                biosData[configOffset + 10] = (byte)((config.SplashLogo1 >> 16) & 0xff);
                biosData[configOffset + 11] = (byte)((config.SplashLogo1 >> 8) & 0xff);
                biosData[configOffset + 12] = (byte)(config.SplashLogo1 & 0xff);
                biosData[configOffset + 13] = (byte)((config.SplashLogo2 >> 16) & 0xff);
                biosData[configOffset + 14] = (byte)((config.SplashLogo2 >> 8) & 0xff);
                biosData[configOffset + 15] = (byte)(config.SplashLogo2 & 0xff);
                biosData[configOffset + 16] = (byte)((config.SplashLogo3 >> 16) & 0xff);
                biosData[configOffset + 17] = (byte)((config.SplashLogo3 >> 8) & 0xff);
                biosData[configOffset + 18] = (byte)(config.SplashLogo3 & 0xff);
            }

            var tempInFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            File.WriteAllBytes(tempInFile, biosData);

            var processStartInfo = new ProcessStartInfo(tempFile, $"\"{tempInFile}\" \"{path}\" \"{path}\"")
            {
                CreateNoWindow = true
            };
            var process = Process.Start(processStartInfo);
            process?.WaitForExit();
        }
    }
}
