using ImGuiNET;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace CerbiosTool
{
    public unsafe class ApplicationUI
    {
        private Window? m_window;
        private PathPicker? m_biosFileOpenPicker;
        private PathPicker? m_biosFileSavePicker;
        private PathPicker? m_configFileOpenPicker;
        private PathPicker? m_configFileSavePicker;
        private PathPicker? m_iniFileSavePicker;
        private SplashDialog m_splashDialog = new();
        private OkDialog? m_okDialog;
        private Config m_config = new();
        private Settings m_settings = new();
        private bool m_biosLoaded = false;
        private byte[] m_biosData = Array.Empty<byte>();
        private Theme[] m_themes = Array.Empty<Theme>();
        private string[] m_themeNames = Array.Empty<string>();
        private bool m_showSplash = true;

        private static void DrawToggle(bool enabled, bool hovered, Vector2 pos, Vector2 size)
        {
            var drawList = ImGui.GetWindowDrawList();

            float radius = size.Y * 0.5f;
            float rounding = size.Y * 0.25f;
            float slotHalfHeight = size.Y * 0.5f;

            var background = hovered ? ImGui.GetColorU32(enabled ? ImGuiCol.FrameBgActive : ImGuiCol.FrameBgHovered) : ImGui.GetColorU32(enabled ? ImGuiCol.CheckMark : ImGuiCol.FrameBg);

            var paddingMid = new Vector2(pos.X + radius + (enabled ? 1 : 0) * (size.X - radius * 2), pos.Y + size.Y / 2);
            var sizeMin = new Vector2(pos.X, paddingMid.Y - slotHalfHeight);
            var sizeMax = new Vector2(pos.X + size.X, paddingMid.Y + slotHalfHeight);
            drawList.AddRectFilled(sizeMin, sizeMax, background, rounding);

            var offs = new Vector2(radius * 0.8f, radius * 0.8f);
            drawList.AddRectFilled(paddingMid - offs, paddingMid + offs, ImGui.GetColorU32(ImGuiCol.SliderGrab), rounding);
        }

        private static bool Toggle(string str_id, ref bool v, Vector2 size)
        {
            ImGui.PushStyleColor(ImGuiCol.Button, ImGui.GetColorU32(new Vector4()));

            var style = ImGui.GetStyle();

            ImGui.PushID(str_id);
            bool status = ImGui.Button("###toggle_button", size);
            if (status)
            {
                v = !v;
            }
            ImGui.PopID();

            var maxRect = ImGui.GetItemRectMax();
            var toggleSize = new Vector2(size.X - 8, size.Y - 8);
            var togglePos = new Vector2(maxRect.X - toggleSize.X - style.FramePadding.X, maxRect.Y - toggleSize.Y - style.FramePadding.Y);
            DrawToggle(v, ImGui.IsItemHovered(), togglePos, toggleSize);

            ImGui.PopStyleColor();

            return status;
        }

        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, uint attr, ref int attrValue, int attrSize);

        public void OpenUrl(string url)
        {
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    Process.Start("cmd", "/C start" + " " + url);
                }
                else if (OperatingSystem.IsLinux())
                {
                    Process.Start("xdg-open", url);
                }
                else if (OperatingSystem.IsMacOS())
                {
                    Process.Start("open", url);
                }
            }
            catch
            {
                // do nothing
            }
        }

        public void Start(string version)
        {
            m_window = new Window();
            m_window.Title = $"Cerbios Tool - {version} (Team Resurgent)";
            m_window.Size = new OpenTK.Mathematics.Vector2i(1240, 564);
            m_window.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Fixed;

            if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000, 0))
            {
                int value = -1;
                uint DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
                _ = DwmSetWindowAttribute(GLFW.GetWin32Window(m_window.WindowPtr), DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, sizeof(int));
            }

            m_settings = Settings.LoadSettings();

            m_themes = Theme.LoadThemes();
            m_themeNames = Theme.BuildThemeDropDownList(m_themes);

            m_biosFileOpenPicker = new PathPicker
            {
                Title = "Load Bios",
                Mode = PathPicker.PickerMode.FileOpen,
                AllowedFiles = new[] { "*.bin" },
                ButtonName = "Load"
            };

            m_biosFileSavePicker = new PathPicker
            {
                Title = "Save Bios",
                Mode = PathPicker.PickerMode.FileSave,
                AllowedFiles = new[] { "*.bin" },
                SaveName = "Bios.bin",
                ButtonName = "Save"
            };

            m_configFileOpenPicker = new PathPicker
            {
                Title = "Load Config",
                Mode = PathPicker.PickerMode.FileOpen,
                AllowedFiles = new[] { "*.config" },
                ButtonName = "Load"
            };

            m_configFileSavePicker = new PathPicker
            {
                Title = "Save Config",
                Mode = PathPicker.PickerMode.FileSave,
                AllowedFiles = new[] { "*.config" },
                SaveName = "CerbiosTool.config",
                ButtonName = "Save"
            };

            m_iniFileSavePicker = new PathPicker
            {
                Title = "Save Ini",
                Mode = PathPicker.PickerMode.FileSave,
                AllowedFiles = new[] { "*.init" },
                SaveName = "cerbios.ini",
                ButtonName = "Save"
            };

            m_okDialog = new();

            m_window.RenderUI = RenderUI;
            m_window.Run();
        }

        private void RenderUI()
        {
            if (m_window == null ||
                m_window.Controller == null ||
                m_biosFileOpenPicker == null ||
                m_biosFileSavePicker == null ||
                m_configFileOpenPicker == null ||
                m_configFileSavePicker == null ||
                m_iniFileSavePicker == null ||
                m_okDialog == null)
            {
                return;
            }

            if (m_biosFileOpenPicker.Render() && !m_biosFileOpenPicker.Cancelled)
            {
                m_settings.BiosPath = m_biosFileOpenPicker.SelectedFolder;
                m_settings.BiosFile = m_biosFileOpenPicker.SelectedFile;
                m_biosLoaded = BiosUtility.LoadBiosComfig(Path.Combine(m_biosFileOpenPicker.SelectedFolder, m_biosFileOpenPicker.SelectedFile), ref m_config, ref m_biosData);
                Settings.SaveSattings(m_settings);
                m_themeNames[0] = "Current";
            }

            if (m_biosFileSavePicker.Render() && !m_biosFileSavePicker.Cancelled && string.IsNullOrEmpty(m_settings.BiosPath) == false)
            {
                var savePath = Path.Combine(m_biosFileSavePicker.SelectedFolder, m_biosFileSavePicker.SaveName);
                BiosUtility.SaveBiosConfig(m_config, Path.Combine(m_settings.BiosPath, m_settings.BiosFile), savePath, m_biosData);
            }

            if (m_configFileOpenPicker.Render() && !m_configFileOpenPicker.Cancelled)
            {
                m_settings.ConfigPath = m_configFileOpenPicker.SelectedFolder;
                m_config = Config.LoadConfiguration(Path.Combine(m_configFileOpenPicker.SelectedFolder, m_configFileOpenPicker.SelectedFile));
                Settings.SaveSattings(m_settings);
            }

            if (m_configFileSavePicker.Render() && !m_configFileSavePicker.Cancelled)
            {
                m_settings.ConfigPath = m_configFileOpenPicker.SelectedFolder;
                var savePath = Path.Combine(m_configFileSavePicker.SelectedFolder, m_configFileSavePicker.SaveName);
                Config.SaveConfiguration(savePath, m_config);
                Settings.SaveSattings(m_settings);
            }

            if (m_iniFileSavePicker.Render() && !m_iniFileSavePicker.Cancelled)
            {
                var iniFile = new StringBuilder();
                iniFile.AppendLine("; Cerbios Config");
                iniFile.AppendLine();

                iniFile.AppendLine("; Check For AV Pack");
                iniFile.AppendLine($"AVCheck = {(m_config.AVCheck == 1 ? "true" : "false")}");
                iniFile.AppendLine();

                iniFile.AppendLine("; LED Ring Color, G = Green, R = Red, A = Amber, O = Off");
                iniFile.AppendLine($"FrontLed = {m_config.FrontLed}");
                iniFile.AppendLine();

                iniFile.AppendLine("; Drive Setup");
                iniFile.AppendLine("; 0 = HDD & DVD,  1 = HDD & No DVD (Legacy Mode), 2 = HDD & No DVD (Modern Mode), 3 = Dual HDD");
                iniFile.AppendLine($"DriveSetup = {m_config.DriveSetup}");
                iniFile.AppendLine();

                iniFile.AppendLine("; Load XDK Launcher/XBDM if it exists (Debug Bios Only)");
                iniFile.AppendLine($"Debug = {(m_config.Debug == 1 ? "true" : "false")}");
                iniFile.AppendLine();

                iniFile.AppendLine("; CD Paths (always falls back to D:\\default.xbe)");
                iniFile.AppendLine($"CdPath1 = {m_config.CdPath1}");
                iniFile.AppendLine($"CdPath2 = {m_config.CdPath2}");
                iniFile.AppendLine($"CdPath3 = {m_config.CdPath3}");
                iniFile.AppendLine();

                iniFile.AppendLine("; Dash Paths (always falls back to C:\\xboxdash.xbe)");
                iniFile.AppendLine($"DashPath1 = {m_config.DashPath1}");
                iniFile.AppendLine($"DashPath2 = {m_config.DashPath2}");
                iniFile.AppendLine($"DashPath3 = {m_config.DashPath3}");
                iniFile.AppendLine();

                iniFile.AppendLine("; Boot Animation Path (always falls back to C:\\BootAnims\\Xbox\\bootanim.xbe)");
                iniFile.AppendLine($"BootAnimPath = {m_config.BootAnimPath}");

                var iniPath = Path.Combine(m_iniFileSavePicker.SelectedFolder, m_iniFileSavePicker.SaveName);
                File.WriteAllText(iniPath, iniFile.ToString()); 
            }

            m_okDialog.Render();

            m_splashDialog.Render();

            if (m_showSplash)
            {
                m_showSplash = false;
                m_splashDialog.ShowdDialog(m_window.Controller.SplashTexture);
            }


            ImGui.Begin("Main", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize);
            ImGui.SetWindowSize(new Vector2(m_window.Width, m_window.Height - 24));
            ImGui.SetWindowPos(new Vector2(0, 24), ImGuiCond.Always);

            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Load Bios", "CTRL+L")) 
                    {
                        var path = Directory.Exists(m_settings.BiosPath) ? m_settings.BiosPath : Directory.GetCurrentDirectory();
                        m_biosFileOpenPicker.ShowModal(path);
                    }

                    if (ImGui.MenuItem("Save Bios", "CTRL+S", false, m_biosLoaded))
                    {
                        var path = Directory.Exists(m_settings.BiosPath) ? m_settings.BiosPath : Directory.GetCurrentDirectory();
                        m_biosFileSavePicker.ShowModal(path);
                    }

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Config"))
                {
                    if (ImGui.MenuItem("Load Config", "ALT+L", false, m_biosLoaded))
                    {
                        var path = Directory.Exists(m_settings.ConfigPath) ? m_settings.ConfigPath : Directory.GetCurrentDirectory();
                        m_configFileOpenPicker.ShowModal(path);
                    }

                    if (ImGui.MenuItem("Save Config", "ALT+S", false, m_biosLoaded))
                    {
                        var path = Directory.Exists(m_settings.ConfigPath) ? m_settings.ConfigPath : Directory.GetCurrentDirectory();
                        m_configFileSavePicker.ShowModal(path);
                    }

                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }

            ImGui.Spacing();

            ImGui.SetCursorPosY(10);
            ImGui.BeginChild(1, new Vector2(230 + 50, 480), true, ImGuiWindowFlags.AlwaysUseWindowPadding);

            if (!m_biosLoaded)
            {
                ImGui.BeginDisabled();
            }

            var fontAtlas = ImGui.GetIO().Fonts;
            var largeFont = fontAtlas.Fonts[1];

            ImGui.PushStyleColor(ImGuiCol.Text, ImGui.ColorConvertFloat4ToU32(new Vector4(1.0f, 0.25f, 0.5f, 1.0f)));
            ImGui.PushFont(largeFont);
            ImGui.Text("NOTE: If loading from config, the\nsettings below where applicable will be\noverridden by the cerbios.ini file.");
            ImGui.PopFont();
            ImGui.PopStyleColor();

            ImGui.Spacing();

            var loadConfig = m_config.LoadConfig == 1;
            ImGui.Text("Load Config From Harddrive:");
            Toggle("##loadConfig", ref loadConfig, new Vector2(38, 20));
            m_config.LoadConfig = (byte)(loadConfig ? 1 : 0);

            string[] driveSetupModes = new string[] { "HDD & DVD", "HDD & No DVD (Legacy Mode)", "HDD & No DVD (Modern Mode)", "Dual HDD" };
            var driveSetup = (int)m_config.DriveSetup;
            ImGui.Text("Drive Setup:");
            ImGui.PushItemWidth(250);
            ImGui.Combo("##driveSetup", ref driveSetup, driveSetupModes, driveSetupModes.Length);
            ImGui.PopItemWidth();
            m_config.DriveSetup = (byte)driveSetup;

            var avCheck = m_config.AVCheck == 1;
            ImGui.Text("AV Check:");
            Toggle("##avCheck", ref avCheck, new Vector2(38, 20));
            m_config.AVCheck = (byte)(avCheck ? 1 : 0);

            var debug = m_config.Debug == 1;
            ImGui.Text("Debug:");
            Toggle("##debug", ref debug, new Vector2(38, 20));
            m_config.Debug = (byte)(debug ? 1 : 0);

            var cdPath1 = m_config.CdPath1;
            ImGui.Text("Cd Path 1:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##cdPath1", ref cdPath1, 99))
            {
                m_config.CdPath1 = cdPath1;
            }
            ImGui.PopItemWidth();

            var cdPath2 = m_config.CdPath2;
            ImGui.Text("Cd Path 2:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##cdPath2", ref cdPath2, 99))
            {
                m_config.CdPath2 = cdPath2;
            }
            ImGui.PopItemWidth();

            var cdPath3 = m_config.CdPath3;
            ImGui.Text("Cd Path 3:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##cdPath3", ref cdPath3, 99))
            {
                m_config.CdPath3 = cdPath3;
            }
            ImGui.PopItemWidth();

            var dashPath1 = m_config.DashPath1;
            ImGui.Text("Dash Path 1:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##dashPath1", ref dashPath1, 99))
            {
                m_config.DashPath1 = dashPath1;
            }
            ImGui.PopItemWidth();

            var dashPath2 = m_config.DashPath2;
            ImGui.Text("Dash Path 2:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##dashPath2", ref dashPath2, 99))
            {
                m_config.DashPath2 = dashPath2;
            }
            ImGui.PopItemWidth();

            var dashPath3 = m_config.DashPath3;
            ImGui.Text("Dash Path 3:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##dashPath3", ref dashPath3, 99))
            {
                m_config.DashPath3 = dashPath3;
            }
            ImGui.PopItemWidth();

            var bootAnimPath = m_config.BootAnimPath;
            ImGui.Text("Boot Anim Path:");
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##bootAnimPath", ref bootAnimPath, 99))
            {
                m_config.BootAnimPath = bootAnimPath;
            }
            ImGui.PopItemWidth();

            var frontLed = m_config.FrontLed;
            ImGui.Text("Front LED:");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted("G = Green, R = Red, A = Amber, O = Off");
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##frontLed", ref frontLed, 4))
            {
                var result = string.Empty;
                frontLed = frontLed.ToUpper();
                for (var i =0; i < frontLed.Length; i++)
                {
                    if (!frontLed[i].Equals('G') && !frontLed[i].Equals('R') && !frontLed[i].Equals('A') && !frontLed[i].Equals('O')) {
                        continue;
                    }
                    result += frontLed[i];
                    result.PadRight(4, 'O');
                }
                m_config.FrontLed = result;
            }
            ImGui.PopItemWidth();

            string[] fanSpeeds = new string[] { "Auto", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" };
            var fanSpeed = m_config.FanSpeed / 10;
            ImGui.Text("Fan Speed:");
            ImGui.PushItemWidth(250);
            ImGui.Combo("##fanSpeed", ref fanSpeed, fanSpeeds, fanSpeeds.Length);
            ImGui.PopItemWidth();
            m_config.FanSpeed = (byte)(fanSpeed * 10);

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            ImGui.PushStyleColor(ImGuiCol.Text, ImGui.ColorConvertFloat4ToU32(new Vector4(1.0f, 0.25f, 0.5f, 1.0f)));
            ImGui.PushFont(largeFont);
            ImGui.Text("NOTE: The following settings are not\napplicable in the cerbios.ini as used\nbefore loading stage.");
            ImGui.PopFont();
            ImGui.PopStyleColor();

            ImGui.Spacing();

            string[] igrMasterPorts = new string[] { "All", "1", "2", "3", "4" };
            var igrMasterPort = (int)m_config.IGRMasterPort;
            ImGui.Text("IGR Master Port:");
            ImGui.PushItemWidth(250);
            ImGui.Combo("##igrMasterPort", ref igrMasterPort, igrMasterPorts, igrMasterPorts.Length);
            ImGui.PopItemWidth();
            m_config.IGRMasterPort = (byte)igrMasterPort;

            var igrDash = m_config.IGRDash;
            ImGui.Text("IGR Dash:");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted("A = 0, B = 1, X = 2, Y = 3, BLACK = 4, WHITE = 5, L-TRIGGER = 6, R-TRIGGER = 7\nUP = 8, DOWN = 9, LEFT = A, RIGHT = B, START = C, BACK = D, L-THUMB = E, R-THUMB = F");
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##igrDash", ref igrDash, 4, ImGuiInputTextFlags.CharsHexadecimal))
            {
                m_config.IGRDash = igrDash;
            }
            ImGui.PopItemWidth();

            var igrGame = m_config.IGRGame;
            ImGui.Text("IGR Game:");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted("A = 0, B = 1, X = 2, Y = 3, BLACK = 4, WHITE = 5, L-TRIGGER = 6, R-TRIGGER = 7\nUP = 8, DOWN = 9, LEFT = A, RIGHT = B, START = C, BACK = D, L-THUMB = E, R-THUMB = F");
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##igrGame", ref igrGame, 4, ImGuiInputTextFlags.CharsHexadecimal))
            {
                m_config.IGRGame = igrGame;
            }
            ImGui.PopItemWidth();

            var igrFull = m_config.IGRFull;
            ImGui.Text("IGR Full:");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted("A = 0, B = 1, X = 2, Y = 3, BLACK = 4, WHITE = 5, L-TRIGGER = 6, R-TRIGGER = 7\nUP = 8, DOWN = 9, LEFT = A, RIGHT = B, START = C, BACK = D, L-THUMB = E, R-THUMB = F");
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##igrFull", ref igrFull, 4, ImGuiInputTextFlags.CharsHexadecimal))
            {
                m_config.IGRFull = igrFull;
            }
            ImGui.PopItemWidth();

            var igrShutdown = m_config.IGRShutdown;
            ImGui.Text("IGR Shutdown:");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted("A = 0, B = 1, X = 2, Y = 3, BLACK = 4, WHITE = 5, L-TRIGGER = 6, R-TRIGGER = 7\nUP = 8, DOWN = 9, LEFT = A, RIGHT = B, START = C, BACK = D, L-THUMB = E, R-THUMB = F");
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
            ImGui.PushItemWidth(250);
            if (ImGui.InputText("##igrFull", ref igrShutdown, 4, ImGuiInputTextFlags.CharsHexadecimal))
            {
                m_config.IGRShutdown = igrShutdown;
            }
            ImGui.PopItemWidth();

            string[] udmaModes = new string[] { "Auto (Startech Adapter)", "Auto (Generic Adapter)", "UDMA 2 (Default / Stock)", "UDMA 3 (Ultra DMA 80-Conductor)", "UDMA 4 (Ultra DMA 80-Conductor)", "UDMA 5 (Ultra DMA 80-Conductor)", "UDMA 6 (Experimental)" };
            var udmaMode = (int)m_config.UDMAMode;
            ImGui.Text("UDMA Mode:");
            ImGui.PushItemWidth(250);
            if (ImGui.Combo("##udmaMode", ref udmaMode, udmaModes, udmaModes.Length))
            {
                if (udmaMode < 2)
                {
                    m_okDialog.Title = "WARNING";
                    m_okDialog.Message = "Auto modes require a Ultra DMA (80-Conductor) IDE/ATA Cable\nto be installed. Using this mode without one can cause your\nXBOX not to boot.\nSafe Mode (Boot with Eject), will allow you to boot using UDMA 2 \nto perform a reflash if required.";
					m_okDialog.ShowModal();
                }
				else if (udmaMode == 6)
				{
					m_okDialog.Title = "WARNING";
					m_okDialog.Message = "UDMA Mode 6 is EXPERIMENTAL and requires a Ultra DMA\n(80-Conductor) IDE/ATA Cable to be installed.\nThis has ONLY been tested with Startech Adapter.\nSafe Mode (Boot with Eject), will allow you to boot using UDMA 2 \nto perform a reflash if required.\n";
					m_okDialog.ShowModal();
				}
				
			}
            ImGui.PopItemWidth();
            m_config.UDMAMode = (byte)(udmaMode);

            if (!m_biosLoaded)
            {
                ImGui.EndDisabled();
            }

            ImGui.EndChild();

            var startPos = new Vector2(250 + 48, 34);
            var imagePos = new Vector2(startPos.X, 10);
            var size = new Vector2(640, 480);
            var drawList = ImGui.GetWindowDrawList();
            drawList.AddRectFilled(startPos, startPos + size, ImGui.ColorConvertFloat4ToU32(Config.RGBToVector4(m_config.SplashBackground)));
            drawList.AddRect(startPos, startPos + size, ImGui.ColorConvertFloat4ToU32(new Vector4(0.5f, 0.5f, 0.5f, 0.5f)));

            if (m_config.SplashScale > 0)
            {
                
                var scaledSize = size / m_config.SplashScale;
                var scaleOffset = (size - scaledSize) / 2;
                ImGui.SetCursorPos(imagePos + scaleOffset);
                ImGui.Image(m_window.Controller.Logo1Texture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashLogo1));
                ImGui.SetCursorPos(imagePos + scaleOffset);
                ImGui.Image(m_window.Controller.Logo2Texture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashLogo2));
                ImGui.SetCursorPos(imagePos + scaleOffset);
                ImGui.Image(m_window.Controller.Logo3Texture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashLogo3));
                ImGui.SetCursorPos(imagePos + scaleOffset);
                ImGui.Image(m_window.Controller.Logo4Texture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashLogo4));
                ImGui.SetCursorPos(imagePos + scaleOffset);
                ImGui.Image(m_window.Controller.CerbiosTextTexture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashCerbiosText));
                ImGui.SetCursorPos(imagePos + scaleOffset);
                ImGui.Image(m_window.Controller.SafeModeTextTexture, scaledSize, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashSafeModeText));
            }
            else 
            {
                ImGui.SetCursorPos(imagePos);
                ImGui.Image(m_window.Controller.SafeModeTextTexture, size, Vector2.Zero, Vector2.One, Config.RGBToVector4(m_config.SplashSafeModeText));
            }

            ImGui.SetCursorPos(new Vector2(950, 10));
            ImGui.BeginChild(2, new Vector2(280, 480), true, ImGuiWindowFlags.AlwaysUseWindowPadding);

            if (!m_biosLoaded)
            {
                ImGui.BeginDisabled();
            }

            int theme = 0;
            ImGui.Text("Theme:");
            ImGui.PushItemWidth(250);
            ImGui.Combo("##theme", ref theme, m_themeNames, m_themeNames.Length);
            ImGui.PopItemWidth();
            if (theme > 0)
            {
                m_config.SetTheme(m_themes[theme - 1]);
                m_themeNames[0] = m_themeNames[theme];
            }

            var splashBackground = Config.RGBToVector3(m_config.SplashBackground);
            ImGui.Text("Splash Background:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashBackground", ref splashBackground, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashBackground = Config.Vector3ToRGB(splashBackground);

            var splashCerbiosText = Config.RGBToVector3(m_config.SplashCerbiosText);
            ImGui.Text("Splash Cerbios Text:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashCerbiosText", ref splashCerbiosText, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashCerbiosText = Config.Vector3ToRGB(splashCerbiosText);

            var splashSafeModeText = Config.RGBToVector3(m_config.SplashSafeModeText);
            ImGui.Text("Splash SafeMode Text:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashSafeModeText", ref splashSafeModeText, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashSafeModeText = Config.Vector3ToRGB(splashSafeModeText);

            var splashLogo1 = Config.RGBToVector3(m_config.SplashLogo1);
            ImGui.Text("Splash Logo1:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashLogo1", ref splashLogo1, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashLogo1 = Config.Vector3ToRGB(splashLogo1);

            var splashLogo2 = Config.RGBToVector3(m_config.SplashLogo2);
            ImGui.Text("Splash Logo2:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashLogo2", ref splashLogo2, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashLogo2 = Config.Vector3ToRGB(splashLogo2);

            var splashLogo3 = Config.RGBToVector3(m_config.SplashLogo3);
            ImGui.Text("Splash Logo3:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashLogo3", ref splashLogo3, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashLogo3 = Config.Vector3ToRGB(splashLogo3);

            var splashLogo4 = Config.RGBToVector3(m_config.SplashLogo4);
            ImGui.Text("Splash Logo4:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit3("##splashLogo4", ref splashLogo4, ImGuiColorEditFlags.DisplayHex);
            ImGui.PopItemWidth();
            m_config.SplashLogo4 = Config.Vector3ToRGB(splashLogo4);

            string[] splashScales = new string[] { "Hide", "Large", "Medium", "Small" };
            var splashScale = (int)m_config.SplashScale;
            ImGui.Text("Splash Scale:");
            ImGui.PushItemWidth(250);
            ImGui.Combo("##splashScale", ref splashScale, splashScales, splashScales.Length);
            ImGui.PopItemWidth();
            m_config.SplashScale = (byte)splashScale;

            if (!m_biosLoaded)
            {
                ImGui.EndDisabled();
            }

            ImGui.EndChild();

            ImGui.SetCursorPosY(m_window.Height - 64);

            if (m_biosLoaded)
            {
                if (ImGui.Button("Default Config", new Vector2(100, 30)))
                {
                    m_config.SetDefaults();
                }

                ImGui.SameLine();

                if (ImGui.Button("Copy Theme JSON", new Vector2(150, 30)))
                {
                    var themeJson = new StringBuilder();
                    themeJson.AppendLine("{");
                    themeJson.AppendLine("    \"Name\":\"Theme Name\",");
                    themeJson.AppendLine($"    \"SplashBackground\":\"{m_config.SplashBackground.ToString("X6")}\",");
                    themeJson.AppendLine($"    \"SplashCerbiosText\":\"{m_config.SplashCerbiosText.ToString("X6")}\",");
                    themeJson.AppendLine($"    \"SplashSafeModeText\":\"{m_config.SplashSafeModeText.ToString("X6")}\",");
                    themeJson.AppendLine($"    \"SplashLogo1\":\"{m_config.SplashLogo1.ToString("X6")}\",");
                    themeJson.AppendLine($"    \"SplashLogo2\":\"{m_config.SplashLogo2.ToString("X6")}\",");
                    themeJson.AppendLine($"    \"SplashLogo3\":\"{m_config.SplashLogo3.ToString("X6")}\",");
                    themeJson.AppendLine($"    \"SplashLogo4\":\"{m_config.SplashLogo4.ToString("X6")}\"");
                    themeJson.AppendLine($"    \"SplashScale\":{m_config.SplashScale}");
                    themeJson.AppendLine("}");
                    ImGui.SetClipboardText(themeJson.ToString());
                }

                ImGui.SameLine();

                if (ImGui.Button("Export cerbios.ini", new Vector2(150, 30)))
                {
                    m_iniFileSavePicker.ShowModal(Directory.GetCurrentDirectory());
                }

                ImGui.SameLine();
            }

            ImGui.SetCursorPosX(m_window.Width - 374);

            if (ImGui.Button("Patreon", new Vector2(100, 30)))
            {
                OpenUrl("https://www.patreon.com/teamresurgent");
            }

            ImGui.SameLine();

            if (ImGui.Button("Ko-Fi", new Vector2(100, 30)))
            {
                OpenUrl("https://ko-fi.com/teamresurgent");
            }

            ImGui.SameLine();

            if (ImGui.Button("Coded by EqUiNoX", new Vector2(150, 30)))
            {
                OpenUrl("https://github.com/Team-Resurgent/CerbiosTool");
            }

            ImGui.End();
        }
    }
}
